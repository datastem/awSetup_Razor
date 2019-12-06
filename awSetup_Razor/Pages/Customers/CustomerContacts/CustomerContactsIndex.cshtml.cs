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

namespace awSetup_Razor.Pages.Customers.CustomerContacts
{
    public class CustomerContactsIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CustomerContactsIndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.CustomerContacts> CustomerContacts { get;set; }

        public async Task OnGetAsync(int id)
        {
            CustomerContacts = await _context.CustomerContacts.Where(cc => cc.CustomerId == id).ToListAsync();
        }

        public async Task<PartialViewResult> OnGetTableRefreshAsync(int id)
        {
            CustomerContacts = await _context.CustomerContacts.Where(cp => cp.CustomerId == id).ToListAsync();

            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Customers\CustomerContacts\CustomerContactsTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = CustomerContacts
                }
            };
            return pv;
        }

        public async Task<PartialViewResult> OnGetCustomerContactsCreateAsync(int id)
        {
            CustomerContactsEdit customerContact = new CustomerContactsEdit
            {
                contact = new Models.CustomerContacts { CustomerId = id },
                Action = "Create"
            };

            return await CustomerContactModalAsync(customerContact);
        }

        public async Task<PartialViewResult> OnGetCustomerContactsEditAsync(int id)
        {
            return await CustomerContactModalAsync(await GetCustomerContactsEditAsync(id,"Edit"));
        }

        public async Task<PartialViewResult> OnGetCustomerContactsDeleteAsync(int id)
        {
            return await CustomerContactModalAsync(await GetCustomerContactsEditAsync(id, "Delete"));
        }

        public async Task<PartialViewResult> OnPostCustomerContactsUpdateAsync(CustomerContactsEdit customerContact)
        {
            if (ModelState.IsValid)
            {
                switch (customerContact.Action)
                {
                    case "Create":
                        _context.CustomerContacts.Add(customerContact.contact);
                        break;
                    case "Edit":
                        _context.CustomerContacts.Update(customerContact.contact);
                        break;
                    case "Delete":
                        _context.CustomerContacts.Remove(customerContact.contact);
                        break;
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    CreateNotification(ex.Message);
                }
            }

            return await CustomerContactModalAsync(customerContact);
        }

        private async Task<CustomerContactsEdit> GetCustomerContactsEditAsync(int id, string action)
        {
            return new CustomerContactsEdit
            {
                contact = await _context.CustomerContacts.FirstOrDefaultAsync(cc => cc.CustomerContactId == id),
                Action = action
            };
        }

        private async Task<PartialViewResult> CustomerContactModalAsync(CustomerContactsEdit customerContact)
        {
            customerContact.AvailableContactTypesSL = await ContactTypesListAsync();

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerContacts\CustomerContactsModal",
                ViewData = new ViewDataDictionary<CustomerContactsEdit>(ViewData, customerContact)
            };
        }
        private async Task<List<SelectListItem>> ContactTypesListAsync()
        {
            return await (from c in _context.Codes
                    where c.Category == "ContactType" && c.IsActive == true
                    select new SelectListItem { Value = c.Code, Text = c.Label }).ToListAsync();
        }

        /*-------------------------------------------------------------
         * Notifcation methods
        * ------------------------------------------------------------*/
        private void CreateNotification(string message)
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as List<string> ?? new List<string>();
            notifications.Add(message);
            TempData["Notifications"] = notifications;
        }

        public PartialViewResult Notifications()
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as IEnumerable<string> ?? Enumerable.Empty<string>();
            
            var pv = new PartialViewResult
            {
                ViewName = @"_NotificationsPartial",
                ViewData = new ViewDataDictionary<IEnumerable<string>>(ViewData, notifications)
            };

            return pv;
        }
    }
}
