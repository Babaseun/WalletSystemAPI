using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _ctx;


        public TransactionRepository(AppDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public new async Task Save(Transaction transaction)
        {
            await _ctx.Transactions.AddAsync(transaction);
            await _ctx.SaveChangesAsync();
        }

        public new async Task<Transaction> Find(string id)
        {
            var transaction = await _ctx.Transactions.FindAsync(id);
            return transaction;

        }

        public new async Task Update(Transaction transaction)
        {
            _ctx.Transactions.Update(transaction);
            await _ctx.SaveChangesAsync();
        }
    }
}
