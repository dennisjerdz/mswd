using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MSWD.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }

        public string CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Display(Name = "Middle Name/Initial")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool IsDisabled { get; set; }

        public string getFullName()
        {
            return $"{GivenName} {MiddleName} {LastName}";
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            ApplicationDbContext db = new ApplicationDbContext();
            City mktCity = db.Cities.FirstOrDefault(c => c.Name == "Makati");

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("CityId", this.CityId.ToString()));
            userIdentity.AddClaim(new Claim("CityName", mktCity.Name.ToString()));
            userIdentity.AddClaim(new Claim("FullName", this.getFullName()));

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