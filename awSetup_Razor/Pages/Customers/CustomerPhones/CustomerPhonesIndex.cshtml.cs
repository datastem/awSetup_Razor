using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;

namespace awSetup_Razor.Pages.Customers.CustomerPhones
{
    public class CustomerPhonesIndexModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public CustomerPhonesIndexModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.CustomerPhones> CustomerPhones { get;set; }

        public async Task OnGetAsync()
        {
            CustomerPhones = await _context.CustomerPhones.ToListAsync();
        }
    }
}
