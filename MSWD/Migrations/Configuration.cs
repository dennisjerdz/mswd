namespace MSWD.Migrations
{
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
        }
    }
}
