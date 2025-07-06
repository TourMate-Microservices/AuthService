using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourMate.AuthService.Repositories.GenericRepository;
using TourMate.AuthService.Repositories.IRepositories;
using TourMate.AuthService.Repositories.Models;

namespace TourMate.AuthService.Repositories.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}
