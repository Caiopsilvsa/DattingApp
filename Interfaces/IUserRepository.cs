using DattingApp.Dto;
using DattingApp.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DattingApp.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetMemberByNameAsync(string name);
        Task<AppUser> GetMemberById(int id);
        Task<IEnumerable<AppUser>> GetAllMembersAsync();


    }
}
