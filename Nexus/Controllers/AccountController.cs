using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace Nexus.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly NexusDbContext _context;

        public AccountController(IConfiguration configuration, NexusDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest loginDTO)
        {
            loginDTO.Password = Common.ToSHA256HashString(loginDTO.Password);

            // Retrieve user from the database
            var loginResponse = _context.Employees.AsNoTracking()
                .FirstOrDefault(u => u.Username == loginDTO.Username && u.PasswordHash == loginDTO.Password);

            if (loginResponse == null)
            {
                return Unauthorized();
            }

            
            string secretKey = _configuration["JWT:SecretKey"];
            DateTime expTime = DateTime.Now.AddHours(int.Parse(_configuration["JWT:LifeTimeHour"].ToString()));
            return new OkObjectResult(NexusAuthorizationHelpers.GenerateToken(loginResponse, secretKey, expTime));
        }
    }
}

// Dùng để lấy thông tin đăng nhập
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
