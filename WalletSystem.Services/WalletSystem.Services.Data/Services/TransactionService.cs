﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        public TransactionService(ITransactionRepository transactionRepository,
                                  IAccountRepository accountRepository,
                                  IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _walletRepository = walletRepository;

        }

        public async Task<Response<Transaction>> Deposit(TransactionDto model, string userId)
        {
            var response = new Response<Transaction>();
            var accounts = await _accountRepository.FindAll();


            if (accounts == null)
            {
                response.Success = false;
                response.Message = "No account exist for these user";

                return response;
            }

            var selectedAccount = accounts.FirstOrDefault(a => a.ApplicationUserId == userId);


            if (selectedAccount == null)
            {
                response.Success = false;
                response.Message = $"There is no wallet for these user";

                return response;
            }


            Transaction transaction = new Transaction
            {
                Amount = model.Amount,
                ApplicationUserId = selectedAccount.ApplicationUserId,
                Type = model.Type,
                TransactionId = Guid.NewGuid().ToString(),
                Currency = model.Currency,
                WalletId = model.WalletId

            };

            await _transactionRepository.Save(transaction);

            response.Message = "Transaction was created successfully and its waiting for approval";
            response.Success = true;
            response.Data = transaction;

            return response;

        }
        public async Task<Response<Wallet>> Withdraw(TransactionDto model)
        {
            var response = new Response<Wallet>();
            var accounts = await _accountRepository.FindAll();
            var wallets = await _walletRepository.FindAll();


            if (accounts == null)
            {
                response.Success = false;
                response.Message = "No account exist for these user";

                return response;
            }

            var selectedAccount = accounts.FirstOrDefault(a => a.ApplicationUserId == model.OwnerId);


            if (selectedAccount == null)
            {
                response.Success = false;
                response.Message = $"There is no wallet for these user";

                return response;
            }



            var wallet = await _walletRepository.Find(model.WalletId);

            if (wallet.Balance <= 0)
            {
                response.Message = "Insufficient funds";
                response.Success = false;

            }
            Transaction transaction = new Transaction
            {
                Amount = model.Amount,
                ApplicationUserId = selectedAccount.ApplicationUserId,
                Type = model.Type,
                TransactionId = Guid.NewGuid().ToString(),
                Currency = model.Currency

            };

            await _transactionRepository.Save(transaction);

            wallet.Balance -= model.Amount;

            await _walletRepository.Update(wallet);

            response.Message = "Transaction was successful";
            response.Success = true;
            response.Data = wallet;

            return response;

        }

        public async Task<Response<Wallet>> Approve(UpdateTransactionDto model, string adminId)
        {

            var response = new Response<Wallet>();


            var transaction = await _transactionRepository.Find(model.TransactionId);

            if (transaction == null)
            {
                response.Success = false;
                response.Message = "Transaction does not exist for these user";
                return response;

            }

            transaction.Approved = model.Approve;

            var wallet = await _walletRepository.Find(transaction.WalletId);

            var currencies = await HttpClientService.LoadCurrencies();


            var rate = currencies[wallet.Currency];
            if (transaction.Type == "Credit")
            {
                wallet.Balance += transaction.Amount * rate;

                response.Message = "Credit transaction approved successfully";
                response.Data = wallet;
                response.Success = true;

                await _transactionRepository.Update(transaction);
                await _walletRepository.Update(wallet);
                return response;
            }

            wallet.Balance -= transaction.Amount * rate;
            response.Message = "Debit transaction approved successfully";
            response.Data = wallet;
            response.Success = true;

            await _transactionRepository.Update(transaction);
            await _walletRepository.Update(wallet);

            return response;

        }

        public async Task<Response<IEnumerable<Wallet>>> Transfer(TransferBetweenTwoAccountsDto model)
        {
            var response = new Response<IEnumerable<Wallet>>();

            var walletTo = await _walletRepository.Find(model.WalletIdOfAccountTo);
            var walletFrom = await _walletRepository.Find(model.WalletIdOfAccountFrom);

            if (walletTo == null)
            {
                response.Success = false;
                response.Message = $"Wallet with ID {model.WalletIdOfAccountTo} does not exist";
                return response;
            }
            if (walletFrom == null)
            {
                response.Success = false;
                response.Message = $"Wallet with ID {model.WalletIdOfAccountFrom} does not exist";
                return response;
            }


            if (walletFrom.Balance <= 0)
            {
                response.Success = false;
                response.Message = $"Insufficient funds";
                return response;
            }

            walletFrom.Balance -= model.Amount;
            walletTo.Balance += model.Amount;

            await _walletRepository.Update(walletFrom);
            await _walletRepository.Update(walletTo);

            response.Success = true;
            response.Message = "Transfer successful";
            response.Data = new List<Wallet> { walletFrom, walletTo };
            return response;

        }

    }
}
