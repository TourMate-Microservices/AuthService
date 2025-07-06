using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Repositories.Models;

namespace TourMate.AuthService.Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<Role> CreateRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
