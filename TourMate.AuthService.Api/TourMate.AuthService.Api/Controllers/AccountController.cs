using Microsoft.AspNetCore.Mvc;
using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Repositories.Models;
using TourMate.AuthService.Repositories.RequestModels;
using TourMate.AuthService.Services.Utilities;

namespace TourMate.AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!ValidInput.IsEmailValid(request.Email))
                    return BadRequest("Invalid email format");

                var result = await _accountService.LoginAsync(request.Email, request.Password);
                
                if (result == null)
                    return Unauthorized("Invalid email or password");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!ValidInput.IsEmailValid(request.Email))
                    return BadRequest("Invalid email format");

                var result = await _accountService.GoogleLoginAsync(request.Email);
                
                if (result == null)
                    return Unauthorized("Google login failed");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google login for email: {Email}", request.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.RefreshNewTokenAsync(request.RefreshToken);
                
                if (result == null)
                    return Unauthorized("Invalid refresh token");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.ChangePasswordAsync(
                    request.AccountId, 
                    request.CurrentPassword, 
                    request.NewPassword);

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password change for account: {AccountId}", request.AccountId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!ValidInput.IsEmailValid(request.Email))
                    return BadRequest("Invalid email format");

                var result = await _accountService.RequestPasswordResetAsync(request.Email);
                
                if (!result)
                    return NotFound("Email not found");

                return Ok(new { message = "Password reset email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset request for email: {Email}", request.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!ValidInput.IsPasswordSecure(request.NewPassword))
                    return BadRequest("Password does not meet security requirements");

                var result = await _accountService.ResetPasswordAsync(request.Token, request.NewPassword);
                
                if (!result)
                    return BadRequest("Invalid or expired token");

                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            try
            {
                var account = await _accountService.GetAccount(id);
                
                if (account == null)
                    return NotFound("Account not found");

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting account: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetAccountByEmail(string email)
        {
            try
            {
                if (!ValidInput.IsEmailValid(email))
                    return BadRequest("Invalid email format");

                var account = await _accountService.GetAccountByEmail(email);
                
                if (account == null)
                    return NotFound("Account not found");

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting account by email: {Email}", email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!ValidInput.IsEmailValid(request.Email))
                    return BadRequest("Invalid email format");

                if (!ValidInput.IsPasswordSecure(request.Password))
                    return BadRequest("Password does not meet security requirements");

                var existingAccount = await _accountService.GetAccountByEmail(request.Email);
                if (existingAccount != null)
                    return Conflict("Email already exists");

                var account = new Account
                {
                    Email = request.Email,
                    Password = HashString.ToHashString(request.Password),
                    RoleId = request.RoleId,
                    Status = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await _accountService.CreateAccount(account);
                return CreatedAtAction(nameof(GetAccount), new { id = result.AccountId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdminAccount([FromBody] CreateAccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!ValidInput.IsEmailValid(request.Email))
                    return BadRequest("Invalid email format");

                if (!ValidInput.IsPasswordSecure(request.Password))
                    return BadRequest("Password does not meet security requirements");

                var existingAccount = await _accountService.GetAccountByEmail(request.Email);
                if (existingAccount != null)
                    return Conflict("Email already exists");

                var account = new Account
                {
                    Email = request.Email,
                    Password = HashString.ToHashString(request.Password),
                    RoleId = request.RoleId,
                    Status = true
                };

                var result = await _accountService.CreateAccountAdmin(account);
                return CreatedAtAction(nameof(GetAccount), new { id = result.AccountId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating admin account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<IActionResult> LockAccount(int id)
        {
            try
            {
                var result = await _accountService.LockAccount(id);
                
                if (!result)
                    return NotFound("Account not found");

                return Ok(new { message = "Account locked successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error locking account: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<IActionResult> UnlockAccount(int id)
        {
            try
            {
                var result = await _accountService.UnlockAccount(id);
                
                if (!result)
                    return NotFound("Account not found");

                return Ok(new { message = "Account unlocked successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unlocking account: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var result = _accountService.DeleteAccount(id);
                
                if (!result)
                    return NotFound("Account not found");

                return Ok(new { message = "Account deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
