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

        public OptionListsHelper()
        {
            GenderList.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            GenderList.Add(new SelectListItem() { Text = "Female", Value = "Female" });

            CivilStatusList.Add(new SelectListItem() { Text = "Single", Value = "Single" });
            CivilStatusList.Add(new SelectListItem() { Text = "Married", Value = "Married" });
            CivilStatusList.Add(new SelectListItem() { Text = "Widowed", Value = "Widowed" });
            CivilStatusList.Add(new SelectListItem() { Text = "Devorced", Value = "Devorced" });

            ResidencyList.Add(new SelectListItem() { Text = "House Owner", Value = "House Owner" });
            ResidencyList.Add(new SelectListItem() { Text = "Sharer", Value = "Sharer" });
            ResidencyList.Add(new SelectListItem() { Text = "Tenant", Value = "Tenant" });
        }
    }
    
}