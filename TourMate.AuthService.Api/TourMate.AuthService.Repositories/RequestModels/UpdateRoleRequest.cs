using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourMate.AuthService.Repositories.RequestModels
{
    public class UpdateRoleRequest
    {
        public string RoleName { get; set; } = string.Empty;
    }
}
