using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Repositories.Context;
using TourMate.AuthService.Repositories.GenericRepository;
using TourMate.AuthService.Repositories.IRepositories;
using TourMate.AuthService.Repositories.Models;
using TourMate.AuthService.Repositories.ResponseModels;


namespace TourMate.AuthService.Repositories.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(TourMateAuthContext context) : base(context) { }

        public async Task<Account?> GetByAccountAndRoleAsync(int id, string role)
        {
            var query = _context.Accounts
                .Include(a => a.Role);

            return await query
                .FirstOrDefaultAsync(a => a.AccountId == id && a.Role.RoleName == role);
        }

        public async Task<Account?> GetRoleByAccountId(int id)
        {
            var query = _context.Accounts
                .Include(a => a.Role).FirstOrDefault(a => a.AccountId == id);
            return query;
        }


        //public async Task<List<AccountSearchResult>> SearchAccountsByNameAsync(string searchTerm, int excludeUserId)
        //{
        //    searchTerm = searchTerm?.ToLower() ?? "";

        //    // Query Customers có tên phù hợp
        //    var customersQuery = from a in _context.Accounts
        //                         join c in _context.Customers on a.AccountId equals c.AccountId
        //                         where a.AccountId != excludeUserId
        //                               && a.RoleId == 2
        //                               && c.FullName.ToLower().Contains(searchTerm)
        //                         select new AccountSearchResult
        //                         {
        //                             AccountId = a.AccountId,
        //                             FullName = c.FullName,
        //                             RoleId = a.RoleId
        //                         };

        //    // Query TourGuides có tên phù hợp
        //    var guidesQuery = from a in _context.Accounts
        //                      join g in _context.TourGuides on a.AccountId equals g.AccountId
        //                      where a.AccountId != excludeUserId
        //                            && a.RoleId == 3
        //                            && g.FullName.ToLower().Contains(searchTerm)
        //                      select new AccountSearchResult
        //                      {
        //                          AccountId = a.AccountId,
        //                          FullName = g.FullName,
        //                          RoleId = a.RoleId
        //                      };

        //    // Union kết quả 2 bảng
        //    var result = await customersQuery
        //        .Union(guidesQuery)
        //        .ToListAsync();

        //    return result;
        //}

        public async Task<Account?> GetAccountByLogin(string email, string password)
        {
            return await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.Email == email && a.Password == password && a.Status);
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account?> CreateAdmin(Account entity)
        {
            try
            {
                entity.Role = null;
                _context.Add(entity);

                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> LockAccount(int id)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
                if (account == null)
                {
                    return false; // Không tìm thấy tài khoản
                }

                if (!account.Status)
                {
                    return true; // Tài khoản đã bị khóa trước đó
                }

                account.Status = false;
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UnlockAccount(int id)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
                if (account == null)
                {
                    return false; // Không tìm thấy tài khoản
                }

                if (account.Status)
                {
                    return true;
                }

                account.Status = true;
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
