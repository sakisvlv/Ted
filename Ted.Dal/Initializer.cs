using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Dal
{
    public static class Initializer
    {
        public static void Initialize(Context context, UserManager<User> userManager)
        {
            context.Database.EnsureCreated();


            // Look for any users or wheel settings
            if (context.Users.Any() || context.Users.Any())
            {
                return;   // DB has been seeded
            }

            // to write seed method
        }
    }
}