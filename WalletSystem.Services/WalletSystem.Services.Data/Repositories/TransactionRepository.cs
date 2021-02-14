using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {

        public TransactionRepository(AppDbContext ctx) : base(ctx) { }

    }
}
