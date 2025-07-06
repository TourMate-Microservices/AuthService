using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourMate.AuthService.Repositories.ResponseModels
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
