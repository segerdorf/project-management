using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RosalieSegerdorf_MVC1.Data.Models;

namespace RosalieSegerdorf_MVC1.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RosalieSegerdorf_MVC1.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Data\Migrations";
        }

        protected override void Seed(DataContext context)
        {
            List<User> users = new List<User>
            {
                new User{
                    UserName = "Admin",
                    Email = "admin@spelforetaget.com",
                    FirstName = "Tom",
                    LastName = "Jones",
                    StartOfEmployment = DateTime.Now,
                    Address = "Kungsgatan 40, 702 11 Örebro",
                    PhoneNumber = "0765-221131"
                    
                },
                new User {
                    UserName = "ProjectLeader",
                    Email = "projectleader@spelforetaget.com",
                    FirstName = "Tintin",
                    LastName = "Fransson",
                    StartOfEmployment = DateTime.Now,
                    Address = "Röksvampsvägen 12, 705 10 Örebro",
                    PhoneNumber = "0763-221123"
                },
                new User {
                    UserName = "Employee",
                    Email = "employee@spelforetaget.com",
                    FirstName = "Majlise",
                    LastName = "Jones",
                    StartOfEmployment = DateTime.Now,
                    Address = "Kungsgatan 40, 702 11 Örebro",
                    PhoneNumber = "0709-443234"
                },
            };

            foreach (var itemUser in users)
            {
                if (!context.Roles.Any(r => r.Name == itemUser.UserName))
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var role = new IdentityRole { Name = itemUser.UserName };
                    roleManager.Create(role);
                }

                if (!context.Users.Any(u => u.UserName == itemUser.UserName))
                {
                    var userStore = new UserStore<User>(context);
                    var userManager = new UserManager<User>(userStore);
                    var user = itemUser;
                    userManager.Create(user, "test123");
                    userManager.AddToRole(user.Id, user.UserName);
                }
            }

        }
    }
}
