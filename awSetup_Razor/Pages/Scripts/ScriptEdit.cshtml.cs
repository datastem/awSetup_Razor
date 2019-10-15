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

            Scripts = await _context.Scripts.Where(s => s.MessageTypeId == id && s.DeliveryTypeCode == deliverycode && s.IsActive == true)
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

            var context = new ApplicationDbContext();
            Models.Scripts OriginalScript = context.Scripts.Where(s => s.ScriptId == Scripts.ScriptId && s.IsActive == true).FirstOrDefault();
            int newscriptid = -1;
            if (Scripts.MessageScript != OriginalScript.MessageScript)
            {
                OriginalScript.IsActive = false;
                OriginalScript.ValidTo = DateTime.Now;
                _context.Scripts.Update(OriginalScript);

                Models.Scripts modifiedscript = new Models.Scripts()
                {
                    CallAttempts = Scripts.CallAttempts,
                    DeliveryTypeCode = Scripts.DeliveryTypeCode,
                    IsActive = Scripts.IsActive,
                    MessageScript = Scripts.MessageScript,
                    MessageType = Scripts.MessageType,
                    MessageTypeId = Scripts.MessageTypeId,
                    RequeueDelay = Scripts.RequeueDelay,
                    ValidFrom = Scripts.ValidFrom,
                    ValidTo = Scripts.ValidTo
                };

                _context.Scripts.Add(modifiedscript);
                await _context.SaveChangesAsync();

                /*working to here*/

                newscriptid = modifiedscript.ScriptId;

                string[] newtags = Scripts.MessageScript.Split("[");
                HashSet<string> NewTagSet = new HashSet<string>();
                List<Models.ScriptTags> OriginalTags = _context.ScriptTags.Where(st => st.ScriptId == OriginalScript.ScriptId).ToList();
                List<Models.ScriptTags> ReplaceTags = new List<Models.ScriptTags>();  // diference in the tag
                foreach (var item in newtags)
                {
                    int p = item.IndexOf("]");
                    if (p > 0)
                    {
                        NewTagSet.Add(item.Substring(0, p));
                    }
                }
                foreach (var item in NewTagSet)
                {
                    Models.ScriptTags originaltag = OriginalTags.Where(ot => ot.TagName == item.ToString()).FirstOrDefault();

                    if (originaltag != null)
                    {
                        ReplaceTags.Add(new Models.ScriptTags
                        {
                            FormatCode = originaltag.FormatCode,
                            QueueMapCode = originaltag.QueueMapCode,
                            DataTypeCode = (item.ToUpper().Contains("DATE")? "D" : item.ToUpper().Contains("TIME") ? "T" : ""),  
                            ScriptId = newscriptid,
                            TagName = item
                        });
                    }
                    else
                    {
                        ReplaceTags.Add(new Models.ScriptTags
                        {
                            DataTypeCode = (item.ToUpper().Contains("DATE") ? "D" : item.ToUpper().Contains("TIME") ? "T" : ""),
                            ScriptId = newscriptid,
                            TagName = item
                        });

                    }
                }
                _context.ScriptTags.AddRange(ReplaceTags);
            }
            else
            {
                _context.Scripts.Update(Scripts);
            }

            await _context.SaveChangesAsync();
            if (newscriptid > 0)
            {
                HttpContext.Session.SetInt32("ScriptId", newscriptid);
            }
            return Page();
        }
    }
}