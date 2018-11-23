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
        [Display(Name ="Date Created")]
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

        public Client(ClientPWDEditModel cpwde)
        {
            GivenName = cpwde.GivenName;
            MiddleName = cpwde.MiddleName;
            SurName = cpwde.SurName;
            Gender = cpwde.Gender;
            CivilStatus = cpwde.CivilStatus;
            Occupation = cpwde.Occupation;
            Citizenship = cpwde.Citizenship;
            CityAddress = cpwde.CityAddress;

            ProvincialAddress = cpwde.ProvincialAddress;
            ContactNumber = cpwde.ContactNumber;
            TypeOfResidency = cpwde.TypeOfResidency;
            StartOfResidency = cpwde.StartOfResidency;
            BirthDate = cpwde.BirthDate;
            BirthPlace = cpwde.BirthPlace;
            Religion = cpwde.Religion;
            DateOfMarriage = cpwde.DateOfMarriage;
            PlaceOfMarriage = cpwde.PlaceOfMarriage;
            SpouseName = cpwde.SpouseName;
            SpouseBirthDate = cpwde.SpouseBirthDate;
            SpouseBluCardNo = cpwde.SpouseBluCardNo;

            EducationalAttainment = cpwde.EducationalAttainment;
            School = cpwde.School;

            EmploymentStatus = cpwde.EmploymentStatus;
            Position = cpwde.Position;
            Company = cpwde.Company;

            NatureOfEmployer = cpwde.NatureOfEmployer;
            TypeOfEmployment = cpwde.TypeOfEmployment;

            TypeOfSkill = cpwde.TypeOfSkill;

            SSSNo = cpwde.SSSNo;
            GSISNo = cpwde.GSISNo;
            PhilhealthNo = cpwde.PhilhealthNo;
            PhilHealthMembershipType = cpwde.PhilHealthMembershipType;
            YellowCardNo = cpwde.YellowCardNo;
            YellowCardMembershipType = cpwde.YellowCardMembershipType;
        }

        [Key]
        public int ClientId { get; set; }

        // Credentials
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Sur Name")]
        public string SurName { get; set; }
        [Required]
        public string Gender { get; set; } // Male, Female
        [Required]
        [Display(Name = "Civil Status")]
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
        [Display(Name = "City Address")]
        public string CityAddress { get; set; }
        [Display(Name = "Provincial Address")]
        public string ProvincialAddress { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        // Residency
        [Display(Name = "Type Of Residency")]
        public string TypeOfResidency { get; set; } // House Owner, Sharer, Lessee/Tenant, Boarder
        [Display(Name = "Start Of Residency")]
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
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]
        [Display(Name = "Birth Place")]
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
        [Display(Name = "Date Of Marriage")]
        public DateTime? DateOfMarriage { get; set; }
        [Display(Name = "Place Of Marriage")]
        public string PlaceOfMarriage { get; set; }
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }
        [Display(Name = "Spouse Birth Date")]
        public DateTime? SpouseBirthDate { get; set; }
        [Display(Name = "Spouse Blu Card No")]
        public string SpouseBluCardNo { get; set; }

        public string School { get; set; }
        public string EducationalAttainment { get; set; }

        /*
            public bool? EAElementary { get; set; }
            public bool? EAElementaryUndergraduate { get; set; }
            public bool? EAHighSchool { get; set; }
            public bool? EAHighSchoolUndergraduate { get; set; }
            public bool? EACollege { get; set; }
            public bool? EACollegeUndergraduate { get; set; }
            public bool? EAGraduate { get; set; }
            public bool? EASPED { get; set; }
            public bool? EAPostGraduate { get; set; }
            public bool? EAVocational { get; set; }
            public bool? EANone { get; set; }
        */

        public string TypeOfSkill { get; set; }

        /*
            Officials Of Government, Executive, Managers, and Supervisors
            Professionals
            Technicians and Asociate Professionals
            Clerks
            Service Workers and Market Sales Workers
            Trades and Related Workers
            Plant, Machine Operators, and Assemblers
            Laborers
            Unskilled Workers
            Special Occupation 
        */

        public string EmploymentStatus { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string NatureOfEmployer { get; set; }
        public string TypeOfEmployment { get; set; }

        public string SSSNo { get; set; }
        public string GSISNo { get; set; }
        public string PhilhealthNo { get; set; }
        public string PhilHealthMembershipType { get; set; }
        public string YellowCardNo { get; set; }
        public string YellowCardMembershipType { get; set; }

        //[ForeignKey("CreatedBy")]
        //public string CreatedBy { get; set; }
        //[Display(Name = "Created By")]
        //public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Date Created")]
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
        [Display(Name = "Created By")]
        public virtual ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Date Created")]
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
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "Civil Status")]
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
        [Display(Name = "Verified By")]
        public ApplicationUser VerifiedBy { get; set; }

        public DateTime? InterviewDate { get; set; }

        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual Client Client { get; set; }
    }

    public class SoloParent
    {
        [Key]
        public int SoloParentId { get; set; }

        public string Status { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        [Display(Name = "Created By")]
        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("VerifiedBy")]
        public string VerifiedByUserId { get; set; }
        [Display(Name = "Verified By")]
        public ApplicationUser VerifiedBy { get; set; }

        public DateTime? InterviewDate { get; set; }

        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual Client Client { get; set; }
    }

    public class Pwd
    {
        [Key]
        public int PwdId { get; set; }

        public string Status { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedByUserId { get; set; }
        [Display(Name = "Created By")]
        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("VerifiedBy")]
        public string VerifiedByUserId { get; set; }
        [Display(Name = "Verified By")]
        public ApplicationUser VerifiedBy { get; set; }

        public bool? PsychosocialMentalDisability { get; set; }
        public bool? VisualDisability { get; set; }
        public bool? CommunicationDisability { get; set; }
        public bool? LearningDisability { get; set; }
        public bool? OrthopedicDisability { get; set; }
        public bool? IntellectualDisability { get; set; }

        public bool? Inborn { get; set; }
        public bool? Acquired { get; set; }
        public int? AgeAcquired { get; set; }

        public DateTime? InterviewDate { get; set; }

        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ReleaseDate { get; set; }

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
        public virtual List<RequirementComment> Comments { get; set; }
    }

    public class RequirementComment
    {
        public int RequirementCommentId { get; set; }

        public string Content { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public int RequirementId { get; set; }
        public virtual Requirement Requirement { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }

    public class RequirementAttachment
    {
        public int RequirementAttachmentId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        [Display(Name = "Date Created")]
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
        [Display(Name = "Is Disabled")]
        public bool IsDisabled { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Display(Name = "Date Created")]
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

    public class Inquiry
    {
        public int InquiryId { get; set; }
        public int ClientId { get; set; }
        public int? MessageId { get; set; }
        public string Status { get; set; }
        public string Resolution { get; set; }

        public DateTime DateCreated { get; set; }
        public virtual Client Client { get; set; }
        public virtual Message Message { get; set; }
    }

    public class InquiryNote
    {
        public int InquiryNoteId { get; set; }
        public int InquiryId { get; set; }

        public string UserName { get; set; }
        public string Content { get; set; }

        public virtual Inquiry Inquiry { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ClientEditModel
    {
        public ClientEditModel() {

        }

        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Sur Name")]
        public string SurName { get; set; }
        [Required]
        public string Gender { get; set; } // Male, Female
        [Required]
        [Display(Name = "Civil Status")]
        public string CivilStatus { get; set; } // Single, Married, Widowed, Divorced

        // Work
        public string Occupation { get; set; }
        public string Citizenship { get; set; }

        // Address
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        [Display(Name = "City Address")]
        public string CityAddress { get; set; }
        [Display(Name = "Provincial Address")]
        public string ProvincialAddress { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        // Residency
        [Display(Name = "Type Of Residency")]
        public string TypeOfResidency { get; set; } // House Owner, Sharer, Lessee/Tenant, Boarder
        [Display(Name = "Start Of Residency")]
        public DateTime? StartOfResidency { get; set; }

        // Age
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]
        [Display(Name = "Birth Place")]
        public string BirthPlace { get; set; }
        public string Religion { get; set; }

        // Marriage
        [Display(Name = "Date Of Marriage")]
        public DateTime? DateOfMarriage { get; set; }
        [Display(Name = "Place Of Marriage")]
        public string PlaceOfMarriage { get; set; }
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }
        [Display(Name = "Spouse Birth Date")]
        public DateTime? SpouseBirthDate { get; set; }
        [Display(Name = "Spouse Blu Card No")]
        public string SpouseBluCardNo { get; set; }

        // User who entered
        public string CreatedByUserId { get; set; }

        public List<ClientBeneficiary> ClientBeneficiaries { get; set; }
    }

    public class ClientPWDEditModel
    {
        public ClientPWDEditModel()
        {

        }

        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Sur Name")]
        public string SurName { get; set; }
        [Required]
        public string Gender { get; set; } // Male, Female
        [Required]
        [Display(Name = "Civil Status")]
        public string CivilStatus { get; set; } // Single, Married, Widowed, Divorced

        // Work
        public string Occupation { get; set; }
        public string Citizenship { get; set; }

        // Address
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        [Display(Name = "City Address")]
        public string CityAddress { get; set; }
        [Display(Name = "Provincial Address")]
        public string ProvincialAddress { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        // Residency
        [Display(Name = "Type Of Residency")]
        public string TypeOfResidency { get; set; } // House Owner, Sharer, Lessee/Tenant, Boarder
        [Display(Name = "Start Of Residency")]
        public DateTime? StartOfResidency { get; set; }

        // Age
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]
        [Display(Name = "Birth Place")]
        public string BirthPlace { get; set; }
        public string Religion { get; set; }

        // Marriage
        [Display(Name = "Date Of Marriage")]
        public DateTime? DateOfMarriage { get; set; }
        [Display(Name = "Place Of Marriage")]
        public string PlaceOfMarriage { get; set; }
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }
        [Display(Name = "Spouse Birth Date")]
        public DateTime? SpouseBirthDate { get; set; }
        [Display(Name = "Spouse Blu Card No")]
        public string SpouseBluCardNo { get; set; }

        public bool? PsychosocialMentalDisability { get; set; }
        public bool? VisualDisability { get; set; }
        public bool? CommunicationDisability { get; set; }
        public bool? LearningDisability { get; set; }
        public bool? OrthopedicDisability { get; set; }
        public bool? IntellectualDisability { get; set; }

        public bool? Inborn { get; set; }
        public bool? Acquired { get; set; }
        public int? AgeAcquired { get; set; }

        public string School { get; set; }
        public string EducationalAttainment { get; set; }

        /*
            public bool? EAElementary { get; set; }
            public bool? EAElementaryUndergraduate { get; set; }
            public bool? EAHighSchool { get; set; }
            public bool? EAHighSchoolUndergraduate { get; set; }
            public bool? EACollege { get; set; }
            public bool? EACollegeUndergraduate { get; set; }
            public bool? EAGraduate { get; set; }
            public bool? EASPED { get; set; }
            public bool? EAPostGraduate { get; set; }
            public bool? EAVocational { get; set; }
            public bool? EANone { get; set; }
        */

        public string TypeOfSkill { get; set; }

        /*
            Officials Of Government, Executive, Managers, and Supervisors
            Professionals
            Technicians and Asociate Professionals
            Clerks
            Service Workers and Market Sales Workers
            Trades and Related Workers
            Plant, Machine Operators, and Assemblers
            Laborers
            Unskilled Workers
            Special Occupation 
        */

        public string EmploymentStatus { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }

        public string NatureOfEmployer { get; set; }
        public string TypeOfEmployment { get; set; }

        public string SSSNo { get; set; }
        public string GSISNo { get; set; }
        public string PhilhealthNo { get; set; }
        public string PhilHealthMembershipType { get; set; }
        public string YellowCardNo { get; set; }
        public string YellowCardMembershipType { get; set; }

        // User who entered
        public string CreatedByUserId { get; set; }

        public List<ClientBeneficiary> ClientBeneficiaries { get; set; }
    }
}