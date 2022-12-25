using DemoServerless.Entities;
using DemoServerless.Models.User;
using DemoServerless.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DemoServerless.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest registerRequest)
        {
            var response = _authService.Register(registerRequest);
            return Ok(response);
        }

        [HttpPost("Login")]
        public ActionResult<LoginResponse> Login(LoginRequest loginRequest)
        {
            var response = _authService.Login(loginRequest);
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("ChangePassword")]
        public ActionResult<bool> ChangePassword(ChangePassword changePassword)
        {
            var user = HttpContext.Items["User"] as User;
            var response = _authService.ChangePassword(user.Id, changePassword);
            return response;
        }
    }
}
