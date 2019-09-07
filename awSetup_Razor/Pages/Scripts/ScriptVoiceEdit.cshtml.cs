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

namespace awSetup_Razor.Pages.Scripts
{
    public class EditModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public EditModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Scripts Scripts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Models.MessageTypes messagetype = _context.MessageTypes.FirstOrDefault(m => m.MessageTypeId == id);
            ViewData["messagetype"] = messagetype.MessageLabel;

            Scripts = await _context.Scripts.Where(s => s.MessageTypeId == id && s.DeliveryTypeCode == "V")
                .Include(s => s.ScriptActions)
                .Include(s => s.ScriptTags)
                .Include(s => s.ScriptSchedules)
                .FirstOrDefaultAsync();

            if (Scripts == null)
            {
                return NotFound();
            }
            ViewData["MessageTypeId"] = new SelectList(_context.MessageTypes, "MessageTypeId", "MessageTypeId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Scripts).State = EntityState.Modified;

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

            return RedirectToPage("./ScriptIndex");
        }

        public async Task<PartialViewResult> OnGetScriptActionsEdit(int? id)
        {
            ActionEdit action = new ActionEdit
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
                ViewData = new ViewDataDictionary<ActionEdit>(ViewData, action)
            };
        }

        public async Task<IActionResult> OnPostScriptActionsEdit(ActionEdit action)
        {
            if (ModelState.IsValid)
            {
                _context.ScriptActions.Update(action.Scriptaction);
                await _context.SaveChangesAsync();
            }

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActionsEditModalPartial",
                ViewData = new ViewDataDictionary<ActionEdit>(ViewData, action)
            };
        }

        public async Task<IActionResult> OnGetScriptActionsTableRefresh(int scriptid)
        {
            Scripts = await _context.Scripts.Where(s => s.ScriptId == scriptid && s.DeliveryTypeCode == "V")
                .Include(s => s.ScriptActions)
                .Include(s => s.ScriptTags)
                .Include(s => s.ScriptSchedules)
                .FirstOrDefaultAsync();
            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActionsTable",
                ViewData = new ViewDataDictionary<EditModel>(ViewData, Scripts)
            };
        }

        private bool ScriptsExists(int id)
        {
            return _context.Scripts.Any(e => e.ScriptId == id);
        }
    }
}