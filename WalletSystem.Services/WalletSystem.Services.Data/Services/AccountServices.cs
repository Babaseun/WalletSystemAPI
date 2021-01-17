using System;
using System.Linq;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Services
{
    public class AccountServices : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AppDbContext _ctx;

        public AccountServices(IAccountRepository accountRepository, AppDbContext ctx)
        {
            _accountRepository = accountRepository;
            _ctx = ctx;
        }

        public async Task<Response<Wallet>> CreateAccount(ApplicationUser user, string currency)
        {
            var response = new Response<Wallet>();

            var hasAnAccount = _ctx.Accounts.FirstOrDefault(x => x.ApplicationUserId == user.Id);

            if (hasAnAccount == null)
            {
                var account = new Account
                {
                    AccountId = Guid.NewGuid().ToString(),
                    ApplicationUserId = user.Id,
                    Wallet = new Wallet
                    {
                        WalletId = Guid.NewGuid().ToString(),
                        Balance = 0.0M,
                        Currency = currency,
                        Main = true
                    },
                };


                await _accountRepository.Save(account);

                response.Success = true;
                response.Data = account.Wallet;
                response.Message = "Wallet created successfully";

            }
            else
            {

                var account = new Account
                {
                    AccountId = Guid.NewGuid().ToString(),
                    ApplicationUserId = user.Id,
                    Wallet = new Wallet
                    {
                        WalletId = Guid.NewGuid().ToString(),
                        Balance = 0.0M,
                        Currency = currency,
                        Main = false
                    },
                };


                await _accountRepository.Save(account);

                response.Success = true;
                response.Data = account.Wallet;
                response.Message = "Wallet created successfully";

            }
            return response;


        }

    }
}

