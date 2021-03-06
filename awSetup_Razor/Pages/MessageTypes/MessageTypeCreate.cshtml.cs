﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using awSetup_Razor.Models;

namespace awSetup_Razor.Pages.MessageTypes
{
    public class CreateModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public CreateModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.MessageTypes MessageTypes { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MessageTypes.Add(MessageTypes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./MessageTypeIndex");
        }
    }
}