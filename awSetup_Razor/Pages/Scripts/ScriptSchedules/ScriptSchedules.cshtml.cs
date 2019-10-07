using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using awSetup_Razor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScriptSchedulesModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public ScriptSchedulesModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int scriptid)
        {
            ScriptSchedules = await _context.ScriptSchedules.Where(sch => sch.ScriptId == scriptid).OrderBy(s => s.Dow).ToListAsync();

            if (ScriptSchedules == null)
            {
                return NotFound();
            }
            return Page();
        }

        [BindProperty]
        public List<Models.ScriptSchedules> ScriptSchedules { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            foreach(var item in ScriptSchedules)
            {
                _context.ScriptSchedules.Update(item);
            }
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}