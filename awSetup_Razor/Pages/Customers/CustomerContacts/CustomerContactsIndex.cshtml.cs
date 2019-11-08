using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using awSetup_Razor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace awSetup_Razor.Pages.Customers.CustomerContacts
{
    public class CustomerContactsIndexModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public CustomerContactsIndexModel(awSetup_Razor.Models.ApplicationDbContext context)
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
                AvailableContactTypesSL = await ContactTypesListAsync(),
                Action = "Create"
            };

            return CustomerContactModal(customerContact);
        }

        public async Task<PartialViewResult> OnGetCustomerContactsEditAsync(int id)
        {
            return CustomerContactModal(await GetCustomerContactsEditAsync(id,"Edit"));
        }

        public async Task<PartialViewResult> OnGetCustomerContactsDeleteAsync(int id)
        {
            return CustomerContactModal(await GetCustomerContactsEditAsync(id, "Delete"));
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
                await _context.SaveChangesAsync();
            }

            return CustomerContactModal(customerContact);
        }

        private async Task<CustomerContactsEdit> GetCustomerContactsEditAsync(int id, string action)
        {
            return new CustomerContactsEdit
            {
                contact = await _context.CustomerContacts.FirstOrDefaultAsync(cc => cc.CustomerContactId == id),
                AvailableContactTypesSL = await ContactTypesListAsync(),
                Action = action
            };
        }

        private PartialViewResult CustomerContactModal(CustomerContactsEdit customerContact)
        {
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
    }
}
