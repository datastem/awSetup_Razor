using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace awSetup_Razor.Models.ViewModels
{
    public class TwilioPhoneEdit
    {
        public CustomerPhones CustomerPhone { get; set; }
        public List<SelectListItem> AvailableNumbersSL { get; set; }
        public string PrimaryPhone { get; set; }
        public int Miles { get; set; }
        public string Action { get; set; }
    }
}
