using Microsoft.AspNetCore.Identity;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Seeder
    {
        /// <summary>
        /// Populates the Role and User Tables with Initial data
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        public static async void SeedDataBase(RoleManager<IdentityRole> roleManager, UserManager<Contact> userManager, ApplicationDbContext context)
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                if (!context.Users.Any())
                {
                    string roles = File.ReadAllText(@"ContactBook-main\JsonFiles\Roles.json");
                    List<IdentityRole> listOfRoles = JsonConvert.DeserializeObject<List<IdentityRole>>(roles);
                    //List<string> listOfRoles = new List<string> { "Admin", "Regular" };

                    foreach (var role in listOfRoles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    string users = File.ReadAllText(@"ContactBook-main\JsonFiles/Users.json");
                    List<Contact> listOfUsers = JsonConvert.DeserializeObject<List<Contact>>(users);
                    int i = 0;
                    foreach (var user in listOfUsers)
                    {
                        await userManager.CreateAsync(user, user.Password);

                        // Adds the first 5 users to the Admin role and the others to the regular role
                        if (i < 5)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                            i++;
                        }
                        else
                            await userManager.AddToRoleAsync(user, "Regular");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
