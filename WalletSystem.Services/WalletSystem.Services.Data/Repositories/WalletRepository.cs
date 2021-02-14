using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {

        public WalletRepository(AppDbContext ctx) : base(ctx)
        {
        }
    }
}
