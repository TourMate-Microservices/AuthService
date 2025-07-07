using Microsoft.AspNetCore.Mvc;
using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Repositories.Models;
using TourMate.AuthService.Repositories.RequestModels;


namespace TourMate.AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all roles");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                
                if (role == null)
                    return NotFound("Role not found");

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name/{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            try
            {
                var role = await _roleService.GetRoleByNameAsync(roleName);
                
                if (role == null)
                    return NotFound("Role not found");

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role by name: {RoleName}", roleName);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingRole = await _roleService.GetRoleByNameAsync(request.RoleName);
                if (existingRole != null)
                    return Conflict("Role already exists");

                var role = new Role
                {
                    RoleName = request.RoleName
                };

                var result = await _roleService.CreateRoleAsync(role);
                return CreatedAtAction(nameof(GetRole), new { id = result.RoleId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingRole = await _roleService.GetRoleByIdAsync(id);
                if (existingRole == null)
                    return NotFound("Role not found");

                existingRole.RoleName = request.RoleName;

                var result = await _roleService.UpdateRoleAsync(existingRole);
                
                if (!result)
                    return StatusCode(500, "Failed to update role");

                return Ok(existingRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var existingRole = await _roleService.GetRoleByIdAsync(id);
                if (existingRole == null)
                    return NotFound("Role not found");

                var result = await _roleService.DeleteRoleAsync(id);
                
                if (!result)
                    return StatusCode(500, "Failed to delete role");

                return Ok(new { message = "Role deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


