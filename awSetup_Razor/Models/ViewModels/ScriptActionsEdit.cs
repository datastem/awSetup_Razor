using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awSetup_Razor.Models.ViewModels
{
    public class ScriptActionsEdit
    {
        public int ScriptId { get; set; }
        public ScriptActions ScriptAction { get; set; }
        public List<SelectListItem> AvailableCodesSL { get; set; }
        public List<SelectListItem> AvailableResponsesSL { get; set; }
        public string Action { get; set; }
    }
}