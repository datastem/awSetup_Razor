using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;

namespace awSetup_Razor.Pages.Customers.CustomerRates
{
    public class CustomerRatesEditModel : PageModel
    {
        private readonly Models.ApplicationDbContext _context;

        public CustomerRatesEditModel(Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.CustomerRates CustomerRates { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerRates = await _context.CustomerRates.FirstOrDefaultAsync(m => m.CustomerId == id);

            if (CustomerRates == null)
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

            _context.Attach(CustomerRates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerRatesExists(CustomerRates.CustomerRateId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool CustomerRatesExists(int id)
        {
            return _context.CustomerRates.Any(e => e.CustomerRateId == id);
        }
    }
}
