// TournamentController.cs
using Humanizer;
using kursovOsn.Server.Data;
using kursovOsn.Server.Models;
using kursovOsn.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace kursovOsn.Server.Controllers
{
    [Route("api/tournament")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public TournamentController(ITournamentService tournamentService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _tournamentService = tournamentService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetAll()
        {
            var tournaments = await _tournamentService.GetAllTournamentsAsync();
            return Ok(tournaments);
        }

        [Authorize]
        [HttpPost("create-tournament")]
        public async Task<ActionResult<Tournament>> Create([FromBody] CreateTournamentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.Start = DateTime.SpecifyKind(dto.Start, DateTimeKind.Utc);
            dto.End = DateTime.SpecifyKind(dto.End, DateTimeKind.Utc);

            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);

            var tournament = new Tournament
            {
                Name = dto.Name,
                Sport_ID = dto.Sport_ID,
                Adress = dto.Adress,
                Age = dto.Age,
                Info = dto.Info,
                Pol = dto.Pol,
                Status = "Created",
                Start = dto.Start,
                End = dto.End,
                Format_ID = dto.Format_ID,
                Creator_ID = user.Id,
                solo = dto.Solo,
                ban = false
            };

            var created = await _tournamentService.CreateTournamentAsync(tournament);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tournament = await _tournamentService.GetTournamentByIdAsync(id);

            if (tournament == null)
                return NotFound();

            return Ok(new
            {
                tournament.ID,
                tournament.Name,
                tournament.Sport,
                tournament.Format,
                tournament.Creator_ID,
                CreatorUserName = tournament.Creator?.UserName,  // Передаем имя создателя
                tournament.Start,
                tournament.End,
                tournament.Adress,
                tournament.Age,
                tournament.Info,
                tournament.Pol,
                tournament.Status,
                tournament.solo,
                tournament.ban,
                tournament.reasonBan
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Tournament>> Update(int id, [FromBody] Tournament tournament)
        {
            if (id != tournament.ID)
                return BadRequest();

            var updated = await _tournamentService.UpdateTournamentAsync(id, tournament);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _tournamentService.DeleteTournamentAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> RegisterToTournament([FromBody] CreateTournamentEntryDto dto)
        {
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userName == null) return Unauthorized();

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound("User not found");

            var entry = new Tournament_Entry
            {
                ID_Tournament = dto.TournamentId,
                Info = dto.Info
            };

            if (dto.TeamId.HasValue)
            {
                var team = await _context.Teams.FindAsync(dto.TeamId.Value);
                if (team == null || team.Creator_ID != user.Id)
                    return BadRequest("Вы не являетесь владельцем команды или команда не найдена");

                entry.ID_Team = dto.TeamId.Value;
            }
            else
            {
                entry.ID_User = user.Id;
            }

            entry.Status = "Pending";
            _context.Tournament_Entries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Заявка успешно подана" });
        }


        [HttpGet("{tournamentId}/entries")]
        [Authorize]
        public async Task<IActionResult> GetEntries(int tournamentId)
        {
            var entries = await _context.Tournament_Entries
                .Include(e => e.User)
                .Include(e => e.Team)
                .Where(e => e.ID_Tournament == tournamentId && e.Status == "Pending")
                .Select(e => new
                {
                    Id = e.ID,
                    UserName = e.User.UserName,
                    UserId = e.User.Id, 
                    TeamName = e.Team != null ? e.Team.Name : null,
                    TeamId = e.Team != null ? e.Team.Id : 0,
                    e.Info
                })
                .ToListAsync();

            return Ok(entries);
        }

        [HttpPost("entry/{entryId}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptEntry(int entryId)
        {
            var entry = await _context.Tournament_Entries
                .Include(e => e.Tournament)
                .FirstOrDefaultAsync(e => e.ID == entryId);

            if (entry == null) return NotFound();

            if (entry.Status == "Accepted")
                return BadRequest("Заявка уже принята");

            entry.Status = "Accepted";

            var isSolo = entry.Tournament.solo ?? false;

            var participant = new Tournament_participant
            {
                Tournament_ID = entry.ID_Tournament,
                User_ID = entry.ID_User,
                Team_ID = entry.ID_Team
            };
            _context.Tournament_Participants.Add(participant);

            // Добавление строки в таблицу при формате Round Robin
            var roundRobinFormat = await _context.Tournament_Formats
                .FirstOrDefaultAsync(f => f.ID == entry.Tournament.Format_ID && f.Name.ToLower().Contains("round robin"));

            if (roundRobinFormat != null)
            {
                var standing = new Tournament_Standings
                {
                    Tournament_ID = entry.Tournament.ID,
                    Player_ID = isSolo ? entry.ID_User : null,
                    Team_ID = !isSolo ? entry.ID_Team : null
                };
                _context.Tournament_Standings.Add(standing);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Заявка принята" });
        }


        [HttpPost("entry/{entryId}/reject")]
        [Authorize]
        public async Task<IActionResult> RejectEntry(int entryId)
        {
            var entry = await _context.Tournament_Entries.FindAsync(entryId);
            if (entry == null) return NotFound();

            entry.Status = "Rejected";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Заявка отклонена" });
        }


        [HttpGet("{id}/participants")]
        public async Task<IActionResult> GetParticipants(int id)
        {
            var participants = await _context.Tournament_Participants
                .Where(p => p.Tournament_ID == id)
                .Include(p => p.User)
                .Include(p => p.Team)
                .ToListAsync();

            var result = participants.Select(p => new
            {
                ID = p.ID,
                UserName = p.User != null ? p.User.UserName : null,
                TeamName = p.Team != null ? p.Team.Name : null,
                UserId = p.User != null ? p.User.Id : null,
                TeamId = p.Team != null ? p.Team.Id : 0
            });

            return Ok(result);
        }

        [HttpGet("{tournamentId}/team-has-request/{teamId}")]
        [Authorize]
        public async Task<IActionResult> TeamHasRequest(int tournamentId, int teamId)
        {
            var exists = await _context.Tournament_Entries
                .AnyAsync(e => e.ID_Tournament == tournamentId && e.ID_Team == teamId);
            return Ok(exists);
        }

        [HttpGet("{tournamentId}/team-is-participant/{teamId}")]
        [Authorize]
        public async Task<IActionResult> TeamIsParticipant(int tournamentId, int teamId)
        {
            var exists = await _context.Tournament_Participants
                .AnyAsync(p => p.Tournament_ID == tournamentId && p.Team_ID == teamId);

            return Ok(exists);
        }

        [HttpGet("{id}/can-register")]
        [Authorize]
        public async Task<IActionResult> CanRegister(int id)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized("Пользователь не найден");

            if (user.ban == true)
                return BadRequest("Вы заблокированы и не можете регистрироваться на турниры.");

            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.ID == id);
            if (tournament == null)
                return NotFound("Турнир не найден");

            if (tournament.ban == true)
                return BadRequest("Турнир заблокирован администрацией.");

            // Проверка пола
            if (tournament.solo == true && !string.IsNullOrEmpty(tournament.Pol) && tournament.Pol.ToLower() != "любой")
            {
                if (!string.Equals(user.Sex, tournament.Pol, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Вы не соответствуете требованиям по полу для участия в этом турнире.");
                }
            }

            // Проверка возраста
            if (!string.IsNullOrEmpty(tournament.Age))
            {
                var ageStr = tournament.Age.Trim();
                if (ageStr.Length >= 2 &&
                    int.TryParse(ageStr.Substring(0, ageStr.Length - 1), out int requiredAge))
                {
                    char sign = ageStr[^1];
                    var today = DateTime.UtcNow;
                    var userAge = today.Year - user.DateOfBirth.Year;
                    if (user.DateOfBirth > today.AddYears(-userAge)) userAge--;

                    if (sign == '+' && userAge < requiredAge)
                        return BadRequest($"Возрастное ограничение: не младше {requiredAge} лет");

                    if (sign == '-' && userAge > requiredAge)
                        return BadRequest($"Возрастное ограничение: не старше {requiredAge} лет");
                }
            }

            var hasEntry = await _context.Tournament_Entries
                .AnyAsync(e => e.ID_Tournament == id && e.ID_User == user.Id);
            if (hasEntry)
                return BadRequest("Вы уже подали заявку на участие в турнире.");

            var isParticipant = await _context.Tournament_Participants
                .AnyAsync(p => p.Tournament_ID == id && p.User_ID == user.Id);
            if (isParticipant)
                return BadRequest("Вы уже участвуете в турнире.");

            return Ok("ok");
        }



        [HttpGet("my-entries")]
        [Authorize]
        public async Task<IActionResult> GetMyEntries()
        {
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userName == null) return Unauthorized();
            var user = await _userManager.FindByNameAsync(userName);


            var entries = await _context.Tournament_Entries
                .Include(e => e.Tournament)
                .Include(e => e.Team)
                .Include(e => e.User)
                .Where(e => e.ID_User == user.Id || (e.Team != null && e.Team.Creator_ID == user.Id))
                .ToListAsync();

            var result = new
            {
                accepted = entries.Where(e => e.Status == "Accepted").Select(e => new
                {
                    tournamentName = e.Tournament.Name,
                    tournamentId = e.Tournament.ID,
                    teamName = e.Team?.Name,
                    userName = e.User?.UserName
                }),
                pending = entries.Where(e => e.Status == "Pending").Select(e => new
                {
                    tournamentName = e.Tournament.Name,
                    tournamentId = e.Tournament.ID,
                    teamName = e.Team?.Name,
                    userName = e.User?.UserName
                }),
                rejected = entries.Where(e => e.Status == "Rejected").Select(e => new
                {
                    tournamentName = e.Tournament.Name,
                    tournamentId = e.Tournament.ID,
                    teamName = e.Team?.Name,
                    userName = e.User?.UserName
                })
            };

            return Ok(result);
        }

        [HttpGet("user-entries/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserEntries(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);


            var entries = await _context.Tournament_Entries
                .Include(e => e.Tournament)
                .Include(e => e.Team)
                .Include(e => e.User)
                .Where(e => e.ID_User == user.Id || (e.Team != null && e.Team.Creator_ID == user.Id))
                .ToListAsync();

            var result = new
            {
                accepted = entries.Where(e => e.Status == "Accepted").Select(e => new
                {
                    tournamentName = e.Tournament.Name,
                    teamName = e.Team?.Name,
                    userName = e.User?.UserName
                }),
                pending = entries.Where(e => e.Status == "Pending").Select(e => new
                {
                    tournamentName = e.Tournament.Name,
                    teamName = e.Team?.Name,
                    userName = e.User?.UserName
                }),
                rejected = entries.Where(e => e.Status == "Rejected").Select(e => new
                {
                    tournamentName = e.Tournament.Name,
                    teamName = e.Team?.Name,
                    userName = e.User?.UserName
                })
            };

            return Ok(result);
        }

        [HttpGet("{id}/standings")]
        public async Task<IActionResult> GetStandings(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Standings)
                    .ThenInclude(s => s.Team)
                .Include(t => t.Standings)
                    .ThenInclude(s => s.Player)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (tournament == null)
                return NotFound("Турнир не найден");

            var isSolo = tournament.solo ?? false;

            var standings = tournament.Standings
                .Select(s => new
                {
                    Name = isSolo
                        ? s.Player != null ? s.Player.UserName : "(неизвестный игрок)"
                        : s.Team != null ? s.Team.Name : "(неизвестная команда)",
                    s.Matches,
                    s.Wins,
                    s.Draws,
                    s.Losses,
                    s.Points,
                    s.Goals_Scored,
                    s.Goals_Conceded
                })
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.Goals_Scored - s.Goals_Conceded)
                .ToList();

            return Ok(standings);
        }

        [HttpGet("{id}/teams")]
        public async Task<IActionResult> GetTeams(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants) 
                .ThenInclude(tp => tp.Team) 
                .FirstOrDefaultAsync(t => t.ID == id);

            if (tournament == null)
                return NotFound("Tournament not found.");

            // Extract teams from the participants
            var teams = tournament.Participants
                .Where(tp => tp.Team != null)
                .Select(tp => new
                {
                    tp.Team.Id,
                    tp.Team.Name,
                    tp.Team.City,
                    tp.Team.Age
                }).ToList();

            return Ok(teams);
        }

        [HttpGet("{id}/matches")]
        public async Task<IActionResult> GetMatchesByTournamentId(int id)
        {
            var matches = await _context.Matches
                .Where(m => m.Tournament_ID == id)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .ToListAsync();

            var result = matches.Select(m => new
            {
                Id = m.Id,
                Date = m.Data,
                Time = m.Time,
                Team1Name = m.Team1?.Name,
                Team2Name = m.Team2?.Name
            });

            return Ok(result);
        }

        [HttpGet("{id}/userMatches")]
        [Authorize]
        public async Task<IActionResult> GetUseMatchesByTournamentId(int id)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);

            // Получаем все матчи турнира
            var matches = await _context.Matches
                .Where(m => m.Tournament_ID == id)
                .Include(m => m.Team1).ThenInclude(t => t.TeamPlayers)
                .Include(m => m.Team2).ThenInclude(t => t.TeamPlayers)
                .Include(m => m.Player1)  // Если одиночные игры
                .Include(m => m.Player2)  // Если одиночные игры
                .ToListAsync();

            // Фильтруем матчи, чтобы получить только те, в которых участвует текущий пользователь (как игрок или через команду)
            var userMatches = matches.Where(m =>
                (m.Player1 != null && m.Player1.Id == user.Id) || // Игрок 1 в одиночной игре
                (m.Player2 != null && m.Player2.Id == user.Id) || // Игрок 2 в одиночной игре
                (m.Team1 != null && m.Team1.TeamPlayers.Any(p => p.ID_User == user.Id)) || // Команда 1
                (m.Team2 != null && m.Team2.TeamPlayers.Any(p => p.ID_User == user.Id))   // Команда 2
            ).Select(m => new
            {
                Id = m.Id,
                Date = m.Data,
                Time = m.Time,
                Team1Name = m.Team1?.Name,
                Team2Name = m.Team2?.Name,
                Player1Name = m.Player1?.UserName,
                Player2Name = m.Player2?.UserName
            });

            return Ok(userMatches);
        }

        [HttpPost("{id}/generate-playoff")]
        [Authorize]
        public async Task<IActionResult> GeneratePlayoff(int id)
        {
            var alreadyExists = await _context.Matches
                .Where(m => m.Tournament_ID == id)
                .AnyAsync(m => _context.Play_Offs.Any(p => p.Match_ID == m.Id));

            if (alreadyExists)
            {
                return BadRequest(new { message = "Сетка уже была создана для этого турнира." });
            }

            await GeneratePlayoffBracketAsync(id);
            return Ok(new { message = "Playoff сетка создана" });
        }

        private async Task GeneratePlayoffBracketAsync(int tournamentId)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.ID == tournamentId);

            if (tournament == null) return;

            var format = await _context.Tournament_Formats
                .FirstOrDefaultAsync(f => f.ID == tournament.Format_ID);

            if (format == null || !format.Name.ToLower().Contains("playoff")) return;

            var participants = tournament.Participants.ToList();

            var ids = tournament.solo == true
                ? participants.Select(p => p.User_ID).Cast<string?>().ToList()
                : participants.Select(p => p.Team_ID?.ToString()).ToList();

            if (ids.Count < 2) return;

            int totalSlots = 1;
            while (totalSlots < ids.Count)
                totalSlots *= 2;

            while (ids.Count < totalSlots)
                ids.Add(null);

            var rnd = new Random();
            ids = ids.OrderBy(_ => rnd.Next()).ToList();

            var stage = new Tournament_Stages
            {
                Tournament_ID = tournamentId,
                Name = "1/" + totalSlots,
                Stage_order = 1
            };
            _context.Tournament_Stages.Add(stage);
            await _context.SaveChangesAsync();

            var matches = new List<Match>();
            for (int i = 0; i < ids.Count; i += 2)
            {
                var match = new Match
                {
                    Tournament_ID = tournamentId,
                    Sport_ID = tournament.Sport_ID,
                    Stage_ID = stage.ID,
                    Data = DateTime.UtcNow.AddDays(1),
                    Time = TimeSpan.FromHours(12)
                };

                if (tournament.solo == true)
                {
                    match.Player1_ID = ids[i];
                    match.Player2_ID = ids[i + 1];
                }
                else
                {
                    match.Team1_ID = string.IsNullOrEmpty(ids[i]) ? null : int.Parse(ids[i]);
                    match.Team2_ID = string.IsNullOrEmpty(ids[i + 1]) ? null : int.Parse(ids[i + 1]);
                }

                matches.Add(match);
            }

            _context.Matches.AddRange(matches);
            await _context.SaveChangesAsync();

            // Сетка PlayOff с привязками к следующему матчу
            var playOffs = new List<Play_Off>();
            for (int i = 0; i < matches.Count; i++)
            {
                var playoff = new Play_Off
                {
                    Match_ID = matches[i].Id
                };

                int nextMatchIndex = matches.Count + (i / 2);
                int totalRounds = (int)Math.Log2(totalSlots);
                int nextRoundMatches = totalSlots / 2;

                // Привязываем победителя к следующему матчу
                if ((i / 2) < matches.Count / 2)
                {
                    // next match должен быть добавлен позже — создаём плейсхолдеры
                    var nextMatch = new Match
                    {
                        Tournament_ID = tournamentId,
                        Sport_ID = tournament.Sport_ID,
                        Stage_ID = stage.ID, // временно, потом можно создать новую стадию
                        Data = DateTime.UtcNow.AddDays(2),
                        Time = TimeSpan.FromHours(12)
                    };
                    _context.Matches.Add(nextMatch);
                    await _context.SaveChangesAsync();

                    playoff.Next_Match_Winner_ID = nextMatch.Id;
                }

                playOffs.Add(playoff);
            }

            _context.Play_Offs.AddRange(playOffs);
            await _context.SaveChangesAsync();
        }

        [HttpPost("{matchId}/add-statistics")]
        [Authorize]
        public async Task<IActionResult> AddStatistics(int matchId, [FromBody] MatchStatisticDto dto)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null)
                return NotFound("Матч не найден");

            if (dto.IsTeam)
            {
                if (dto.TeamId == null)
                    return BadRequest("TeamId обязателен для командной статистики");

                foreach (var stat in dto.Statistics)
                {
                    var teamStat = new Team_Statistic
                    {
                        Match_ID = matchId,
                        Team_ID = dto.TeamId.Value,
                        Stat_ID = stat.StatId,
                        Stats = stat.Value
                    };
                    _context.Team_Statistics.Add(teamStat);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(dto.PlayerId))
                    return BadRequest("PlayerId обязателен для индивидуальной статистики");

                foreach (var stat in dto.Statistics)
                {
                    var playerStat = new Player_Statistic
                    {
                        Match_ID = matchId,
                        Player_ID = dto.PlayerId,
                        Stat_ID = stat.StatId,
                        Stats = stat.Value
                    };
                    _context.Player_Statistics.Add(playerStat);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Статистика успешно добавлена" });
        }

        [HttpGet("{id}/bracket")]
        public async Task<IActionResult> GetBracket(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Play_Offs)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Team1)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Team2)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Player1)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Player2)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (tournament == null)
                return NotFound("Турнир не найден");

            var bracket = tournament.Matches
                .Where(m => _context.Play_Offs.Any(p => p.Match_ID == m.Id))
                .Select(m => new
                {
                    matchId = m.Id,
                    team1 = m.Team1?.Name,
                    team2 = m.Team2?.Name,
                    player1 = m.Player1?.UserName,
                    player2 = m.Player2?.UserName,
                    date = m.Data,
                    time = m.Time
                })
                .OrderBy(m => m.date)
                .ToList();

            return Ok(bracket);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyTournaments()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized();

            var tournaments = await _context.Tournaments
                .Where(t => t.Creator_ID == user.Id)
                .Select(t => new
                {
                    t.ID,
                    t.Name,
                    t.Start,
                    t.End,
                    t.solo,
                    SportName = t.Sport.Name
                })
                .ToListAsync();

            return Ok(tournaments);
        }

        [Authorize]
        [HttpPost("{id}/ban")]
        public async Task<IActionResult> BanTournament(int id, [FromBody] BanTournamentDto banDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user.admin != true) { return Unauthorized("Invalid right");}
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound("Турнир не найден");
            }


            // Блокировка турнира и сохранение причины
            tournament.ban = true;
            tournament.reasonBan = banDto.ReasonBan;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Турнир успешно заблокирован", reason = banDto.ReasonBan });
        }

        [HttpGet("banned")]
        [Authorize]
        public async Task<IActionResult> GetBannedTournaments()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.admin != true)
                return Forbid("Only admins can access banned tournaments");

            var bannedTournaments = await _context.Tournaments
                .Where(t => t.ban == true)
                .Select(t => new
                {
                    t.ID,
                    t.Name,
                    t.Info,
                    t.Status,
                    t.Adress,
                    t.Age,
                    t.Pol,
                    t.Start,
                    t.End,
                    CreatorName = t.Creator != null ? t.Creator.UserName : "Unknown",
                    Reason = t.reasonBan
                })
                .ToListAsync();

            return Ok(bannedTournaments);
        }

        [HttpPost("{tournamentId}/unban")]
        [Authorize]
        public async Task<IActionResult> UnbanTournament(int tournamentId)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.admin != true)
                return Forbid("Only admins can unban tournaments");

            var tournament = await _context.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
                return NotFound("Tournament not found");

            if (tournament.ban != true)
                return BadRequest("Tournament is not banned");

            tournament.ban = false;
            tournament.reasonBan = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Tournament unbanned successfully" });
        }

        [HttpGet("sports")]
        public async Task<IActionResult> GetSports()
        {
            var sports = await _context.Sports
                .Select(s => new { s.ID, s.Name })
                .ToListAsync();

            return Ok(sports);
        }

        [HttpGet("formats")]
        public async Task<IActionResult> GetFormats()
        {
            var formats = await _context.Tournament_Formats
                .Select(f => new { f.ID, f.Name })
                .ToListAsync();

            return Ok(formats);
        }
    }
}


