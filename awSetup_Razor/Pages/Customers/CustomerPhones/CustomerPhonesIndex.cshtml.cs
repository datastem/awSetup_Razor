using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using awSetup_Razor.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using awSetup_Razor.Models.ViewModels;
using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account.AvailablePhoneNumberCountry;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace awSetup_Razor.Pages.Customers.CustomerPhones
{
    public class CustomerPhonesIndexModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public CustomerPhonesIndexModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.CustomerPhones> CustomerPhones { get; set; }

        public async Task OnGetAsync(int id)
        {
            CustomerPhones = await _context.CustomerPhones.Where(cp => cp.CustomerId == id).ToListAsync();
        }

        public PartialViewResult OnGetCustomerPhonesCreate(int id)
        {
            Models.Customers customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            TwilioClient.Init(customer.TwilioAccountSid, customer.TwilioAuthToken);
            //            ResourceSet<LocalResource> localAvailableNumbers = LocalResource.Read("US", nearLatLong: "29.568842, -97.964729", distance: 50);
            ResourceSet<LocalResource> localAvailableNumbers = LocalResource.Read("US", nearNumber: customer.PrimaryPhone, distance: 10);

            TwilioPhoneEdit phone = new TwilioPhoneEdit
            {
                CustomerPhone = new Models.CustomerPhones { CustomerId = id },
                AvailableNumbersSL = (from c in localAvailableNumbers
                                      select new SelectListItem { Value = c.PhoneNumber.ToString().Substring(2), Text = c.FriendlyName.ToString() }).ToList()
            };

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesCreateModalPartial",
                ViewData = new ViewDataDictionary<TwilioPhoneEdit>(ViewData, phone)
            };
        }
        public PartialViewResult OnGetCustomerPhonesEdit(int id)
        {
            Models.CustomerPhones customerPhone = _context.CustomerPhones.FirstOrDefault(cp => cp.CustomerPhoneId == id);

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesEditModalPartial",
                ViewData = new ViewDataDictionary<Models.CustomerPhones>(ViewData, customerPhone)
            };
        }

    }
}
