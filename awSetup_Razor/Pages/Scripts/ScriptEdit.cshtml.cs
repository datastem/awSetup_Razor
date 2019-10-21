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

            _context.Scripts.Update(Scripts);

            string[] newtags = Scripts.MessageScript.Split("[");
            HashSet<string> NewTagSet = new HashSet<string>();
            List<Models.ScriptTags> OriginalTags = _context.ScriptTags.Where(st => st.ScriptId == Scripts.ScriptId).ToList();
            foreach (var item in newtags)
            {
                int p = item.IndexOf("]");
                if (p > 0)
                {
                    NewTagSet.Add(item.Substring(0, p));
                }
            }

            List<Models.ScriptTags> NewTags = new List<Models.ScriptTags>();
            foreach (var item in NewTagSet)
            {
                Models.ScriptTags originaltag = OriginalTags.Where(ot => ot.TagName == item.ToString()).FirstOrDefault();

                if (originaltag == null)   //new tag
                {
                    NewTags.Add(new Models.ScriptTags
                    {
                        ScriptId = Scripts.ScriptId,
                        TagName = item,
                        DataTypeCode = (item.ToUpper().Contains("DATE") ? "D" : item.ToUpper().Contains("TIME") ? "T" : "")
                    });
                }
            }
            _context.ScriptTags.AddRange(NewTags);

            List<Models.ScriptTags> RemoveTags = new List<Models.ScriptTags>();
            foreach (var item in OriginalTags)
            {
                if (!NewTagSet.Contains(item.TagName))
                {
                    RemoveTags.Add(item);
                }
            }
            _context.ScriptTags.RemoveRange(RemoveTags);

            await _context.SaveChangesAsync();

            return Page();
        }
    }
}