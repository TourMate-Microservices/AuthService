using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Repositories.IRepositories;
using TourMate.AuthService.Repositories.Models;

namespace TourMate.AuthService.Services.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshToken?> GetByRefreshToken(string refreshToken)
        {
            return await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        }

        public async Task<bool> UpdateRefreshToken(RefreshToken refreshToken)
        {
            return await _refreshTokenRepository.UpdateAsync(refreshToken);
        }

        public async Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken)
        {
            return await _refreshTokenRepository.CreateAndReturnAsync(refreshToken);
        }
    }
}
