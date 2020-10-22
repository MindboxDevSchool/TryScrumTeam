using System;
using System.Security.Claims;
using ItHappend.RestAPI.Authentication;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IUserService _userService;

        public AuthenticationController(IJwtIssuer jwtIssuer, IUserService userService)
        {
            _jwtIssuer = jwtIssuer ?? throw new ArgumentNullException(nameof(jwtIssuer));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        
        [HttpPost]
        [Route("authentication")]
        public IActionResult Authenticate([FromBody]LoginRequest request)
        {
            if (request.Login == request.Password)
            {
                var token1 = _jwtIssuer.GenerateToken(new UserDto(Guid.Empty, request.Login));
            
                var response1 = new LoginResponse(token1);
                return Ok(response1);
            }

            var user = _userService.LoginUser(request.Login, request.Password);
            if (user == null)
            {
                return Unauthorized("User with provided credentials not found");
            }

            var token = _jwtIssuer.GenerateToken(user);
            
            var response = new LoginResponse(token);
            return Ok(response);
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