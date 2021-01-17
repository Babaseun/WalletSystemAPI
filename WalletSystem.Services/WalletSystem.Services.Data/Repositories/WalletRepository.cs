using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        private readonly AppDbContext _ctx;


        public WalletRepository(AppDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }


        public new async Task<Wallet> Find(string id)
        {
            var wallet = await _ctx.Wallets.FindAsync(id);
            return wallet;

        }
        public new async Task<IEnumerable<Wallet>> FindAll()
        {
            var wallets = await _ctx.Wallets.ToListAsync();
            return wallets;

        }
    }
}
