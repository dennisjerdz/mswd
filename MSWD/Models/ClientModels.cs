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
        [Key]
        public int ClientId { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("SeniorCitizen")]
        public int SeniorCitizenId { get; set; }
        public virtual SeniorCitizen SeniorCitizen { get; set; }

        [ForeignKey("Pwd")]
        public int PwdId { get; set; }
        public virtual Pwd Pwd { get; set; }

        [ForeignKey("SoloParent")]
        public int SoloParentId { get; set; }
        public virtual SoloParent SoloParent { get; set; }

        public virtual List<Requirement> Requirements { get; set; }
    }

    public class SeniorCitizen
    {
        [Key]
        public int SeniorCitizenId { get; set; }

        public virtual Client Client { get; set; }
    }

    public class SoloParent
    {
        [Key]
        public int SoloParentId { get; set; }

        public virtual Client Client { get; set; }
    }

    public class Pwd
    {
        [Key]
        public int PwdId { get; set; }

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
}