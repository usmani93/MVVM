﻿using ChatAPI.Data;
using ChatAPI.Helper;
using ChatAPI.Hubs;
using ChatAPI.Interfaces;
using ChatAPI.Model;
using ChatAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Controllers
{
    [Route("api/chatserver")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly ChatContext _chatContext;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IAuthenticateUser _authenticateUser;

        public ChatController(ChatContext context, IHubContext<ChatHub> hubContext, IAuthenticateUser authenticateUser)
        {
            _chatContext = context;
            _hubContext = hubContext;
            _authenticateUser = authenticateUser;
        }

        [Route("send")]
        [HttpPost]
        public IActionResult SendRequest([FromBody] Request request)
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", request.UserName, request.Message);
            return Ok();
        }

        [Route("getUsers")]
        [HttpGet]
        public List<User> GetUsers()
        {
            return ChatHub.GetUsersList();
        }

        [Route("sendMessage")]
        [HttpPost]
        public IActionResult SendMessageToUser([FromBody] Request request)
        {
            _hubContext.Clients.Client(request.UserId.Trim()).SendAsync("PrivateMessage", request.UserName);
            return Ok();
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] Request request)
        {
            var token = _authenticateUser.AuthenticateUserWithJWT(request.UserName);
            request.Token = token;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            else
            {
                return Ok(request);
            }
        }

        [Authorize]
        [Route("validate")]
        [HttpPost]
        public IActionResult ValidateToken(string token)
        {
            return Ok(_authenticateUser.TokenValidator(token));
        }

        [Route("getUsersFromDB")]
        [HttpPost]
        public async IAsyncEnumerable<User> GetUsersFromDBAsync()
        {
            var id = await _chatContext.Users.ToListAsync();
            foreach (var item in id)
            {
                yield return item;
            }
        }

        [Route("addUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            _chatContext.Users.Add(user);
            await _chatContext.SaveChangesAsync();
            return Ok();
        }

        [Route("updateUser")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var userToUpdate = await _chatContext.Users.FirstOrDefaultAsync(x => x.userID == user.userID);
            if (userToUpdate == null)
            {
                return StatusCode(404);
            }
            try
            {
                userToUpdate.connectionId = user.connectionId;
                userToUpdate.userName = user.userName;
                userToUpdate.userToken = user.userToken;
                //user.ApplyTo(userToUpdate);
                _chatContext.Update(userToUpdate);
                await _chatContext.SaveChangesAsync();
                return Ok(userToUpdate);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }

}