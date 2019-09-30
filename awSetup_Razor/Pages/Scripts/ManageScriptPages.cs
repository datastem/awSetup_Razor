using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace awSetup_Razor.Pages.Scripts
{
    public static class ManageScriptPages
    {
        public static string ScriptEdit => "ScriptEdit";

        public static string ScriptActions => "ScriptActions";

        public static string ScriptSchedules => "ScriptSchedules";

        public static string ScriptTags => "ScriptTags";

        public static string ScriptEditNavClass(ViewContext viewContext) => PageNavClass(viewContext, ScriptEdit);

        public static string ScriptActionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ScriptActions);

        public static string ScriptSchedulesNavClass(ViewContext viewContext) => PageNavClass(viewContext, ScriptSchedules);

        public static string ScriptTagsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ScriptTags);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}