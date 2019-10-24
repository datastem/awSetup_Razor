using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class Customers
    {
        [Key]
        [Column("CustomerID")]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(10)]
        public string CustomerCode { get; set; }

        [Required]
        [StringLength(10)]
        public string PrimaryPhone { get; set; }

        public int TimeZoneOffset { get; set; }

        [Column("FTPFolderPath")]
        [StringLength(255)]
        public string FtpfolderPath { get; set; }

        [Column("FTPUserName")]
        [StringLength(24)]
        public string FtpuserName { get; set; }

        [Column("FTPPassword")]
        [StringLength(32)]
        public string Ftppassword { get; set; }

        [Column("SendGridAPIKey")]
        [StringLength(100)]
        public string SendGridApikey { get; set; }

        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Column("TwilioAccountSID")]
        [StringLength(100)]
        public string TwilioAccountSid { get; set; }

        [StringLength(100)]
        public string TwilioAuthToken { get; set; }

        public bool Active { get; set; }
    }
}