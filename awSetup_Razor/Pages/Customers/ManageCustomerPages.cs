using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace awSetup_Razor.Pages.Customers
{
    public static class ManageCustomerPages
    {
        public static string CustomerEdit => "CustomerEdit";

        public static string CustomerContacts => "CustomerContacts";

        public static string CustomerOptions => "CustomerOptions";

        public static string CustomerPhones => "CustomerPhones";

        public static string CustomerRates => "CustomerRates";

        public static string CustomerEditNavClass(ViewContext viewContext) => PageNavClass(viewContext, CustomerEdit);

        public static string CustomerContactsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CustomerContacts);

        public static string CustomerOptionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CustomerOptions);

        public static string CustomerPhonesNavClass(ViewContext viewContext) => PageNavClass(viewContext, CustomerPhones);

        public static string CustomerRatesNavClass(ViewContext viewContext) => PageNavClass(viewContext, CustomerRates);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}