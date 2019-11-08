using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class CustomerOptions
    {
        [Key]
        [Column("CustomerOptionID")]
        public int CustomerOptionId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(30)]
        public string OptionTypeCode { get; set; }
        [Required]
        [StringLength(30)]
        public string OptionNameCode { get; set; }
        [Required]
        [StringLength(30)]
        public string OptionValueCode { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerOptions")]
        public virtual Customers Customer { get; set; }
    }
}
