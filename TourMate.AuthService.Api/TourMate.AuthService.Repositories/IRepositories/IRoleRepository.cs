using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Repositories.GenericRepository;
using TourMate.AuthService.Repositories.Models;

namespace TourMate.AuthService.Repositories.IRepositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
