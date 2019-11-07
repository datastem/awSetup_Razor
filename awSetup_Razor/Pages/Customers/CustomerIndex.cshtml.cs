using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace awSetup_Razor.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly awSetup_Razor.Models.ApplicationDbContext _context;

        public IndexModel(awSetup_Razor.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Customers> Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _context.Customers.ToListAsync();
        }

        public async Task<PartialViewResult> OnGetTableRefresh()
        {
            Customers = await _context.Customers.ToListAsync();

            PartialViewResult pv = new PartialViewResult
            {
                ViewName = @".\Customers\CustomersTablePartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = Customers
                }
            };
            return pv;
        }

        public PartialViewResult OnGetCustomerCreate()
        {
            Models.Customers customer = new Models.Customers()
            {
                Action = "Create",
                IsMember = false,
                Active = true
            };

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomersModal",
                ViewData = new ViewDataDictionary<Models.Customers>(ViewData, customer)
            };

            //TODO:  Create CustomerRate record
        }

        public IActionResult OnGetCustomerEdit(int id)
        {
            HttpContext.Session.SetInt32("CustomerId", id);
            return RedirectToPage("/Customers/CustomerEdit", new { id });
        }

        public async Task<PartialViewResult> OnGetCustomerDelete(int id)
        {
            Models.Customers customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            customer.Action = "Delete";

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomersModal",
                ViewData = new ViewDataDictionary<Models.Customers>(ViewData, customer)
            };

            //TODO:  Create CustomerRate record
        }

        public PartialViewResult OnPostCustomersUpdate(Models.Customers customer)
        {
            if (ModelState.IsValid)
            {
                switch (customer.Action)
                {
                    case "Create":
                        string ftppath = _context.Settings.Where(s => s.ItemName == "FTPFolderPath").Select(s => s.ItemValue).Single();

                        customer.FTPFolderPath = ftppath + customer.CustomerCode;
                        customer.FTPUserName = customer.CustomerCode;
                        customer.FTPPassword = GenerateRandomPassword();

                        //TODO: Create Twilio Subaccount
                        //TODO: Get SendGridAPIKey and populate, generate e-mail address
                        

                        _context.Customers.Add(customer);
                        _context.SaveChanges();

                        Models.MasterRates masterrate = _context.MasterRates.FirstOrDefault(mr => mr.IsMember == customer.IsMember);
                        Models.CustomerRates rate = new Models.CustomerRates
                        {
                            CustomerId = customer.CustomerId,
                            EmailRate = masterrate.EmailRate,
                            TextRate = masterrate.TextRate,
                            VoiceRate = masterrate.VoiceRate
                        };
                        _context.CustomerRates.Add(rate);


                        break;
                    case "Edit":
                        //Not used here
                        break;
                    case "Delete":
                        _context.Customers.Remove(customer);
                        //TODO: Close Twilio Subaccount
                        //TODO: Cancel Twilio Phone
                        break;
                }
                _context.SaveChanges();
            }

            return new PartialViewResult
            {
                ViewName = @".\Customers\CustomersModal",
                ViewData = new ViewDataDictionary<Models.Customers>(ViewData, customer)
            };
        }




        public async Task<IActionResult> OnGetCreateFTPUser(int id)
        {
            /*https://kimsereyblog.blogspot.com/2018/01/start-processes-from-c-in-dotnet-core.html
             * https://stackoverflow.com/questions/43515360/net-core-process-start-leaving-defunct-child-process-behind
             */

            Models.Customers customer = await _context.Customers.FindAsync(id);

            List<Models.Settings> settings = await _context.Settings.ToListAsync();

            string coreftpexec = settings.Find(s => s.ItemName == "CoreFTPServerExec").ItemValue;
            string coreftpdomain = settings.Find(s => s.ItemName == "CoreFTPDomain").ItemValue;
            string ftppath = settings.Find(s => s.ItemName == "FTPFolderPath").ItemValue;

            if (string.IsNullOrWhiteSpace(customer.FTPFolderPath))
                customer.FTPFolderPath = ftppath + customer.CustomerCode;

            if (string.IsNullOrWhiteSpace(customer.FTPUserName))
                customer.FTPUserName = customer.CustomerCode;

            if (string.IsNullOrWhiteSpace(customer.FTPPassword))
                customer.FTPPassword = GenerateRandomPassword();

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = coreftpexec,
                WorkingDirectory = Path.GetDirectoryName(coreftpexec),
                Arguments = $"-adduser {coreftpdomain} {customer.FTPUserName} {customer.FTPPassword} {customer.FTPFolderPath}",
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = false,
                Domain = "tejashma",
                UserName = "strapp",
                PasswordInClearText = "Brushy42514!",
                Verb = "runas"
            };


            using (Process process = new Process() { StartInfo = processStartInfo, EnableRaisingEvents = true })
            {
                process.Start();
            }

            return RedirectToPage("/Customers/CustomerIndex");
        }     


        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        /// https://www.ryadel.com/en/c-sharp-random-password-generator-asp-net-core-mvc/
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 12,
                RequiredUniqueChars = 8,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?#_-~"                      // non-alphanumeric
            };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

    }
}