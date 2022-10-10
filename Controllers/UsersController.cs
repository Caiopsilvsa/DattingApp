using AutoMapper;
using DattingApp.Data;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var usersMapped = _mapper.Map<IEnumerable<MemberDto>>(await _userRepository.GetAllMembersAsync());

            return Ok(usersMapped); 
        }

        [HttpGet("{userid}")]
        //[Authorize]
        public async Task<ActionResult<MemberDto>> GetUserById(int userid)
        {
            var userMapped = _mapper.Map<MemberDto>(await _userRepository.GetMemberById(userid));

            return Ok(userMapped);
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByName(string username)
        {
            var userMapped = _mapper.Map<MemberDto>(await _userRepository.GetMemberByNameAsync(username));

            return Ok(userMapped);
        }
    }
}
