using DattingApp.Data;
using DattingApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DattingApp.Controllers
{
    [ApiController]
    [Route("Api/controller")]
    public class UsersController:Controller
    {
        private readonly DataContext _dataContext;
        public UsersController( DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _dataContext.Users.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{Userid}")]
        public async Task<ActionResult<AppUser>> GetUser(int Userid)
        {
            var user = await _dataContext.Users.Where(u => u.Id == Userid).FirstOrDefaultAsync();

            return Ok(user);
        } 
        
    }
}
