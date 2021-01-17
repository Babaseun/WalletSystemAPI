using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletSystem.Services.Models
{
    public class Transaction
    {
        [Key]
        public string TransactionId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        [EnumDataType(typeof(TransactionType))]
        public string Type { get; set; }
        public string ApplicationUserId { get; set; }
        public bool Approved { get; set; }
        public string Currency { get; set; }
        [ForeignKey("WalletId")]
        public string WalletId { get; set; }


    }
}




