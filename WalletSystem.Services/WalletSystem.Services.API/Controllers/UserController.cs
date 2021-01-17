using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Data.Services;
using WalletSystem.Services.DTOs;
using WalletSystem.Services.Models;
using WalletSystem.Services.Utilities.Extensions;
using WalletSystem.Services.Utilities.Helpers;

namespace WalletSystem.Services.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ILogger<ApplicationUser> logger, IAccountService accountService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accountService = accountService;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {

                if (model.Role != "Noob" && model.Role != "Elite" && model.Role != "Admin")
                    return BadRequest(new { message = "User can either be an Admin , a Noob or an Elite" });

                var currencies = new HttpClientService();
                var currency = await currencies.LoadCurrencies();
                if (!currency.ContainsKey(model.Currency)) return BadRequest(new { message = "Please select a known currency" });
                var user = new ApplicationUser { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    await _userManager.AddToRoleAsync(user, model.Role);


                    if (model.Role == "Noob" || model.Role == "Elite")
                    {


                        var account = await _accountService.CreateAccount(user, model.Currency);

                        if (account.Success)
                        {
                            _logger.LogInformation("User created a new wallet.");

                            Helper helper = new Helper(_configuration);
                            var token = helper.GenerateToken(user.Id, model.Role);

                            return Created("", new { user = user.AsDto(), account, token });
                        }

                        return BadRequest(new { message = "Failed to create wallet" });
                    }

                    return Created("", new { user = user.AsDto() });
                }

                return BadRequest(new { message = result.Errors });

            }
            return BadRequest(new { message = "Bad Request", ModelState });

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {



            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null) return BadRequest(new { message = "User not registered" });

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {


                    _logger.LogInformation("User logged in.");

                    return Ok(new { message = "User logged in." });
                }

            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return BadRequest(new { message = "User not logged in" });


        }
    }
}

