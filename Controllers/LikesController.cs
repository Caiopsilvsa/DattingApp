using DattingApp.Dto;
using DattingApp.Extensions;
using DattingApp.Helpers;
using DattingApp.Interfaces;
using DattingApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DattingApp.Controllers
{
    [Authorize]
    public class LikesController: BaseController
    {
        private readonly ILikeInterface _likesRepository;
        private readonly IUserRepository _userRepository;

        public LikesController(ILikeInterface likes, IUserRepository user)
        {
            _likesRepository = likes;
            _userRepository = user;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string userName)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetMemberByNameAsync(userName);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == userName) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("Você já curtiu esse usuário");

            userLike = new Entities.UserLike
            {
                SourceUserId = sourceUser.Id,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveChanges()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet("likes")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.Count, users.TotalPages);

            return Ok(users);
        }
    }
}
