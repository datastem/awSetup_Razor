﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awSetup_Razor.Models.ViewModels
{
    public class ScriptTagsEdit
    {
        public ScriptTags ScriptTag { get; set; }
        public List<SelectListItem> DataTypeSL { get; set; }
        public List<SelectListItem> DateFormatSL { get; set; }
        public List<SelectListItem> TimeFormatSL { get; set; }
        public List<SelectListItem> QueueMapSL { get; set; }
        public string Action { get; set; }
    }
}
