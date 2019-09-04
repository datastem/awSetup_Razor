using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using awSetup_Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScriptActionsEditModalModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public ScriptActionsEditModalModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.ScriptActions ScriptActions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ScriptActions = await _context.ScriptActions.Where(s => s.ScriptActionId == id).FirstOrDefaultAsync();
            //ViewData.Keypad =
            return Page();
        }
    }
}