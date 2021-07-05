using ChatAPI.Hubs;
using ChatAPI.Interfaces;
using ChatAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IAuthenticateUser _authenticateUser;

        public ChatController(IHubContext<ChatHub> hubContext, IAuthenticateUser authenticateUser)
        {
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
        public List<string> GetUsers()
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

        [Route("authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] Request request)
        {
            var token = _authenticateUser.AuthenticateUserWithJWT(request.UserName);
            if(string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }

    }
}
