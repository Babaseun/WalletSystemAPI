using System.ComponentModel.DataAnnotations;

namespace WalletSystem.Services.DTOs
{
    public class UpdateTransactionDto
    {

        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public bool Approve { get; set; }
        [Required]
        public string WalletId { get; set; }

    }
}
