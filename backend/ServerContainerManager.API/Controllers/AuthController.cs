using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Models.Requests;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(ILogger<AuthController> logger, SignInManager<AppUser> signInManager) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        [HttpPost("signin")]
        public async Task<Results<Ok, ForbidHttpResult, UnauthorizedHttpResult>> SignIn(SignInRequest request)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                request.Username,
                request.Password,
                isPersistent: true,
                lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
                return TypedResults.Forbid();

            if (signInResult.IsNotAllowed)
                return TypedResults.Forbid();

            if (!signInResult.Succeeded)
                return TypedResults.Unauthorized();

            return TypedResults.Ok();
        }

        [HttpPost("signout")]
        public async Task<Ok> SignOut()
        {
            await _signInManager.SignOutAsync();

            return TypedResults.Ok();
        }
    }
}
