using System.Threading.Tasks;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Interfaces
{
    public interface IAccountService
    {
        Task<Response<Wallet>> CreateAccount(ApplicationUser user, string currency);
    }
}
