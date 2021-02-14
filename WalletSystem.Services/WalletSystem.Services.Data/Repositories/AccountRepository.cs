using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext ctx) : base(ctx) { }
    }
}
