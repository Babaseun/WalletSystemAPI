﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<Transaction>> Deposit(TransactionDto model, string userId);
        Task<Response<Wallet>> Withdraw(TransactionDto model);
        Task<Response<Wallet>> Approve(UpdateTransactionDto model, string adminId);
        Task<Response<IEnumerable<Wallet>>> Transfer(TransferBetweenTwoAccountsDto model);


    }
}
