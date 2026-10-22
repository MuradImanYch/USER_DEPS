using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
using USER_DEPS.Data;
using USER_DEPS.Dto;
using USER_DEPS.DTOs;
using USER_DEPS.Entity;
using USER_DEPS.Services;

namespace USER_DEPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtService _jwtService;

        public AuthController(
            AppDbContext context,
            PasswordService passwordService,
            JwtService jwtService,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var authHeader = Request.Headers.Authorization.ToString();

            if (!authHeader.StartsWith(
                "Bearer ",
                StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized(new
                {
                    message = "Token göndərilməyib."
                });
            }

            var token = authHeader["Bearer ".Length..].Trim();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Token == token);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Token bazada tapılmadı."
                });
            }

            user.Token = null;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Logout uğurla tamamlandı."
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(
       [FromBody] RegisterRequest request)
        {
            var username = request.Username.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    message = "Username boş ola bilməz."
                });
            }

            var exists = await _context.Users
                .AnyAsync(u => u.Username == username);

            if (exists)
            {
                return Conflict(new
                {
                    message = "Bu username artıq mövcuddur."
                });
            }

            var user = new User
            {
                Username = username,
                PersonalId = null
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    request.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                message = "Qeydiyyat uğurla tamamlandı."
            });
        }


 
     [HttpPost("login")]
    public async Task<IActionResult> Login(
       [FromBody] LoginDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Username == request.Username);

            if (user == null ||
                string.IsNullOrWhiteSpace(user.PasswordHash) ||
                !_passwordService.VerifyPassword(
                    request.Password,
                    user.PasswordHash))
            {
                return Unauthorized(new
                {
                    message = "İstifadəçi adı və ya parol yanlışdır."
                });
            }

            var token = _jwtService.GenerateToken(user.Id, user.Username);

            user.Token = token;

            await _context.SaveChangesAsync();
 
            return Ok(new
            {
                message = "Login uğurludur.",
                token,
                username = user.Username
            });
        }
 
     }
}

