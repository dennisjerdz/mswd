using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSWD
{
    public class OptionListsHelper
    {
        public List<SelectListItem> GenderList = new List<SelectListItem>();
        public List<SelectListItem> CivilStatusList = new List<SelectListItem>();
        public List<SelectListItem> ResidencyList = new List<SelectListItem>();
        public List<SelectListItem> EAList = new List<SelectListItem>();
        public List<SelectListItem> ESList = new List<SelectListItem>();
        public List<SelectListItem> NEList = new List<SelectListItem>();
        public List<SelectListItem> TEList = new List<SelectListItem>();
        public List<SelectListItem> TOSList = new List<SelectListItem>();
        public List<SelectListItem> MType = new List<SelectListItem>();

        public OptionListsHelper()
        {
            GenderList.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            GenderList.Add(new SelectListItem() { Text = "Female", Value = "Female" });

            CivilStatusList.Add(new SelectListItem() { Text = "Single", Value = "Single" });
            CivilStatusList.Add(new SelectListItem() { Text = "Married", Value = "Married" });
            CivilStatusList.Add(new SelectListItem() { Text = "Widowed", Value = "Widowed" });
            CivilStatusList.Add(new SelectListItem() { Text = "Divorced", Value = "Divorced" });

            ResidencyList.Add(new SelectListItem() { Text = "House Owner", Value = "House Owner" });
            ResidencyList.Add(new SelectListItem() { Text = "Sharer", Value = "Sharer" });
            ResidencyList.Add(new SelectListItem() { Text = "Tenant", Value = "Tenant" });

            EAList.Add(new SelectListItem() { Text = "Elementary", Value = "Elementary" });
            EAList.Add(new SelectListItem() { Text = "ElementaryUndergraduate", Value = "ElementaryUndergraduate" });
            EAList.Add(new SelectListItem() { Text = "HighSchool", Value = "HighSchool" });
            EAList.Add(new SelectListItem() { Text = "HighSchoolUndergraduate", Value = "HighSchoolUndergraduate" });
            EAList.Add(new SelectListItem() { Text = "College", Value = "College" });
            EAList.Add(new SelectListItem() { Text = "CollegeUndergraduate", Value = "CollegeUndergraduate" });
            EAList.Add(new SelectListItem() { Text = "Graduate", Value = "Graduate" });
            EAList.Add(new SelectListItem() { Text = "SPED", Value = "SPED" });
            EAList.Add(new SelectListItem() { Text = "PostGraduate", Value = "PostGraduate" });
            EAList.Add(new SelectListItem() { Text = "Vocational", Value = "Vocational" });
            EAList.Add(new SelectListItem() { Text = "None", Value = "None" });

            ESList.Add(new SelectListItem() { Text = "N/A", Value = "N/A" });
            ESList.Add(new SelectListItem() { Text = "Employed", Value = "Employed" });
            ESList.Add(new SelectListItem() { Text = "Resigned", Value = "Resigned" });
            ESList.Add(new SelectListItem() { Text = "Unemployed", Value = "Unemployed" });
            ESList.Add(new SelectListItem() { Text = "Dispaced Worker", Value = "Dispaced Worker" });
            ESList.Add(new SelectListItem() { Text = "Returning OFW", Value = "Returning OFW" });

            MType.Add(new SelectListItem() { Text = "Member", Value = "Member" });
            MType.Add(new SelectListItem() { Text = "Dependent", Value = "Dependent" });

            NEList.Add(new SelectListItem() { Text = "N/A", Value = "N/A" });
            NEList.Add(new SelectListItem() { Text = "Private", Value = "Private" });
            NEList.Add(new SelectListItem() { Text = "Government", Value = "Government" });

            TEList.Add(new SelectListItem() { Text = "N/A", Value = "N/A" });
            TEList.Add(new SelectListItem() { Text = "Contractual", Value = "Contractual" });
            TEList.Add(new SelectListItem() { Text = "Permanent", Value = "Permanent" });
            TEList.Add(new SelectListItem() { Text = "Self-Employed", Value = "Self-Employed" });
            TEList.Add(new SelectListItem() { Text = "Seasonal", Value = "Seasonal" });

            TOSList.Add(new SelectListItem() { Text = "N/A", Value = "N/A" });
            TOSList.Add(new SelectListItem() { Text = "Officials, Executives, Managers, Supervisors", Value = "Contractual" });
            TOSList.Add(new SelectListItem() { Text = "Professionals", Value = "Permanent" });
            TOSList.Add(new SelectListItem() { Text = "Technicians, Associate Professionals", Value = "Technicians, Associate Professionals" });
            TOSList.Add(new SelectListItem() { Text = "Clerks", Value = "Clerks" });
            TOSList.Add(new SelectListItem() { Text = "Service Worker, Shop/Market Worker", Value = "Service Worker, Shop/Market Worker" });
            TOSList.Add(new SelectListItem() { Text = "Trades and Related Worker", Value = "Trades and Related Worker" });
            TOSList.Add(new SelectListItem() { Text = "Plant, Machine Operators and Assemblers", Value = "Plant, Machine Operators and Assemblers" });
            TOSList.Add(new SelectListItem() { Text = "Laborers", Value = "Laborers" });
            TOSList.Add(new SelectListItem() { Text = "Unskilled Workers", Value = "Unskilled Workers" });
            TOSList.Add(new SelectListItem() { Text = "Special Occupation", Value = "Special Occupation" });
        }
    }
    
}