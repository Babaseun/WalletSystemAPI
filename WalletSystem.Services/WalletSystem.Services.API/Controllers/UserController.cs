using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
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
                              ILogger<ApplicationUser> logger,
                              IAccountService accountService,
                              IConfiguration configuration)
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
            if (!ModelState.IsValid) return BadRequest(new { message = "Bad Request", ModelState });

            if (model.Role != "Noob" && model.Role != "Elite" && model.Role != "Admin")
                return BadRequest(new { message = "User can either be an Admin , a Noob or an Elite" });

            try
            {
                var currency = await HttpClientService.LoadCurrencies();

                if (!currency.ContainsKey(model.Currency))
                    return BadRequest(new { message = "Please select a known currency" });

                var user = new ApplicationUser { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) return BadRequest(new { message = result.Errors });
                _logger.LogInformation("User created a new account with password.");


                await _userManager.AddToRoleAsync(user, model.Role);


                if (model.Role != "Noob" && model.Role != "Elite") return Created("", new { user = user.AsDto() });
                var account = await _accountService.CreateAccount(user, model.Currency);

                if (!account.Success) return BadRequest(new { message = "Failed to create wallet" });
                _logger.LogInformation("User created a new wallet.");



                return Created("", new { user = user.AsDto(), account });

            }
            catch (Exception e)
            {
                _logger.LogInformation("User created a new wallet.");
                return BadRequest(new { message = "Bad Request", error = e.Message });

            }

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null) return BadRequest(new { message = "Invalid login credentials" });

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)).ToList();

                    authClaims.Add(new Claim("Id", user.Id));

                    var helper = new Helper(_configuration);
                    var token = helper.GenerateToken(authClaims);

                    _logger.LogInformation("User logged in.");
                    return Ok(new { token });
                }

            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return BadRequest(new { message = "User not logged in" });
        }
    }
}

