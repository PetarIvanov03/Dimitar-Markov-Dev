using LegalTranslation.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LegalTranslation.Data
{
    public static class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Languages.Any())
                {
                    context.Languages.AddRange(new List<Language>()
                    {
                        new Language()
                        {
                            Name = "Bulgarian",
                            IsActive = true,
                        },
                        new Language()
                        {
                            Name = "English",
                            IsActive = true,
                        }
                    });

                    context.SaveChanges();

                }


            }

        }

        public static void SeedEmails(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Emails.Any())
                {
                    context.Emails.AddRange(new List<Emails>()
                    {
                        new Emails()
                        {
                            Name = "testpetar1337@gmail.com",
                            Password = "uehb qajg ssuf qcyy",
                            IsAdmin = true
                        },
                        new Emails()
                        {
                            Name = "i.petarivanov03@gmail.com",
                            IsAdmin = false
                        }
                    });

                    context.SaveChanges();
                }


            }

        }

        public static async Task SeedUsersAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Worker))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Worker));


                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                //Admin
                if (userManager.Users.Count() == 0)
                {
                    string adminUserEmail = "random123sad@gmail.com";

                    var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                    if (adminUser == null)
                    {
                        var newAdminUser = new AppUser()
                        {
                            UserName = adminUserEmail,
                            Email = adminUserEmail,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(newAdminUser, "Dsfkhghvsaouasb12!@");
                        await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                    }

                    string workerUserEmail = "randomworker123sad@gmail.com";

                    var workerUser = await userManager.FindByEmailAsync(workerUserEmail);
                    if (workerUser == null)
                    {
                        var newWorkerUser = new AppUser()
                        {
                            UserName = workerUserEmail,
                            Email = workerUserEmail,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(newWorkerUser, "Dsfkhghvsaadas85412!@");
                        await userManager.AddToRoleAsync(newWorkerUser, UserRoles.Worker);

                    }


                }
            }
        }
    }
}
