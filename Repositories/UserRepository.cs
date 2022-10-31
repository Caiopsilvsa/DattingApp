using AutoMapper;
using AutoMapper.QueryableExtensions;
using DattingApp.Data;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Helpers;
using DattingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DattingApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;

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
                .FindAsync(id);
        }

        public async Task<AppUser> GetMemberByNameAsync(string name)
        {
            return await _dataContext.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _dataContext.Users.AsQueryable();
                query = query.Where(u => u.UserName != userParams.CurrentUserName);
                query = query.Where(u => u.Gender == userParams.Gender);

            var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);
                query = query.Where(d => d.DateOfBirth >= minDateOfBirth && d.DateOfBirth <= maxDateOfBirth);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };
           
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);
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
