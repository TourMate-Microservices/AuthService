using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Repositories.GenericRepository;
using TourMate.AuthService.Repositories.Models;
using TourMate.AuthService.Repositories.ResponseModels;


namespace TourMate.AuthService.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account?> GetByAccountAndRoleAsync(int id, string role);
        Task<Account?> GetRoleByAccountId(int id);
        //Task<List<AccountSearchResult>> SearchAccountsByNameAsync(string searchTerm, int excludeUserId);
        Task<Account?> GetAccountByLogin(string email, string password);
        Task<Account?> GetAccountByEmail(string email);
        Task<Account?> CreateAdmin(Account entity);
        Task<bool> LockAccount(int id);
        Task<bool> UnlockAccount(int id);
    }
}
