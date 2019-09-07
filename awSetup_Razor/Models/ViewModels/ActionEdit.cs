using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awSetup_Razor.Models.ViewModels
{
    public class ActionEdit
    {
        public ScriptActions Scriptaction { get; set; }
        public List<SelectListItem> AvailablecodesSL { get; set; }
        public List<SelectListItem> AvailableresponsesSL { get; set; }
    }
}