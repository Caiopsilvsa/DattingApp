using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DattingApp.Interfaces
{
    public interface ILikeInterface
    {
        Task<UserLike> GetUserLike(int sourceId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
