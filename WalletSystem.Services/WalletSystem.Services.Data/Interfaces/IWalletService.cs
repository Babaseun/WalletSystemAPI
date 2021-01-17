using System.Threading.Tasks;
using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Interfaces
{
    public interface IWalletService
    {
        Task<Response<Wallet>> CreateWallet(WalletDto model);

    }
}
