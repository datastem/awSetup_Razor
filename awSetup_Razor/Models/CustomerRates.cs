using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class CustomerRates
    {
        [Column("CustomerRateID")]
        public int CustomerRateId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal EmailRate { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal TextRate { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal VoiceRate { get; set; }
    }
}
