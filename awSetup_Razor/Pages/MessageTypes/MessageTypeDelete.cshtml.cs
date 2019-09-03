using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;

namespace awSetup_Razor.Pages.MessageTypes
{
    public class DeleteModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public DeleteModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.MessageTypes MessageTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MessageTypes = await _context.MessageTypes.FirstOrDefaultAsync(m => m.MessageTypeId == id);

            if (MessageTypes == null)
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

            MessageTypes = await _context.MessageTypes.FindAsync(id);

            if (MessageTypes != null)
            {
                _context.MessageTypes.Remove(MessageTypes);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./MessageTypeIndex");
        }
    }
}