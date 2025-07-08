using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using TourMate.AuthService.Api.Protos;
using TourMate.AuthService.Services.IGrpcServices;
using TourMate.AuthService.Services.Models;
using static TourMate.AuthService.Api.Protos.UserService;


namespace TourMate.AuthService.Services.GrpcServices
{
    public class UserGrpcService : IUserGrpcService
    {
        private readonly UserServiceClient _userGrpcClient;

        public UserGrpcService(UserServiceClient userGrpcClient)
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
