using TourMate.AuthService.Services.Models;
using TourMate.AuthService.Api.Protos;
using TourMate.AuthService.Services.IGrpcServices;

namespace TourMate.AuthService.Services.GrpcServices
{
    public class NotificationGrpcService : INotificationGrpcService
    {
        private readonly NotificationService.NotificationServiceClient _notificationGrpcClient;

        public NotificationGrpcService(NotificationService.NotificationServiceClient notificationGrpcClient)
        {
            _notificationGrpcClient = notificationGrpcClient;
        }

        public async Task<bool> SendEmailAsync(EmailDto email)
        {
            try
            {
                var request = new SendEmailRequest
                {
                    To = email.ToEmail,
                    Subject = email.Subject,
                    Body = email.Body
                };
                
                var reply = await _notificationGrpcClient.SendEmailAsync(request);
                return reply.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
