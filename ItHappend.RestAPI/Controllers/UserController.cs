using System;
using System.Security.Claims;
using ItHappend.RestAPI.Authentication;
using ItHappend.RestAPI.Filters;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IUserService _userService;

        public UserController(IJwtIssuer jwtIssuer, IUserService userService)
        {
            _jwtIssuer = jwtIssuer ?? throw new ArgumentNullException(nameof(jwtIssuer));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        
        [HttpPost]
        [Route("authentication")]
        public IActionResult Authenticate([FromBody]LoginRequest request)
        {
            var user = _userService.LoginUser(request.Login, request.Password);
            
            var token = _jwtIssuer.GenerateToken(user);
            
            var response = new LoginResponse(token);
            return Ok(response);
        }

        [HttpPost]
        [Route("user")]
        public IActionResult RegisterUser([FromBody]LoginRequest request)
        {
            var newUser = _userService.CreateUser(request.Login, request.Password);
            var result = new
            {
                Id = newUser.Id,
                Login = newUser.Login
            };
            return Ok(result);
        }
        
        [HttpGet]
        [Route("self")]
        [Authorize]
        public IActionResult GetSelf()
        {
            var result = new
            {
                Id = User.FindFirstValue(JwtClaimTypes.Id),
                Login = User.FindFirstValue(JwtClaimTypes.Login)
            };
            return Ok(result);
        }
    }
}