using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using awSetup_Razor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScriptActionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ScriptActionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Models.ScriptActions> ScriptActions { get; set; }
        public async Task<IActionResult> OnGetAsync(int scriptid)
        {
            //int? scriptId = HttpContext.Session.GetInt32("ScriptId");

            ScriptActions = await _context.ScriptActions.Where(sa => sa.ScriptId == scriptid).ToListAsync();
            return Page();
        }

        public PartialViewResult OnGetScriptActionsCreate(int scriptid)
        {
            ActionEdit action = new ActionEdit();
            action.ScriptId = scriptid;

            action.Scriptaction = new Models.ScriptActions { ScriptId = scriptid };

            action = CreateSelectLists(action);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsCreateModalPartial",
                ViewData = new ViewDataDictionary<ActionEdit>(ViewData, action)
            };
        }
        public async Task<PartialViewResult> OnGetScriptActionsEdit(int? id)
        {
            ActionEdit action = new ActionEdit();
            action.Scriptaction = await _context.ScriptActions.Where(s => s.ScriptActionId == id).FirstOrDefaultAsync();

            action = EditSelectLists(action);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsEditModalPartial",
                ViewData = new ViewDataDictionary<ActionEdit>(ViewData, action)
            };
        }

        public async Task<IActionResult> OnPostScriptActionsCreate(ActionEdit action)
        {
            if (ModelState.IsValid)
            {
                _context.ScriptActions.Add(action.Scriptaction);
                await _context.SaveChangesAsync();
            }

            action = CreateSelectLists(action);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsCreateModalPartial",
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

            action = EditSelectLists(action);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsEditModalPartial",
                ViewData = new ViewDataDictionary<ActionEdit>(ViewData, action)
            };
        }

        public async Task<PartialViewResult> OnGetScriptActionsDelete(int? id)
        {
            Models.ScriptActions scriptaction = await _context.ScriptActions.Where(s => s.ScriptActionId == id).FirstOrDefaultAsync();

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsDeleteModalPartial",
                ViewData = new ViewDataDictionary<Models.ScriptActions>(ViewData, scriptaction)
            };
        }

        public async Task<PartialViewResult> OnPostScriptActionsDelete(int id)
        {
            Models.ScriptActions scriptaction = await _context.ScriptActions.Where(s => s.ScriptActionId == id).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                _context.ScriptActions.Remove(scriptaction);
                await _context.SaveChangesAsync();
            }

            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsDeleteModalPartial",
                ViewData = new ViewDataDictionary<Models.ScriptActions>(ViewData, scriptaction)
            };
            return pv;
        }

        public async Task<PartialViewResult> OnGetScriptActionsTableRefresh(int scriptid)
        {
            int? scriptId = HttpContext.Session.GetInt32("ScriptId");

            ScriptActions = await _context.ScriptActions.Where(sa => sa.ScriptId == scriptId).ToListAsync();

            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsTablePartial",
                //ViewData = new ViewDataDictionary<ScriptActions>(ViewData, Scripts.ScriptActions)
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ScriptActions
                }
            };
            return pv;
        }

        private ActionEdit CreateSelectLists(ActionEdit action)
        {
            action.AvailablecodesSL = (from c in _context.Codes
                                       where c.Category == "VoiceAction"
                                       && (!_context.ScriptActions.Any(sa => c.Code == sa.ActionCode && sa.ScriptId == action.ScriptId))
                                       select new SelectListItem { Value = c.Code, Text = c.Label }).ToList();

            action.AvailableresponsesSL = (from c in _context.Codes
                                           where c.Category == "Keypad"
                                           && (!_context.ScriptActions.Any(sa => c.Code == sa.Response && sa.ScriptId == action.ScriptId))
                                           select new SelectListItem { Value = c.Code, Text = c.Label }).ToList();

            return action;
        }

        private ActionEdit EditSelectLists(ActionEdit action)
        {
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

            return action;
        }

    }
}