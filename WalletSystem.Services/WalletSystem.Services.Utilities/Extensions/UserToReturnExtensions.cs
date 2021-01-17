using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Utilities.Extensions
{
    public static class UserToReturnExtensions
    {
        public static UserToReturnDto AsDto(this ApplicationUser user)
        {
            return new UserToReturnDto
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
