using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Services.Models;
using NotificationService.Grpc;

namespace TourMate.AuthService.Api.GrpcServices
{
    public class NotificationGrpcService : INotificationGrpcService
    {
        private readonly NotificationGrpc.NotificationGrpcClient _notificationGrpcClient;

        public NotificationGrpcService(NotificationGrpc.NotificationGrpcClient notificationGrpcClient)
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
