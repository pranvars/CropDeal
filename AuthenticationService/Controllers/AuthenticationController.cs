using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Cors;
using AuthenticationService.Process;
using AuthenticationService.EntityModel;
using AuthenticationService;
using Microsoft.Identity.Client;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("allowAllPolicy")]
    public class AccountsController : ControllerBase
    {

        private readonly AuthProcess _repository;

        public AccountsController(AuthProcess repository)
        {
            _repository = repository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                var error1 = new FaultContract()
                {
                    FaultId = 1,
                    FaultDescription = "Either name or password is empty",
                    FaultName = "Missing Values",
                    FaultType = "Authentication"
                };
                return BadRequest(error1);
            }
            var user = await _repository.ValidateUserAndGenerateResponse(request);
            if (user != null)
            {
                return Ok(user);
            }

            var error2 = new FaultContract()
            {
                FaultId = 2,
                FaultDescription = "Incorrect credentials",
                FaultName = "Unauthorized",
                FaultType = "Authentication"
            };
            return Unauthorized(error2);
        }
        [HttpGet("validate")]
        public async Task<ActionResult<AuthResponse>> Validate()
        {
            try
            {
                var user = await _repository.ValidateTokenAndReturnUser();
                if (user != null)
                {
                    return Ok(user);
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
            }
            return Unauthorized();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            try
            {
                var response = await _repository.Signup(request);
                return CreatedAtAction(nameof(Signup), new { userId = response.User.UserId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetUserName/{userId}")]
        public async Task<IActionResult> GetUsernameByUserIdAsync(int userId)
        {
            var username = await _repository.GetUsernameByUserIdAsync(userId);
            return Ok(new {Username = username});
        }
    }
}
