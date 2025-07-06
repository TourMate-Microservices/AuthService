using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Services.Models;

namespace TourMate.AuthService.Services.IServices
{
    public interface IUserGrpcService
    {
        Task<UserDto?> GetCustomerByAccountIdAsync(int accountId);
        Task<UserDto?> GetTourGuideByAccountIdAsync(int accountId);
    }
}
