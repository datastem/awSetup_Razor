using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;
using awSetup_Razor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScriptVoiceEditModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public ScriptVoiceEditModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Scripts Scripts { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            Models.MessageTypes messagetype = _context.MessageTypes.FirstOrDefault(m => m.MessageTypeId == id);
            ViewData["messagetype"] = messagetype.MessageLabel;

            Scripts = await _context.Scripts.Where(s => s.MessageTypeId == id && s.DeliveryTypeCode == "V")
                .Include(s => s.ScriptActions)
                .Include(s => s.ScriptTags)
                .Include(s => s.ScriptSchedules)
                .FirstOrDefaultAsync();

            Scripts.ScriptSchedules = Scripts.ScriptSchedules.OrderBy(s => s.Dow).ToList();

            if (Scripts == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetInt32("CustomerID", messagetype.CustomerId);

            ViewData["MessageTypeId"] = new SelectList(_context.MessageTypes, "MessageTypeId", "MessageTypeId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Scripts/ScriptVoiceEdit", new { id = Scripts.MessageTypeId });
                //return Page();
            }

            _context.Attach(Scripts).State = EntityState.Modified;
            foreach (var item in Scripts.ScriptSchedules)
            {
                _context.Attach(item).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScriptsExists(Scripts.ScriptId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            int Customerid = HttpContext.Session.GetInt32("CustomerID").Value;

            return RedirectToPage("/MessageTypes/MessageTypeIndex", new {customerid = Customerid });
        }

        public async Task<PartialViewResult> OnGetScriptActionsEdit(int? id)
        {
            ScriptActionsEdit action = new ScriptActionsEdit
            {
                Scriptaction = await _context.ScriptActions.Where(s => s.ScriptActionId == id).FirstOrDefaultAsync()
            };

            action.AvailablecodesSL = (from c in _context.Codes
                                       where c.Category == "VoiceAction"
                                       && (
                                             (!_context.ScriptActions.Any(sa => c.Code == sa.ActionCode && sa.ScriptId == action.Scriptaction.ScriptId))
                                              || c.Code == action.Scriptaction.ActionCode  // get current selection
                                              )
                                       select new SelectListItem { Value = c.Code, Text = c.Label }).ToList();

            action.AvailableresponsesSL = (from c in _context.Codes
                                           where c.Category == "Keypad"
                                           && ((!_context.ScriptActions.Any(sa => c.Code == sa.Response && sa.ScriptId == action.Scriptaction.ScriptId))
                                           || c.Code == action.Scriptaction.Response)   // get current selection
                                           select new SelectListItem { Value = c.Code, Text = c.Label }).ToList();

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActionsEditModalPartial",
                ViewData = new ViewDataDictionary<ScriptActionsEdit>(ViewData, action)
            };
        }

        public async Task<IActionResult> OnPostScriptActionsEdit(ScriptActionsEdit action)
        {
            if (ModelState.IsValid)
            {
                _context.ScriptActions.Update(action.Scriptaction);
                await _context.SaveChangesAsync();
            }

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActionsEditModalPartial",
                ViewData = new ViewDataDictionary<ScriptActionsEdit>(ViewData, action)
            };
        }

        public async Task<PartialViewResult> OnGetScriptActionsTableRefresh(int scriptid)
        {
            Scripts = await _context.Scripts.Where(s => s.ScriptId == scriptid && s.DeliveryTypeCode == "V")
                .Include(s => s.ScriptActions)
                .Include(s => s.ScriptTags)
                .Include(s => s.ScriptSchedules)
                .FirstOrDefaultAsync();

            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActionsTablePartial",
                //ViewData = new ViewDataDictionary<ScriptActions>(ViewData, Scripts.ScriptActions)
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = Scripts.ScriptActions
                }
            };
            return pv;
        }



        private bool ScriptsExists(int id)
        {
            return _context.Scripts.Any(e => e.ScriptId == id);
        }
    }
}