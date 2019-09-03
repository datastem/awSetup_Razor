using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;

namespace awSetup_Razor.Pages.Scripts
{
    public class DeleteModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public DeleteModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Scripts Scripts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Scripts = await _context.Scripts
                .Include(s => s.MessageType).FirstOrDefaultAsync(m => m.ScriptId == id);

            if (Scripts == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Scripts = await _context.Scripts.FindAsync(id);

            if (Scripts != null)
            {
                _context.Scripts.Remove(Scripts);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}