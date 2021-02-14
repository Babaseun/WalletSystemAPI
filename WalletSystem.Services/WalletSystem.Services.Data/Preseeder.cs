using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletSystem.Services.Data.Interfaces;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data
{
    public static class PreSeeder
    {
        public static async Task Seeder(AppDbContext ctx,
                                        RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        IAccountRepository accountRepository)
        {
            await ctx.Database.EnsureCreatedAsync();

            if (!roleManager.Roles.Any())
            {
                var listOfRoles = new List<IdentityRole>
                {
                    new IdentityRole("Noob"),
                    new IdentityRole("Elite"),
                    new IdentityRole("Admin")
                };

                foreach (var role in listOfRoles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {
                var listOfUsers = new List<ApplicationUser>
                {
                    new ApplicationUser {UserName = "yemi@mail.com", Email = "yemi@mail.com",},
                    new ApplicationUser {UserName = "kk@mail.com", Email = "kk@mail.com",},
                    new ApplicationUser {UserName = "ababaseun@gmail.com", Email = "ababaseun@mail.com",},
                };
                int counter = 0;

                foreach (var user in listOfUsers)
                {
                    var result = await userManager.CreateAsync(user, "P@$$word1");

                    if (result.Succeeded)
                    {
                        if (counter == 0)
                        {
                            await userManager.AddToRoleAsync(user, "Noob");

                            await accountRepository.Save(new Account
                            {
                                AccountId = Guid.NewGuid().ToString(),
                                ApplicationUserId = user.Id,
                                Wallet = new Wallet
                                {
                                    WalletId = Guid.NewGuid().ToString(),
                                    Balance = 0.0M,
                                    Currency = "dollar",
                                    Main = true
                                }
                            });
                        }

                        if (counter == 1)
                        {
                            await userManager.AddToRoleAsync(user, "Elite");
                            await accountRepository.Save(new Account
                            {
                                AccountId = Guid.NewGuid().ToString(),
                                ApplicationUserId = user.Id,
                                Wallet = new Wallet
                                {
                                    WalletId = Guid.NewGuid().ToString(),
                                    Balance = 0.0M,
                                    Currency = "naria",
                                    Main = true

                                }
                            });
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                    }

                    counter++;
                }
            }
        }
    }
}