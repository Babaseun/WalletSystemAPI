using System.ComponentModel.DataAnnotations;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.DTOs
{
    public class TransactionDto
    {
        [Required]
        public decimal Amount { get; set; }
        [EnumDataType(typeof(TransactionType))]
        public string Type { get; set; }
        public string OwnerId { get; set; }
        public string Currency { get; set; }
        public string WalletId { get; set; }
    }
}
