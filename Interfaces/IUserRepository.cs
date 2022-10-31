using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DattingApp.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetMemberByNameAsync(string name);
        Task<AppUser> GetMemberById(int id);
        //Task<IEnumerable<AppUser>> GetAllMembersAsync();
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        void UpdateMember(AppUser user);
        Task<bool> SaveChanges();

    }
}
