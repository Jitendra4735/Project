using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Business.Interface;
using WebApi.Utilities.Models;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserAccountController : ControllerBase
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IUserAccountManager _userAccountManager;
        private readonly IAuthenticationProcessor _authenticationProcessor;

        public UserAccountController(ILogger<UserAccountController> logger, IUserAccountManager userAccountManager, IAuthenticationProcessor authenticationProcessor)
        {
            _logger = logger;
            _userAccountManager = userAccountManager;
            _authenticationProcessor = authenticationProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserAccount userAccount)
        {
            _logger.LogInformation($"RegisterUser method to Register user in Database **STARTS** with UserAccount = {userAccount.ToString()}");
            if (ModelState.IsValid)
            {
                // Check if the username or email is already taken
                if (_userAccountManager.IsUserExists(userAccount).Result)
                {
                    return BadRequest(new { message = "Username or email already exists" });
                }

                await _userAccountManager.RegisterUser(userAccount);
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                if (_userAccountManager.VerifyUserCredentials(userAccount).Result)
                {
                    var token = await _authenticationProcessor.GenerateJwtToken(userAccount);
                    return Ok(new { Token = token });

                }
                return Unauthorized(new { message = "Invalid username or password" });
            }
            return BadRequest(ModelState);
        }
    }
}
