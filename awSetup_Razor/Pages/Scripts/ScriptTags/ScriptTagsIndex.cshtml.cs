using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using awSetup_Razor.Models;
using awSetup_Razor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScriptTagsIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ScriptTagsIndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.ScriptTags> ScriptTags { get; set; }

        public async Task OnGetAsync(int id)
        {
            ScriptTags = await _context.ScriptTags.Where(st => st.ScriptId == id).OrderBy(st => st.TagName).ToListAsync();
        }

        public async Task<PartialViewResult> OnGetTableRefresh(int scriptid)
        {
            ScriptTags = await _context.ScriptTags.Where(st => st.ScriptId == scriptid).OrderBy(st => st.TagName).ToListAsync();

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptTags\ScriptTagsTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ScriptTags
                }
            };
        }

        public async Task<PartialViewResult> OnGetScriptTagsEditAsync(int id)
        {
            ScriptTagsEdit tag = new ScriptTagsEdit
            {
                ScriptTag = await _context.ScriptTags.Where(s => s.ScriptTagId == id).FirstOrDefaultAsync(),
                Action = "Edit"
            };

            return await ScriptTagsModalAsync(tag);
        }

        public async Task<PartialViewResult> OnPostScriptTagsUpdateAsync(ScriptTagsEdit tag)
        {
            if (ModelState.IsValid)
            {
                switch (tag.Action)
                {
                    case "Create":
                    case "Delete":
                        //Tags are created an deleted when the script is modified. They are based upon the tags used the message script.
                        break;
                    case "Edit":
                        _context.ScriptTags.Update(tag.ScriptTag);
                        await _context.SaveChangesAsync();
                        break;
                }
            }

            tag = await PopulateSelectListsAsync(tag);

            return await ScriptTagsModalAsync(tag);
        }

        private async Task<ScriptTagsEdit> PopulateSelectListsAsync(ScriptTagsEdit tag)
        {
            DateTime now = DateTime.Now;

            tag.QueueMapSL = await(from c in _context.Codes
                              where c.Category == "QueueMap"
                              && (!_context.ScriptTags.Any(st => c.Code == st.QueueMapCode && st.ScriptId == tag.ScriptTag.ScriptId))
                              select new SelectListItem { Value = c.Code, Text = c.Label }).ToListAsync();

            tag.DataTypeSL = await(from c in _context.Codes
                              where c.Category == "DataType"
                              select new SelectListItem { Value = c.Code, Text = c.Label }).ToListAsync();

            tag.DateFormatSL = await(from c in _context.Codes
                                where c.Category == "DateFormat"
                                select new SelectListItem { Value = c.Code, Text = now.ToString(c.Code) }).ToListAsync();

            tag.TimeFormatSL = await (from c in _context.Codes
                                where c.Category == "TimeFormat"
                                select new SelectListItem { Value = c.Code, Text = c.Code == "h:mm xx" ? now.ToString("h:mm") + " in the " + (now.Hour < 12 ? "morning": "afternoon") : now.ToString(c.Code) }).ToListAsync();

            return tag;
        }

        private async Task<PartialViewResult> ScriptTagsModalAsync(ScriptTagsEdit tag)
        {
            tag = await PopulateSelectListsAsync(tag);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptTags\ScriptTagsModal",
                ViewData = new ViewDataDictionary<ScriptTagsEdit>(ViewData, tag)
            };
        }

        /*
         * This method is called via ajax form the modal
         */
        public async Task<JsonResult> OnGetFormatCodesAsync(string datatypecode)
        {
            DateTime now = DateTime.Now;

            var codes = new List<SelectListItem>();
            switch (datatypecode)
            {
                case "D":
                    codes = await (from c in _context.Codes
                                   where c.Category == "DateFormat"
                                   select new SelectListItem { Value = c.Code, Text = now.ToString(c.Code) }).ToListAsync();
                    break;
                case "T":
                    codes = await (from c in _context.Codes
                                   where c.Category == "TimeFormat"
                                   select new SelectListItem { Value = c.Code, Text = c.Code == "h:mm xx" ? now.ToString("h:mm") + " in the " + (now.Hour < 12 ? "morning" : "afternoon") : now.ToString(c.Code) }).ToListAsync();
                    break;

            }
            return new JsonResult(codes);
        }

    }
}
