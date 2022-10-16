using DattingApp.Data;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DattingApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<AppUser>> GetAllMembersAsync()
        {
            return await _dataContext.Users
                .Include(p => p.Photos) 
                .ToListAsync();
        }

        public async Task<AppUser> GetMemberById(int id)
        {
           return await _dataContext.Users
                .Include(_p => _p.Photos)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetMemberByNameAsync(string name)
        {
            return await _dataContext.Users
                .Include(p => p.Photos)
                .Where(n=> n.UserName == name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateMember(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}
