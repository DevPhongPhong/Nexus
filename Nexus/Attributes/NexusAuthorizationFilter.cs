using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Nexus.Models;
using Nexus.Models.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace EIM.Attributes.FilterPipelines.Authorizations;

public class NexusAuthorizationFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public NexusAuthorizationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Kiểm tra xem có yêu cầu quyền anonymous không
        if (NexusAuthorizationHelpers.IsAllowAnonymousRequest(context)) return;

        var issuer = _configuration["JWT:Issuer"];
        var secretkey = _configuration["JWT:SecretKey"];

        try
        {
            // Lấy token từ header
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = NexusAuthorizationHelpers.ValidateToken(token, secretkey);

            if (principal == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Lấy EmployeeType từ claim
            var user = JsonConvert.DeserializeObject<User>(principal.Claims.FirstOrDefault(c => c.Type == "User").Value);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            Role? employeeType = user.RoleId;

            // Kiểm tra quyền truy cập theo EmployeeType
            var requiredEmployeeType = context.ActionDescriptor.EndpointMetadata
                .OfType<RoleAccessAttribute>()
                .FirstOrDefault()?.AllowedEmployeeTypes;
            if (requiredEmployeeType != null && !requiredEmployeeType.Any(type => type == employeeType))
            {
                context.Result = new ForbidResult();
                return;
            }

            context.HttpContext.User = principal;
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}