using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;

namespace awSetup_Razor.Pages.MessageTypes
{
    public class EditModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public EditModel(awSetup_Razor.Models.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MessageTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageTypesExists(MessageTypes.MessageTypeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MessageTypesExists(int id)
        {
            return _context.MessageTypes.Any(e => e.MessageTypeId == id);
        }
    }
}