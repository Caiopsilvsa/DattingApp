using DattingApp.Data;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DattingApp.Controllers
{
    public class AccountController:BaseController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenInterface _tokenInterface;
        public AccountController(DataContext dataContext, ITokenInterface TokenInterface)
        {
            this._dataContext = dataContext;
            this._tokenInterface = TokenInterface;
        }

        [HttpPost("registrer")]
        public async Task<ActionResult<UseDto>> Register([FromBody] UserDto user)
        {
            if (await UserExistsByName(user))
            {
                return BadRequest("Usuario ja existe");
            }
            using var hmac = new HMACSHA512();
            
            var newUser = new AppUser
            {
                UserName = user.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key
            };

            _dataContext.AddAsync(newUser);
            _dataContext.SaveChangesAsync();

            return new UseDto
            {
                UserName = newUser.UserName,
                Token = _tokenInterface.CreateToken(newUser)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UseDto>> Login([FromBody] LoginDto userLogin)
        {
            var testUser = await _dataContext.Users
               .Where(u => u.UserName == userLogin.UserName).FirstOrDefaultAsync();

            if (testUser == null)
            {
                return Unauthorized("Usuário invalido");
            }

            using var hmac = new HMACSHA512(testUser.PasswordSalt);
            var hashedPass = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLogin.Password));

            for (int i = 0; i < hashedPass.Length; i++)
            {
                if (hashedPass[i] != testUser.PasswordHash[i])
                {
                    return Unauthorized("Senha Invalida");
                }
            }

            return new UseDto
            {
                UserName = testUser.UserName,
                Token = _tokenInterface.CreateToken(testUser)
            };
        }

        private async Task<bool> UserExistsByName(UserDto user)
        {
           var testuser = await _dataContext.Users.AnyAsync(x => x.UserName == user.UserName);

            return testuser;
        }
    }
}
