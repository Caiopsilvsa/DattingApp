using DattingApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DattingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("Api/controller")]
    public class BaseController:ControllerBase
    {
    }
}
