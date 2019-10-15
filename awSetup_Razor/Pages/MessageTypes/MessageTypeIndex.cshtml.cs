using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;
using Microsoft.AspNetCore.Http;

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

        public async Task<IActionResult> OnGetLauncherAsync(int id, string deliverycode)
        {
            Models.MessageTypes messagetype = _context.MessageTypes.FirstOrDefault(m => m.MessageTypeId == id);
            ViewData["messagetype"] = messagetype.MessageLabel;

            Models.Scripts scr = await _context.Scripts.Where(s => s.MessageTypeId == id && s.DeliveryTypeCode == deliverycode && s.IsActive == true)
                .FirstOrDefaultAsync();

            HttpContext.Session.SetInt32("ScriptId", scr.ScriptId);
            HttpContext.Session.SetInt32("CustomerID", messagetype.CustomerId);
            HttpContext.Session.SetInt32("MessageTypeId", messagetype.MessageTypeId);
            HttpContext.Session.SetString("DeliveryTypeCode", deliverycode);

            return RedirectToPage("/Scripts/ScriptEdit", new { id = id, deliverycode = deliverycode });
        }
    }
}