using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DattingApp.Data;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Extensions;
using DattingApp.Helpers;
using DattingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DattingApp.Repositories
{
    public class LikeRepository : ILikeInterface
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            this._context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceId, likedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users =  _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes =  _context.Likes.AsQueryable();

           if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(u => u.SourceUserId == likesParams.UserId);
                users = likes.Select(u => u.LikedUser);
            }

           if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(l => l.LikedUserId == likesParams.UserId);
                users = likes.Select(l => l.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });
            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize); 

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
