using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models.Enums;

namespace Nexus.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(NexusAuthorizationFilter))]
    public class BaseController : ControllerBase
    {
        public EmployeeType EmployeeType { get; set; }
    }
}
