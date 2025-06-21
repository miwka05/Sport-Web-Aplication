using kursovOsn.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kursovOsn.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace kursovOsn.Server.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto dto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);

            var team = new Team
            {
                Name = dto.Name,
                Sport_ID = dto.Sport_ID,
                City = dto.City,
                Age = dto.Age,
                Creator_ID = user.Id
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Sport)
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null) return NotFound();

            return Ok(new
            {
                team.Id,
                team.Name,
                team.City,
                team.Age,
                team.ban,
                team.reasonBan,
                Sport = team.Sport?.Name,
                Creator = team.Creator?.UserName,
                CreatorId = team.Creator?.Id
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.Teams
                .Include(t => t.Sport)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.City,
                    t.Age,
                    t.ban,
                    t.reasonBan,
                    SportName = t.Sport != null ? t.Sport.Name : null
                })
                .ToListAsync();

            return Ok(teams);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyTeams()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized();

            var myTeams = await _context.Teams
                .Where(t => t.Creator_ID == user.Id)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Sport_ID
                })
                .ToListAsync();

            return Ok(myTeams);
        }

        [HttpPut("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> EditTeam(int id, [FromBody] CreateTeamDto dto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized();

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound("Команда не найдена");

            if (team.Creator_ID != user.Id && user.admin != true)
                return Forbid("Вы не являетесь создателем команды или администратором");

            team.Name = dto.Name;
            team.City = dto.City;
            team.Age = dto.Age;
            team.Sport_ID = dto.Sport_ID;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Команда успешно обновлена" });
        }

        [HttpGet("sports")]
        public async Task<IActionResult> GetSports()
        {
            var sports = await _context.Sports
                .Select(s => new { s.ID, s.Name })
                .ToListAsync();

            return Ok(sports);
        }

        [HttpPost("{id}/join")]
        [Authorize]
        public async Task<IActionResult> JoinTeam(int id)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return Unauthorized();

            var existing = await _context.Team_Entries
                .FirstOrDefaultAsync(te => te.Team_ID == id && te.User_ID == user.Id);
            if (existing != null) return BadRequest("Заявка уже подана");

            var entry = new Team_entry
            {
                Team_ID = id,
                User_ID = user.Id,
                EntryDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            };

            _context.Team_Entries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("{id}/has-request")]
        [Authorize]
        public async Task<IActionResult> HasRequest(int id)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);

            var exists = await _context.Team_Entries.AnyAsync(e => e.Team_ID == id && e.User_ID == user.Id);
            return Ok(exists);
        }

        [HttpGet("{id}/players")]
        public async Task<IActionResult> GetPlayers(int id)
        {
            var players = await _context.Team_Players
                .Where(tp => tp.ID_Team == id)
                .Include(tp => tp.User)
                .Select(tp => new
                {
                    id = tp.User.Id,             // ID пользователя
                    userName = tp.User.UserName  // Имя пользователя
                })
                .ToListAsync();

            return Ok(new { values = players });
        }


        [HttpGet("{id}/entries")]
        [Authorize]
        public async Task<IActionResult> GetTeamEntries(int id)
        {
            var entries = await _context.Team_Entries
                .Where(e => e.Team_ID == id)
                .Include(e => e.User)
                .Select(e => new { id = e.ID,username = e.User.UserName, userId = e.User.Id })
                .ToListAsync();

            return Ok(new { values = entries });
        }

        [HttpPost("entry/{entryId}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptEntry(int entryId)
        {
            var entry = await _context.Team_Entries.Include(e => e.User).FirstOrDefaultAsync(e => e.ID == entryId);
            if (entry == null) return NotFound();

            var participant = new Team_player
            {
                ID_Team = entry.Team_ID,
                ID_User = entry.User_ID,
                Role = "Игрок"
            };

            _context.Team_Players.Add(participant);
            _context.Team_Entries.Remove(entry);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("entry/{entryId}/reject")]
        [Authorize]
        public async Task<IActionResult> RejectEntry(int entryId)
        {
            var entry = await _context.Team_Entries.FindAsync(entryId);
            if (entry == null) return NotFound();

            _context.Team_Entries.Remove(entry);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/ban")]
        public async Task<IActionResult> BanTeam(int id, [FromBody] BanTournamentDto banDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user.admin != true) { return Unauthorized("Invalid right"); }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound("Команда не найдена");
            }


            // Блокировка турнира и сохранение причины
            team.ban = true;
            team.reasonBan = banDto.ReasonBan;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Команда успешно заблокирована", reason = banDto.ReasonBan });
        }

        [HttpGet("banned")]
        [Authorize]
        public async Task<IActionResult> GetBannedTeamss()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.admin != true)
                return Forbid("Only admins can access banned tournaments");

            var bannedTeams = await _context.Teams
                .Where(t => t.ban == true)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.City,
                    t.Age,
                    CreatorName = t.Creator != null ? t.Creator.UserName : "Unknown",
                    Reason = t.reasonBan
                })
                .ToListAsync();

            return Ok(bannedTeams);
        }

        [HttpPost("{teamId}/unban")]
        [Authorize]
        public async Task<IActionResult> UnbanTournament(int teamId)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.admin != true)
                return Forbid("Only admins can unban team");

            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
                return NotFound("Team not found");

            if (team.ban != true)
                return BadRequest("Team is not banned");

            team.ban = false;
            team.reasonBan = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Team unbanned successfully" });
        }
    }
}

