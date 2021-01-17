using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletSystem.Services.Models
{
    public class Wallet
    {
        [Key]
        public string WalletId { get; set; }
        [DataType(DataType.Currency)]
        public string Currency { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Balance { get; set; }
        public bool Main { get; set; }
    }
}
