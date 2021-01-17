using System.ComponentModel.DataAnnotations;

namespace WalletSystem.Services.DTOs
{
    public class WalletDto
    {

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string Currency { get; set; }


    }
}

