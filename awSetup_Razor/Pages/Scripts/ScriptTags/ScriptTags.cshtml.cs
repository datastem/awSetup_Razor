using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using awSetup_Razor.Models;
using awSetup_Razor.Models.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace awSetup_Razor.Pages.Scripts
{
    public class ScriptTagsModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public ScriptTagsModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public IList<Models.ScriptTags> ScriptTags { get;set; }

        public async Task OnGetAsync(int scriptid)
        { 
            ScriptTags = await _context.ScriptTags.Where(st => st.ScriptId == scriptid ).OrderBy(st => st.TagName).ToListAsync();
        }

        public async Task<PartialViewResult> OnGetScriptTagsEdit(int? id)
        {
            TagEdit tag = new TagEdit();
            tag.ScriptTag = await _context.ScriptTags.Where(s => s.ScriptTagId == id).FirstOrDefaultAsync();

            tag = PopulateSelectLists(tag);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptTags\ScriptTagsEditModalPartial",
                ViewData = new ViewDataDictionary<TagEdit>(ViewData, tag)
            };
        }

        public async Task<PartialViewResult> OnPostScriptTagsEdit(TagEdit tag)
        {
            if (ModelState.IsValid)
            {
                _context.ScriptTags.Update(tag.ScriptTag);
                await _context.SaveChangesAsync();
            }

            tag = PopulateSelectLists(tag);

            return new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptTags\ScriptTagsEditModalPartial",
                ViewData = new ViewDataDictionary<TagEdit>(ViewData, tag)
            };
        }

        public async Task<PartialViewResult> OnGetScriptTagsTableRefresh(int scriptid)
        {
            ScriptTags = await _context.ScriptTags.Where(st => st.ScriptId == scriptid).OrderBy(st => st.TagName).ToListAsync();

            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Scripts\ScriptTags\ScriptTagsTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ScriptTags
                }
            };
            return pv;
        }

        private TagEdit PopulateSelectLists(TagEdit tag)
        {
            tag.QueueMapSL = (from c in _context.Codes
                              where c.Category == "QueueMap"
                              && (!_context.ScriptTags.Any(st => c.Code == st.QueueMapCode && st.ScriptId == tag.ScriptTag.ScriptId))
                              select new SelectListItem { Value = c.Code, Text = c.Label }).ToList();

            tag.DataTypeSL = (from c in _context.Codes
                              where c.Category == "DataType"
                              select new SelectListItem { Value = c.Code, Text = c.Label }).ToList();

            tag.DateFormatSL = (from c in _context.Codes
                                where c.Category == "DateFormat" 
                                select new SelectListItem { Value = c.Code, Text = DateTime.Now.ToString(c.Code) }).ToList();

            tag.TimeFormatSL = (from c in _context.Codes
                                where c.Category == "TimeFormat"
                                select new SelectListItem { Value = c.Code, Text = c.Code == "h:mm xx" ? DateTime.Now.ToString("h:mm") + " in the morning" : DateTime.Now.ToString(c.Code) }).ToList();

            return tag;
        }

    }
}
