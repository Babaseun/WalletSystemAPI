using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _ctx;


        public AccountRepository(AppDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public new async Task Save(Account account)
        {
            await _ctx.Accounts.AddAsync(account);
            await _ctx.SaveChangesAsync();
        }

        public new async Task<Account> Find(string id)
        {
            var account = await _ctx.Accounts.FindAsync(id);
            return account;

        }
        public new async Task<IEnumerable<Account>> FindAll()
        {
            var accounts = await _ctx.Accounts.ToListAsync();
            return accounts;

        }
    }
}
