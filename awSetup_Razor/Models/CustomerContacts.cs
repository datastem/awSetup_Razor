using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class CustomerContacts
    {
        [Key]
        [Column("CustomerContactID")]
        public int CustomerContactId { get; set; }

        [Required]
        [Column("CustomerID")]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(20)]
        public string ContactTypeCode { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactName { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(5)]
        public string Zip { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime ValidFrom { get; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime ValidTo { get; }


        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerContacts")]
        public virtual Customers Customer { get; set; }
    }
}
