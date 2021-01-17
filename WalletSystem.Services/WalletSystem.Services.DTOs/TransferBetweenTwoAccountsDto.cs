using System.ComponentModel.DataAnnotations;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.DTOs
{
    public class TransferBetweenTwoAccountsDto
    {
        [Required]
        public decimal Amount { get; set; }
        [EnumDataType(typeof(TransactionType))]
        public string Type { get; set; }
        public string OwnerIdOfAccountFrom { get; set; }
        public string OwnerIdOfAccountTo { get; set; }
        public string WalletIdOfAccountFrom { get; set; }
        public string WalletIdOfAccountTo { get; set; }


    }
}
