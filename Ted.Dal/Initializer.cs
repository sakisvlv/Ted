using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ted.Model.Auth;

namespace Ted.Dal
{
    public static class Initializer
    {
        public async static Task Initialize(Context context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            context.Database.EnsureCreated();


            // Look for any users or wheel settings
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            await roleManager.CreateAsync(new Role() { Name = "User" });
            await roleManager.CreateAsync(new Role() { Name = "Admin" });

            User u;
            for (int i = 0; i < 10; i++)
            {
                u = new User();
                u.Email = "user" + i + "@gmail.com";
                u.UserName = u.Email;
                u.FirstName = "User" + i;
                u.LastName = "Userovic" + i;
                u.PhoneNumber = "6981234567";
                var result = await userManager.CreateAsync(u, "1");
            }
            var users = context.Users.Where(x => true).ToList();
            for (int i = 0; i < users.Count; i++)
            {
                
                if (i == 0)
                {
                    await userManager.AddToRoleAsync(users[i], "Admin");
                }
            }
            context.SaveChanges();


            // to write seed method
        }
    }
}