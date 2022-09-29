using DattingApp.Entities;

namespace DattingApp.Interfaces
{
    public interface ITokenInterface
    {
         string CreateToken(AppUser user);
    }
}
