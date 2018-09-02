using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MSWD.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public DbSet<City> Cities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientNote> ClientNotes { get; set; }
        public DbSet<ClientBeneficiary> ClientBeneficiary { get; set; }
        public DbSet<MobileNumber> MobileNumbers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SeniorCitizen> SeniorCitizens { get; set; }
        public DbSet<Pwd> Pwds { get; set; }
        public DbSet<SoloParent> SoloParents { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RequirementAttachment> RequirementAttachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeniorCitizen>()
            .HasKey(s => s.SeniorCitizenId)
            .HasRequired(s => s.Client)
            .WithOptional(c => c.SeniorCitizen)
            .Map(s => s.MapKey("ClientId"));

            modelBuilder.Entity<Pwd>()
            .HasKey(s => s.PwdId)
            .HasRequired(s => s.Client)
            .WithOptional(c => c.Pwd)
            .Map(s => s.MapKey("ClientId"));

            modelBuilder.Entity<SoloParent>()
            .HasKey(s => s.SoloParentId)
            .HasRequired(s => s.Client)
            .WithOptional(c => c.SoloParent)
            .Map(s=>s.MapKey("ClientId"));

            base.OnModelCreating(modelBuilder);
        }

        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // One to One
            modelBuilder.Entity<Client>()
                        .HasOptional(c => c.SeniorCitizen)
                        .WithRequired(s => s.Client);

            modelBuilder.Entity<Client>()
                        .HasOptional(c => c.Pwd)
                        .WithRequired(s => s.Client);

            modelBuilder.Entity<Client>()
                        .HasOptional(c => c.SoloParent)
                        .WithRequired(s => s.Client);
        }*/

        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();
        }
    }
}