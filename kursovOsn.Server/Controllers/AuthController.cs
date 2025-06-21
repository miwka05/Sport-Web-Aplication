using kursovOsn.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using kursovOsn.Server.Data;
//using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace kursovOsn.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || model.Date == null|| string.IsNullOrEmpty(model.Sex) )
            {
                return BadRequest(new { Message = "Username, Password, FirstName, and LastName are required." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                DateOfBirth = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc), 
                Sex = model.Sex 
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }

            return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            return Unauthorized();
        }


        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(userId);

            if (user == null)
                return NotFound("User not found");

            user.FirstName = model.FirstName;
            user.LastName = model.SecondName;
            user.Email = model.Email;
            user.DateOfBirth = DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc);
            user.Sex = model.Pol;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Profile updated");
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(userId);

            if (user == null)
                return NotFound("User not found");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password changed successfully");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.DateOfBirth,
                user.Sex,
                user.ban,
                user.reasonBan
            });
        }

        [Authorize]
        [HttpPost("{id}/ban")]
        public async Task<IActionResult> BanUser(string id, [FromBody] BanTournamentDto banDto)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user.admin != true) { return Unauthorized("Invalid right"); }
            var Auser = await _context.Users.FindAsync(id);
            if (Auser == null)
            {
                return NotFound("Пользователь не найден");
            }


            // Блокировка турнира и сохранение причины
            Auser.ban = true;
            Auser.reasonBan = banDto.ReasonBan;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь успешно заблокирован", reason = banDto.ReasonBan });
        }

        [HttpGet("banned")]
        [Authorize]
        public async Task<IActionResult> GetBannedUser()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.admin != true)
                return Forbid("Only admins can access banned user");

            var bannedUser = await _context.Users
                .Where(t => t.ban == true)
                .Select(t => new
                {
                    t.Id,
                    t.UserName,
                    t.Email,
                    t.FullName,
                    t.Sex,
                    Reason = t.reasonBan
                })
                .ToListAsync();

            return Ok(bannedUser);
        }

        [HttpPost("{userId}/unban")]
        [Authorize]
        public async Task<IActionResult> UnbanTournament(string userId)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.admin != true)
                return Forbid("Only admins can unban user");

            var Auser = await _context.Users.FindAsync(userId);
            if (Auser == null)
                return NotFound("User not found");

            if (Auser.ban != true)
                return BadRequest("User is not banned");

            Auser.ban = false;
            Auser.reasonBan = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "User unbanned successfully" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("Invalid token");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                DateOfBirth = user.DateOfBirth.ToString("yyyy-MM-dd"),
                Sex = user.Sex
            });
        }


        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
