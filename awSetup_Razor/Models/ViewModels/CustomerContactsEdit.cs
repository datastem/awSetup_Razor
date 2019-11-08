using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace awSetup_Razor.Models.ViewModels
{
    public class CustomerContactsEdit
    {
        public CustomerContacts contact { get; set; }
        public List<SelectListItem> AvailableContactTypesSL { get; set; }
        public string Action { get; set; }
    }
}
