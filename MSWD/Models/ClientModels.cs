using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MSWD.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual List<Client> Clients { get; set; }
    }

    public class Client
    {
        public Client()
        {

        }

        public Client(ClientEditModel ce)
        {
            GivenName = ce.GivenName;
            MiddleName = ce.MiddleName;
            SurName = ce.SurName;
            Gender = ce.Gender;
            CivilStatus = ce.CivilStatus;
            Occupation = ce.Occupation;
            Citizenship = ce.Citizenship;
            CityAddress = ce.CityAddress;

            ProvincialAddress = ce.ProvincialAddress;
            ContactNumber = ce.ContactNumber;
            TypeOfResidency = ce.TypeOfResidency;
            StartOfResidency = ce.StartOfResidency;
            BirthDate = ce.BirthDate;
            BirthPlace = ce.BirthPlace;
            Religion = ce.Religion;
            DateOfMarriage = ce.DateOfMarriage;
            PlaceOfMarriage = ce.PlaceOfMarriage;
            SpouseName = ce.SpouseName;
            SpouseBirthDate = ce.SpouseBirthDate;
            SpouseBluCardNo = ce.SpouseBluCardNo;
        }

        [Key]
        public int ClientId { get; set; }

        // Credentials
        [Required]
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string Gender { get; set; } // Male, Female
        [Required]
        public string CivilStatus { get; set; } // Single, Married, Widowed, Divorced

        public string getFullName()
        {
            return $"{GivenName} {MiddleName} {SurName}";
        }

        // Work
        public string Occupation { get; set; }
        public string Citizenship { get; set; }

        // Address
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        public string CityAddress { get; set; }
        public string ProvincialAddress { get; set; }
        public string ContactNumber { get; set; }

        // Residency
        public string TypeOfResidency { get; set; } // House Owner, Sharer, Lessee/Tenant, Boarder
        public DateTime? StartOfResidency { get; set; }

        public int getResidencyLength()
        {
            var residencyLength = DateTime.UtcNow.AddHours(8).Year - this.StartOfResidency.Value.Year;
            // Go back to the year the person was born in case of a leap year
            if (this.BirthDate > DateTime.UtcNow.AddHours(8).AddYears(-residencyLength)) residencyLength--;

            return residencyLength;
        }

        // Age
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string BirthPlace { get; set; }
        public string Religion { get; set; }

        public int getAge()
        {
            var age = DateTime.UtcNow.AddHours(8).Year - this.BirthDate.Year;
            // Go back to the year the person was born in case of a leap year
            if (this.BirthDate > DateTime.UtcNow.AddHours(8).AddYears(-age)) age--;

            return age;
        }

        // Marriage
        public DateTime? DateOfMarriage { get; set; }
        public string PlaceOfMarriage { get; set; }
        public string SpouseName { get; set; }
        public DateTime? SpouseBirthDate { get; set; }
        public string SpouseBluCardNo { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        //[ForeignKey("SeniorCitizen")]
        //public int SeniorCitizenId { get; set; }
        public virtual SeniorCitizen SeniorCitizen { get; set; }

        //[ForeignKey("Pwd")]
        //public int PwdId { get; set; }
        public virtual Pwd Pwd { get; set; }

        //[ForeignKey("SoloParent")]
        //public int SoloParentId { get; set; }
        public virtual SoloParent SoloParent { get; set; }
        public virtual List<Requirement> Requirements { get; set; }
        public virtual List<ClientNote> ClientNotes { get; set; }
        public virtual List<ClientBeneficiary> ClientBeneficiaries { get; set; }
    }

    public class ClientNote
    {
        public int ClientNoteId { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        public string Note { get; set; }
        public int Done { get; set; } // 1 or 0, show in dashboard

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }

    public class ClientBeneficiary
    {
        public int ClientBeneficiaryId { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public string Name { get; set; }
        public string Relationship { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ContactNumber { get; set; }
        public string CivilStatus { get; set; } // Single, Married, Widowed, Divorced
        public string Occupation { get; set; }
        public string Income { get; set; }

        public int getAge()
        {
            var age = DateTime.UtcNow.AddHours(8).Year - this.BirthDate.Value.Year;
            // Go back to the year the person was born in case of a leap year
            if (this.BirthDate > DateTime.UtcNow.AddHours(8).AddYears(-age)) age--;

            return age;
        }
    }

    public class SeniorCitizen
    {
        [Key]
        public int SeniorCitizenId { get; set; }

        public string Status { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("VerifiedBy")]
        public string VerifiedByUserId { get; set; }
        public ApplicationUser VerifiedBy { get; set; }

        public virtual Client Client { get; set; }
    }

    public class SoloParent
    {
        [Key]
        public int SoloParentId { get; set; }

        public string Status { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("VerifiedBy")]
        public string VerifiedByUserId { get; set; }
        public ApplicationUser VerifiedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Client Client { get; set; }
    }

    public class Pwd
    {
        [Key]
        public int PwdId { get; set; }

        public string Status { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("VerifiedBy")]
        public string VerifiedByUserId { get; set; }
        public ApplicationUser VerifiedBy { get; set; }

        public virtual Client Client { get; set; }
    }

    public class Requirement
    {
        public int RequirementId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual List<RequirementAttachment> Attachments { get; set; }
    }

    public class RequirementAttachment
    {
        public int RequirementAttachmentId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }

        public int RequirementId { get; set; }
        public virtual Requirement Requirement { get; set; }
    }

    public class MobileNumber
    {
        public int MobileNumberId { get; set; }

        [Required]
        public string MobileNo { get; set; }
        public string Token { get; set; }
        public bool IsDisabled { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual List<Message> Messages { get; set; }
    }

    public class Message
    {
        public int MessageId { get; set; }

        public int? MobileNumberId { get; set; }
        public virtual MobileNumber MobileNumber { get; set; }

        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ClientEditModel
    {
        public ClientEditModel() {

        }

        [Required]
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string Gender { get; set; } // Male, Female
        [Required]
        public string CivilStatus { get; set; } // Single, Married, Widowed, Divorced

        // Work
        public string Occupation { get; set; }
        public string Citizenship { get; set; }

        // Address
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        public string CityAddress { get; set; }
        public string ProvincialAddress { get; set; }
        public string ContactNumber { get; set; }

        // Residency
        public string TypeOfResidency { get; set; } // House Owner, Sharer, Lessee/Tenant, Boarder
        public DateTime? StartOfResidency { get; set; }

        // Age
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string BirthPlace { get; set; }
        public string Religion { get; set; }

        // Marriage
        public DateTime? DateOfMarriage { get; set; }
        public string PlaceOfMarriage { get; set; }
        public string SpouseName { get; set; }
        public DateTime? SpouseBirthDate { get; set; }
        public string SpouseBluCardNo { get; set; }

        // User who entered
        public string CreatedByUserId { get; set; }

        public List<ClientBeneficiary> ClientBeneficiaries { get; set; }
    }
}