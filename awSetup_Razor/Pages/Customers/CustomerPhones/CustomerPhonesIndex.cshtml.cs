using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using awSetup_Razor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account.AvailablePhoneNumberCountry;

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

        public async Task<PartialViewResult> OnGetTableRefreshAsync(int id)
        {
            CustomerPhones = await _context.CustomerPhones.Where(cp => cp.CustomerId == id).ToListAsync();

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = CustomerPhones
                }
            };
        }

        public async Task<PartialViewResult> OnGetCustomerPhonesCreateAsync(int id)
        {
            Models.Customers customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            CustomerPhonesEdit phone = new CustomerPhonesEdit
            {
                CustomerPhone = new Models.CustomerPhones { CustomerId = id },
                AvailableNumbersSL = TwilioPhoneList(id, customer.PrimaryPhone, 10),
                PrimaryPhone = customer.PrimaryPhone,
                Action = "Create"
            };

            return CustomerPhonesModal(phone);
        }

        public async Task<PartialViewResult> OnGetCustomerPhonesEditAsync(int id)
        {
            return CustomerPhonesModal(await GetCustomerPhonesEditAsync(id,"Edit"));
        }

        public async Task<PartialViewResult> OnGetCustomerPhonesDeleteAsync(int id)
        {
            return CustomerPhonesModal(await GetCustomerPhonesEditAsync(id, "Delete"));
        }

        public async Task<PartialViewResult> OnPostCustomerPhonesUpdateAsync(CustomerPhonesEdit phone)
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
                await _context.SaveChangesAsync();
            }

            return CustomerPhonesModal(phone);
        }

        private async Task<CustomerPhonesEdit> GetCustomerPhonesEditAsync(int id, string action)
        {
            return new CustomerPhonesEdit
            {
                CustomerPhone = await _context.CustomerPhones.FirstOrDefaultAsync(cc => cc.CustomerPhoneId == id),
                Action = action
            };
        }

        private PartialViewResult CustomerPhonesModal(CustomerPhonesEdit customerPhone)
        {
            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomerPhones\CustomerPhonesModal",
                ViewData = new ViewDataDictionary<CustomerPhonesEdit>(ViewData, customerPhone)
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
