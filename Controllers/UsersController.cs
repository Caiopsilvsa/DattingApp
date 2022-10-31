using AutoMapper;
using DattingApp.Data;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Extensions;
using DattingApp.Helpers;
using DattingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DattingApp.Controllers
{
    public class UsersController:BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController( IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var userName = User.GetUsername();
            var user = await _userRepository.GetMemberByNameAsync(userName);
            userParams.CurrentUserName = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        [HttpGet("{userid}")]
        [Authorize]
        public async Task<ActionResult<MemberDto>> GetUserById(int userid)
        {
            var userMapped = _mapper.Map<MemberDto>(await _userRepository.GetMemberById(userid));

            return Ok(userMapped);
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<AppUser>> GetUserByName(string username)
        {
            var user = await _userRepository.GetMemberByNameAsync(username);

            return Ok(user);
        }


        [HttpPut("user")]
        public async Task<ActionResult<bool>> UpdateMember(MemberUpdateDto member)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetMemberByNameAsync(username);

            _mapper.Map(member, user);

            _userRepository.UpdateMember(user);

            if (await _userRepository.SaveChanges()) return NoContent();

            return BadRequest("Failed to update user");
        }
    }
}
