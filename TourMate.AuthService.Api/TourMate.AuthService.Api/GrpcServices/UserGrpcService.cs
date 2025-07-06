using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Services.Models;
using UserService.Grpc;

namespace TourMate.AuthService.Api.GrpcServices
{
    public class UserGrpcService : IUserGrpcService
    {
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;

        public UserGrpcService(UserGrpc.UserGrpcClient userGrpcClient)
        {
            _userGrpcClient = userGrpcClient;
        }

        public async Task<UserDto?> GetCustomerByAccountIdAsync(int accountId)
        {
            try
            {
                var request = new GetByAccountIdRequest { AccountId = accountId };
                var reply = await _userGrpcClient.GetCustomerByAccountIdAsync(request);
                
                return new UserDto { FullName = reply.FullName };
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserDto?> GetTourGuideByAccountIdAsync(int accountId)
        {
            try
            {
                var request = new GetByAccountIdRequest { AccountId = accountId };
                var reply = await _userGrpcClient.GetTourGuideByAccountIdAsync(request);
                
                return new UserDto { FullName = reply.FullName };
            }
            catch
            {
                return null;
            }
        }
    }
}
