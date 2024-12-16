using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Services;
using System.Security.Cryptography;
using System.Text;

namespace Nexus.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;


        public AccountController(IEmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest loginDTO)
        {
            loginDTO.Password = ToSHA256HashString(loginDTO.Password);
            var loginResponse = _employeeService.ValidateEmployee(loginDTO.Username, loginDTO.Password);
            if (loginResponse == null)
            {
                return new UnauthorizedResult();
            }
            string secretKey = _configuration["JWT:SecretKey"];
            DateTime expTime = DateTime.Now.AddHours(int.Parse(_configuration["JWT:LifeTimeHour"].ToString()));
            return new OkObjectResult(NexusAuthorizationHelpers.GenerateToken(loginResponse, secretKey, "Nexus", "Nexus_WebUI", expTime));
        }

        public static string ToSHA256HashString(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}

// Dùng để lấy thông tin đăng nhập
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
