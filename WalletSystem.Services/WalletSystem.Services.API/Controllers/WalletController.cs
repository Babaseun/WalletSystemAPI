using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.DTOs;

namespace WalletSystem.Services.API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateWallet(WalletDto model)
        {


            var response = await _walletService.CreateWallet(model);
            if (response.Success) return Ok(response);

            return BadRequest(response);

        }
    }
}
