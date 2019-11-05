using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using awSetup_Razor.Models.ViewModels;
using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account.AvailablePhoneNumberCountry;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CustomerPhones = await _context.CustomerPhones.Where(cp => cp.CustomerId == id).ToListAsync();
            return Page();
        }

        public PartialViewResult OnGetTableRefresh(int id)
        {
            CustomerPhones = _context.CustomerPhones.Where(cp => cp.CustomerId == id).ToList();
            
            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = CustomerPhones
                }
            };
            return pv;
        }

        public PartialViewResult OnGetCustomerPhonesCreate(int id)
        {
            Models.Customers customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            List<SelectListItem> phonelist = TwilioPhoneList(id, customer.PrimaryPhone, 10);

            TwilioPhoneEdit phone = new TwilioPhoneEdit
            {
                CustomerPhone = new Models.CustomerPhones { CustomerId = id },
                AvailableNumbersSL = phonelist,
                PrimaryPhone = customer.PrimaryPhone,
                Action = "Create"
            };

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesModalPartial",
                ViewData = new ViewDataDictionary<TwilioPhoneEdit>(ViewData, phone)
            };
        }

        public PartialViewResult OnGetCustomerPhonesEdit(int id)
        {
            TwilioPhoneEdit phone = new TwilioPhoneEdit
            {
                CustomerPhone = _context.CustomerPhones.FirstOrDefault(cp => cp.CustomerPhoneId == id),
                Action = "Edit"
            };

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesModalPartial",
                ViewData = new ViewDataDictionary<TwilioPhoneEdit>(ViewData, phone)
            };
        }

        public PartialViewResult OnGetCustomerPhonesDelete(int id)
        {
            TwilioPhoneEdit phone = new TwilioPhoneEdit
            {
                CustomerPhone = _context.CustomerPhones.FirstOrDefault(cp => cp.CustomerPhoneId == id),
                Action = "Delete"
            };

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesModalPartial",
                ViewData = new ViewDataDictionary<TwilioPhoneEdit>(ViewData, phone)
            };
        }

        public PartialViewResult OnPostCustomerPhonesUpdate(TwilioPhoneEdit phone)
        {
            if (ModelState.IsValid)
            {
                switch (phone.Action)
                {
                    case "Create":
                        _context.CustomerPhones.Add(phone.CustomerPhone);
                        //TODO:  Reserve Twilio Number
                        //TODO:  Configure Inbound webhooks
                        break;
                    case "Edit":
                        _context.CustomerPhones.Update(phone.CustomerPhone);
                        break;
                    case "Delete":
                        _context.CustomerPhones.Remove(phone.CustomerPhone);
                        //TODO:  Cancel Twilio Phone
                        break;
                }
                _context.SaveChanges();
            }

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesModalPartial",
                ViewData = new ViewDataDictionary<TwilioPhoneEdit>(ViewData, phone)
            };
        }

        /*
         * Refresh the Twilio Available numbers (Create only)
         */
        public JsonResult OnGetTwilioPhoneListRefresh(int customerid, string phone, int miles)
        {
            return new JsonResult(TwilioPhoneList(customerid, phone, miles));
        }
        private List<SelectListItem> TwilioPhoneList(int customerid, string phone, int miles)
        {
            Models.Customers customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerid);

            phone = (string.IsNullOrEmpty(phone) ? "+1" + customer.PrimaryPhone : "+1" + phone);

            TwilioClient.Init(customer.TwilioAccountSid, customer.TwilioAuthToken);
            ResourceSet<LocalResource> localAvailableNumbers = LocalResource.Read("US", nearNumber: phone, distance: miles);

            return (from c in localAvailableNumbers
                    select new SelectListItem { Value = c.PhoneNumber.ToString().Substring(2), Text = c.FriendlyName.ToString() + " " + c.Locality }).ToList();
        }
    }
}
