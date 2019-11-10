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
    public class ScriptActionsIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ScriptActionsIndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IList<Models.ScriptActions> ScriptActions { get; set; }

        public async Task OnGetAsync(int id)
        {
            ScriptActions = await _context.ScriptActions.Where(sa => sa.ScriptId == id).ToListAsync();
        }

        public async Task<PartialViewResult> OnGetTableRefresh(int id)
        {
            int? scriptId = HttpContext.Session.GetInt32("ScriptId");

            ScriptActions = await _context.ScriptActions.Where(sa => sa.ScriptId == id).ToListAsync();

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ScriptActions
                }
            };
        }

        public async Task<PartialViewResult> OnGetScriptActionsCreate(int id)
        {
            ScriptActionsEdit action = new ScriptActionsEdit
            {
                ScriptAction = new Models.ScriptActions { ScriptId = id },
                Action = "Create"
            };

            return await GetScriptActionsModalAsync(action);
        }

        public async Task<PartialViewResult> OnGetScriptActionsEdit(int id)
        {
            ScriptActionsEdit action = new ScriptActionsEdit
            {
                ScriptAction = await _context.ScriptActions.FirstOrDefaultAsync(sa => sa.ScriptActionId == id),
                Action = "Edit"
            };

            return await GetScriptActionsModalAsync(action);
        }

        public async Task<PartialViewResult> OnGetScriptActionsDelete(int id)
        {
            ScriptActionsEdit action = new ScriptActionsEdit
            {
                ScriptAction = await _context.ScriptActions.FirstOrDefaultAsync(sa => sa.ScriptActionId == id),
                Action = "Delete"
            };

            return await GetScriptActionsModalAsync(action);
        }

        public async Task<IActionResult> OnPostScriptActionsUpdateAsync(ScriptActionsEdit action)
        {
            if (ModelState.IsValid)
            {
                switch (action.Action)
                {
                    case "Create":
                        _context.ScriptActions.Add(action.ScriptAction);
                        break;
                    case "Edit":
                        _context.ScriptActions.Update(action.ScriptAction);
                        break;
                    case "Delete":
                        _context.ScriptActions.Remove(action.ScriptAction);
                        break;
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //CreateNotification(ex.Message);
                }
            }

            return await GetScriptActionsModalAsync(action);
        }

        private async Task<ScriptActionsEdit> GetSelectionListsAsync(ScriptActionsEdit action)
        {
            switch (HttpContext.Session.GetString("DeliveryTypeCode"))
            {
                case "V":
                    action.AvailableCodesSL = await (from c in _context.Codes
                                               where c.Category == "VoiceAction"
                                               && (
                                                     (!_context.ScriptActions.Any(sa => c.Code == sa.ActionCode && sa.ScriptId == action.ScriptAction.ScriptId))
                                                      || c.Code == action.ScriptAction.ActionCode  // get current selection
                                                      )
                                               select new SelectListItem { Value = c.Code, Text = c.Label }).ToListAsync();

                    action.AvailableResponsesSL = await (from c in _context.Codes
                                                   where c.Category == "Keypad"
                                                   && ((!_context.ScriptActions.Any(sa => c.Code == sa.Response && sa.ScriptId == action.ScriptAction.ScriptId))
                                                   || c.Code == action.ScriptAction.Response)   // get current selection
                                                   select new SelectListItem { Value = c.Code, Text = c.Label }).ToListAsync();
                    break;
                case "T":
                    action.AvailableCodesSL = await (from c in _context.Codes
                                               where c.Category == "SmsAction"
                                               && (
                                                     (!_context.ScriptActions.Any(sa => c.Code == sa.ActionCode && sa.ScriptId == action.ScriptAction.ScriptId))
                                                      || c.Code == action.ScriptAction.ActionCode  // get current selection
                                                      )
                                               select new SelectListItem { Value = c.Code, Text = c.Label }).ToListAsync();
                    break;
            }

            return action;
        }

        private async Task<PartialViewResult> GetScriptActionsModalAsync(ScriptActionsEdit action)
        {
            action = await GetSelectionListsAsync(action);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptActions\ScriptActionsModal",
                ViewData = new ViewDataDictionary<ScriptActionsEdit>(ViewData, action)
            };

        }
    }
}