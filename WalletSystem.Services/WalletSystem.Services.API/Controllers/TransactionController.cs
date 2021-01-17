using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Data.Services;
using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;


namespace WalletSystem.Services.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ITransactionRepository transactionRepository,
                                     IAccountRepository accountRepository,
                                     IWalletRepository walletRepository,
                                     UserManager<ApplicationUser> userManager)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _walletRepository = walletRepository;
            _userManager = userManager;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> DepositTransaction(TransactionDto model)
        {
            if (ModelState.IsValid)
            {
                var transaction = new TransactionService(_transactionRepository, _accountRepository, _walletRepository, _userManager);
                var response = await transaction.Deposit(model);

                if (!response.Success) return BadRequest(response);

                return Ok(response);

            }

            return BadRequest();


        }
        [HttpPost("approve/{adminId}")]
        public async Task<IActionResult> ApproveTransaction(UpdateTransactionDto model, string adminId)
        {
            if (ModelState.IsValid)
            {
                var transaction = new TransactionService(_transactionRepository, _accountRepository, _walletRepository, _userManager);
                var response = await transaction.Approve(model, adminId);

                if (!response.Success) return BadRequest(response);

                return Ok(response);

            }

            return BadRequest();


        }

        [HttpPost("transfer-between-accounts")]
        public async Task<IActionResult> TransferBetweenAccounts(TransferBetweenTwoAccountsDto model)
        {
            if (ModelState.IsValid)
            {

                var transaction = new TransactionService(_transactionRepository, _accountRepository, _walletRepository, _userManager);

                var response = await transaction.Transfer(model);
                var toJson = JsonConvert.SerializeObject(response);


                if (!response.Success) return BadRequest(toJson);

                return Ok(toJson);
            }

            return BadRequest();
        }
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawalTransaction(TransactionDto model)
        {
            if (ModelState.IsValid)
            {
                var transaction = new TransactionService(_transactionRepository, _accountRepository, _walletRepository, _userManager);
                var response = await transaction.Withdraw(model);

                if (!response.Success) return BadRequest(response);

                return Ok(response);

            }

            return BadRequest();


        }
    }
}
