using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Services
{
    public class WalletServices : IWalletService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletRepository _walletRepository;

        public WalletServices(UserManager<ApplicationUser> userManager,
                              IWalletRepository walletRepository)
        {
            _userManager = userManager;
            _walletRepository = walletRepository;
        }

        public async Task<Response<Wallet>> CreateWallet(WalletDto model)
        {
            var user = await _userManager.FindByIdAsync(model.OwnerId);

            var response = new Response<Wallet>();

            var role = await _userManager.IsInRoleAsync(user, "Elite");

            if (role)
            {
                var account = new Account
                {
                    AccountId = Guid.NewGuid().ToString(),
                    ApplicationUserId = user.Id,
                    Wallet = new Wallet
                    {
                        WalletId = Guid.NewGuid().ToString(),
                        Balance = 0.0M,
                        Currency = model.Currency
                    }
                };

                response.Success = true;
                response.Message = "Wallet successfully created";
                response.Data = account.Wallet;

                await _walletRepository.Save(account.Wallet);

                return response;

            }
            response.Success = false;
            response.Message = "You are not authorized to create a account";

            return response;

        }

    }
}
