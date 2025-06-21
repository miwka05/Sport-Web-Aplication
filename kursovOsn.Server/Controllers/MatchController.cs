using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using kursovOsn.Server.Models;
using Microsoft.EntityFrameworkCore;
using kursovOsn.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace kursovOsn.Server.Controllers
{
    // MatchController.cs
    [ApiController]
    [Route("api/match")]
    public class MatchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MatchController> _logger;

        public MatchController(ApplicationDbContext context, ILogger<MatchController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null) return NotFound();

            return Ok(new
            {
                match.Id,
                match.Data,
                match.Time,
                Team1 = new { match.Team1?.Id, match.Team1?.Name },
                Team2 = new { match.Team2?.Id, match.Team2?.Name }
            });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateMatchWithStageDto dto)
        {
            if (dto.Team1_ID == dto.Team2_ID)
                return BadRequest("Команды не могут быть одинаковыми.");

            if (!DateTime.TryParse(dto.Data, out var date))
                return BadRequest("Неверный формат даты.");

            if (!TimeSpan.TryParse(dto.Time, out var time))
                return BadRequest("Неверный формат времени.");

            var tournament = await _context.Tournaments
                .FirstOrDefaultAsync(t => t.ID == dto.Tournament_ID);

            if (tournament == null)
                return NotFound("Турнир не найден");

            var stage = new Tournament_Stages
            {
                Tournament_ID = tournament.ID,
                Name = dto.Stage_Name,
                Stage_order = dto.Stage_Order
            };

            _context.Tournament_Stages.Add(stage);
            await _context.SaveChangesAsync(); // получить stage.ID

            var match = new Match
            {
                Tournament_ID = tournament.ID,
                Team1_ID = dto.Team1_ID,
                Team2_ID = dto.Team2_ID,
                Data = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                Time = time,
                Sport_ID = tournament.Sport_ID,
                Stage_ID = stage.ID
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync(); // получить match.Id

            // Получаем статистики по виду спорта
            var stats = await _context.Sport_Statistics
                .Where(s => s.Sport_ID == tournament.Sport_ID)
                .ToListAsync();

            // Добавляем нулевую статистику для команд
            var teamStats = stats.SelectMany(stat => new[]
            {
        new Team_Statistic
        {
            Match_ID = match.Id,
            Team_ID = dto.Team1_ID,
            Stat_ID = stat.ID,
            Stats = 0
        },
        new Team_Statistic
        {
            Match_ID = match.Id,
            Team_ID = dto.Team2_ID,
            Stat_ID = stat.ID,
            Stats = 0
        }
    }).ToList();

            // Если турнир одиночный — добавляем нулевую статистику игрокам
            List<Player_Statistic> playerStats = new();
            if (tournament.solo == true)
            {
                var players = await _context.Tournament_Participants
                    .Where(p => p.Tournament_ID == tournament.ID)
                    .Select(p => p.User_ID)
                    .Distinct()
                    .ToListAsync();

                foreach (var playerId in players)
                {
                    playerStats.AddRange(stats.Select(stat => new Player_Statistic
                    {
                        Match_ID = match.Id,
                        Player_ID = playerId,
                        Stat_ID = stat.ID,
                        Stats = 0
                    }));
                }
            }

            _context.Team_Statistics.AddRange(teamStats);
            _context.Player_Statistics.AddRange(playerStats);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Матч и статистика успешно созданы", match.Id });
        }

        [HttpGet("{id}/team-stats")]
        public async Task<IActionResult> GetTeamStats(int id)
        {
            var teamStats = await _context.Team_Statistics
                .Where(ts => ts.Match_ID == id)
                .Include(ts => ts.Team)
                .Include(ts => ts.Sport_Statistic) 
                .ToListAsync();

            var grouped = teamStats
                .GroupBy(ts => ts.Team_ID)
                .Select(g => new
                {
                    teamId = g.Key,
                    teamName = g.FirstOrDefault()?.Team?.Name,
                    stats = g.Select(s => new
                    {
                        statId = s.Stat_ID,
                        statName = s.Sport_Statistic.Stat_Name,
                        value = s.Stats
                    }).ToList()
                }).ToList();

            return Ok(grouped);
        }

        [HttpPost("{id}/score")]
        [Authorize]
        public async Task<IActionResult> UpdateScore(int id, [FromBody] MatchScoreDto dto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound("Матч не найден");

            // Обновляем счёт матча
            match.Team1_Score = dto.Team1Score;
            match.Team2_Score = dto.Team2Score;

            // Вычисляем победителей
            if (dto.Team1Score > dto.Team2Score)
            {
                match.Winner = "Team1";
            }
            else if (dto.Team2Score > dto.Team1Score)
            {
                match.Winner = "Team2";
            }
            else
            {
                match.Winner = "Draw";
            }

            await _context.SaveChangesAsync();

            // Обновляем турнирную таблицу
            await UpdateTournamentStandings(match.Tournament_ID);

            return Ok(new { message = "Счёт обновлён успешно", match.Team1_Score, match.Team2_Score });
        }

        private async Task UpdateTournamentStandings(int tournamentId)
        {
            var matches = await _context.Matches
                .Where(m => m.Tournament_ID == tournamentId && m.Team1_ID != null && m.Team2_ID != null)
                .ToListAsync();

            var standings = await _context.Tournament_Standings
                .Where(s => s.Tournament_ID == tournamentId)
                .ToListAsync();

            // Сбрасываем показатели перед перерасчетом
            foreach (var s in standings)
            {
                s.Matches = 0;
                s.Wins = 0;
                s.Draws = 0;
                s.Losses = 0;
                s.Points = 0;
                s.Goals_Scored = 0;
                s.Goals_Conceded = 0;
            }

            foreach (var match in matches)
            {
                var team1 = standings.FirstOrDefault(s => s.Team_ID == match.Team1_ID);
                var team2 = standings.FirstOrDefault(s => s.Team_ID == match.Team2_ID);

                if (team1 == null || team2 == null) continue;

                team1.Matches += 1;
                team2.Matches += 1;

                if (match.Team1_Score.HasValue && match.Team2_Score.HasValue)
                {
                    team1.Goals_Scored += match.Team1_Score.Value;
                    team1.Goals_Conceded += match.Team2_Score.Value;
                }

                if (match.Team1_Score.HasValue && match.Team2_Score.HasValue)
                {
                    team2.Goals_Scored += match.Team2_Score.Value;
                    team2.Goals_Conceded += match.Team1_Score.Value;
                }

                switch (match.Winner)
                {
                    case "Team1":
                        team1.Wins += 1;
                        team1.Points += 3;
                        team2.Losses += 1;
                        break;

                    case "Team2":
                        team2.Wins += 1;
                        team2.Points += 3;
                        team1.Losses += 1;
                        break;

                    case "Draw":
                        team1.Draws += 1;
                        team2.Draws += 1;
                        team1.Points += 1;
                        team2.Points += 1;
                        break;
                }
            }

            await _context.SaveChangesAsync();
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetScore(int id)
        {
            var match = await _context.Matches
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound("Матч не найден");

            return Ok(new
            {
                team1Goals = match.Team1_Score ?? 0,
                team2Goals = match.Team2_Score ?? 0
            });
        }

        [HttpPost("{id}/update-stats")]
        [Authorize]
        public async Task<IActionResult> UpdateStats(int id, [FromBody] MatchStatsUpdateDto dto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound("Матч не найден");

            if (dto.TeamStats != null)
            {
                foreach (var teamStat in dto.TeamStats)
                {
                    var sportStat = await _context.Sport_Statistics
                        .FirstOrDefaultAsync(s => s.ID == teamStat.StatId);

                    if (sportStat == null)
                    {
                        return BadRequest($"StatId {teamStat.StatId} не существует в таблице Sport_Statistics.");
                    }

                    var stat = await _context.Team_Statistics
                        .FirstOrDefaultAsync(ts => ts.Match_ID == id && ts.Team_ID == teamStat.TeamId && ts.Stat_ID == teamStat.StatId);

                    if (stat != null)
                    {
                        stat.Stats = teamStat.Value;
                    }
                    else
                    {
                        _context.Team_Statistics.Add(new Team_Statistic
                        {
                            Match_ID = id,
                            Team_ID = teamStat.TeamId,
                            Stat_ID = teamStat.StatId,
                            Stats = teamStat.Value
                        });
                    }
                }
            }

            if (dto.PlayerStats != null)
            {
                foreach (var playerStat in dto.PlayerStats)
                {
                    var sportStat = await _context.Sport_Statistics
                        .FirstOrDefaultAsync(s => s.ID == playerStat.StatId);

                    if (sportStat == null)
                    {
                        return BadRequest($"StatId {playerStat.StatId} не существует в таблице Sport_Statistics.");
                    }

                    var stat = await _context.Player_Statistics
                        .FirstOrDefaultAsync(ps => ps.Match_ID == id && ps.Player_ID == playerStat.PlayerId && ps.Stat_ID == playerStat.StatId);

                    if (stat != null)
                    {
                        stat.Stats = playerStat.Value;
                    }
                    else
                    {
                        _context.Player_Statistics.Add(new Player_Statistic
                        {
                            Match_ID = id,
                            Player_ID = playerStat.PlayerId,
                            Stat_ID = playerStat.StatId,
                            Stats = playerStat.Value
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Статистика успешно обновлена" });
        }



        [HttpGet("{id}/player-stats")]
        public async Task<IActionResult> GetPlayerStats(int id)
        {
            var playerStats = await _context.Player_Statistics
                .Where(ps => ps.Match_ID == id)
                .Include(ps => ps.Player)
                .Include(ps => ps.Sport_Statistic)
                .ToListAsync();

            var result = playerStats
                .GroupBy(ps => ps.Player_ID)
                .Select(group => new
                {
                    PlayerId = group.Key,
                    PlayerName = group.FirstOrDefault()?.Player?.UserName,
                    Stats = group.Select(ps => new
                    {
                        StatName = ps.Sport_Statistic.Stat_Name,
                        Value = ps.Stats
                    }).ToList()
                }).ToList();

            return Ok(result);
        }


        [HttpGet("tournament/{tournamentId}")]
        public async Task<IActionResult> GetByTournament(int tournamentId)
        {
            var matches = await _context.Matches
                .Where(m => m.Tournament_ID == tournamentId)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .ToListAsync();

            return Ok(matches.Select(m => new
            {
                m.Id,
                m.Data,
                m.Time,
                Team1 = m.Team1?.Name,
                Team2 = m.Team2?.Name
            }));
        }
    }

}

