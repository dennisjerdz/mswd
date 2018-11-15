namespace MSWD.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MSWD.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MSWD.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            /*seed cities*/
            context.Cities.AddOrUpdate(
                c => c.Name,
                new City { Name = "Makati", DateCreated = DateTime.UtcNow.AddHours(8) },
                new City { Name = "Mandaluyong", DateCreated = DateTime.UtcNow.AddHours(8) }
            );

            context.SaveChanges();

            int makati_city = context.Cities.FirstOrDefault(c => c.Name == "Makati").CityId;
            int mandaluyong_city = context.Cities.FirstOrDefault(c => c.Name == "Mandaluyong").CityId;

            /*seed roles*/
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!context.Roles.Any(r => r.Name == "Social Worker"))
            {
                var role = new IdentityRole { Name = "Social Worker" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "OIC"))
            {
                var role = new IdentityRole { Name = "OIC" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Client"))
            {
                var role = new IdentityRole { Name = "Client" };
                roleManager.Create(role);
            }

            /*seed accounts*/
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
            };

            if (!context.Users.Any(u => u.UserName == "makati-sw1@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    CityId = makati_city,
                    GivenName = "Makati Social Worker 1",
                    MiddleName = "",
                    LastName = "Test",
                    UserName = "makati-sw1@gmail.com",
                    Email = "makati-sw1@gmail.com",
                    EmailConfirmed = true,
                    IsDisabled = false
                };
                userManager.Create(user, "Testing@123");
                userManager.AddToRole(user.Id, "Social Worker");
            }
        }
    }
}
