using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Services.Models;

namespace TourMate.AuthService.Services.IServices
{
    public interface INotificationGrpcService
    {
        Task<bool> SendEmailAsync(EmailDto email);
    }
}
