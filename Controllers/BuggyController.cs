using DattingApp.Data;
using DattingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DattingApp.Controllers
{
    public class BuggyController: BaseController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext dataContext)
        {
            this._context = dataContext;
        }

        [HttpGet("auth")]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var notFound = _context.Users.Where(c => c.Id == -1).FirstOrDefault();

            if(notFound == null){
                return NotFound();
            }

            return Ok(notFound);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}
