using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletSystem.Services.Models
{
    public class Account
    {
        [Key]
        public string AccountId { get; set; }
        public Wallet Wallet { get; set; }
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
    }
}
