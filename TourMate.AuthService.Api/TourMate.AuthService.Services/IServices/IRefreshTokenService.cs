using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Repositories.Models;

namespace TourMate.AuthService.Services.IServices
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken?> GetByRefreshToken(string refreshToken);
        Task<bool> UpdateRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken);
    }
}
