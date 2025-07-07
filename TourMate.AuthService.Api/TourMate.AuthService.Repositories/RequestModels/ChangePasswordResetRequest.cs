using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourMate.AuthService.Repositories.RequestModels
{
    public class RequestPasswordResetRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
