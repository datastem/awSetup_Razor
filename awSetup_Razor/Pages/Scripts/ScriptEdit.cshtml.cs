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
    public class ScriptEditModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public ScriptEditModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int id, string deliverycode)
        {

            Models.MessageTypes messagetype = _context.MessageTypes.FirstOrDefault(m => m.MessageTypeId == id);
            ViewData["messagetype"] = messagetype.MessageLabel;

            Scripts = await _context.Scripts.Where(s => s.MessageTypeId == id && s.DeliveryTypeCode == deliverycode)
                .FirstOrDefaultAsync();

            Scripts.OriginalScript = Scripts.MessageScript;

            //            Scripts.ScriptSchedules = Scripts.ScriptSchedules.OrderBy(s => s.Dow).ToList();

            //TODO
            //if (Scripts == null)
            //{
            //    Models.Scripts scripts = new Models.Scripts
            //    {
            //        MessageTypeId = id,
            //        DeliveryTypeCode = deliverycode
            //    };
            //    _context.Scripts.Add(Scripts);
            //    await _context.SaveChangesAsync();
            //    Scripts = await _context.Scripts.Where(s => s.MessageTypeId == id && s.DeliveryTypeCode == deliverycode)
            //        .FirstOrDefaultAsync();
            //}

            /*
            HttpContext.Session.SetInt32("ScriptId", Scripts.ScriptId);
            HttpContext.Session.SetInt32("CustomerID", messagetype.CustomerId);
            HttpContext.Session.SetInt32("MessageTypeId", messagetype.MessageTypeId);
            HttpContext.Session.SetString("DeliveryTypeCode", deliverycode);
            */
            return Page();
        }

        [BindProperty]
        public Models.Scripts Scripts { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Scripts.MessageScript != Scripts.OriginalScript)
            {
                _context.Scripts.Attach(Scripts);
                string[] newtags = Scripts.MessageScript.Split("[");
                HashSet<string> NewTagSet = new HashSet<string>();
                List<Models.ScriptTags> OriginalTags = _context.ScriptTags.Where(st => st.ScriptId == Scripts.ScriptId).ToList();
                List<Models.ScriptTags> ReplaceTags = new List<Models.ScriptTags>();
                foreach (var item in newtags)
                {
                    int p = item.IndexOf("]");
                    if (p > 0)
                    {
                        NewTagSet.Add(item.Substring(0, p));
                    }
                }
                foreach( var item in NewTagSet)
                {
                    if (OriginalTags.FindIndex(ot => ot.TagName == item.ToString()) >=0)
                    {

                        var t = "stop";

                    }
                }
            }



            if (_context.Entry(Scripts).State == EntityState.Modified)
            {
                var t = "Modified";
            }

            await _context.SaveChangesAsync();

            return Page();
        }
    }
}