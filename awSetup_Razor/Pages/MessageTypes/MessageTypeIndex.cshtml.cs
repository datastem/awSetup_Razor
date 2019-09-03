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
    public class IndexModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public IndexModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.MessageTypes> MessageTypes { get; set; }

        public async Task OnGetAsync(int customerid)
        {
            MessageTypes = await _context.MessageTypes.Where(m => m.CustomerId == customerid).ToListAsync();
        }
    }
}