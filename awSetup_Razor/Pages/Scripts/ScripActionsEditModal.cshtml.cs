using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScripActionsEditModalModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public EditModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }
    }
}