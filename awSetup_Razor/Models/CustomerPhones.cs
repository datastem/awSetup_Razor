using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class CustomerPhones
    {
        [Key]
        [Column("CustomerPhoneID")]
        public int CustomerPhoneId { get; set; }
        
        [Column("CustomerID")]
        public int CustomerId { get; set; }
       
        [Required]
        [StringLength(10)]
        public string TwilioPhoneNumber { get; set; }
        
        [StringLength(10)]
        public string ForwardNumber { get; set; }
        
        [StringLength(500)]
        public string UnhandledMessage { get; set; }
    }
}
