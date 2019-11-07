using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class MasterRates
    {
        [Key]
        [Column("MasterRateID")]
        public int MasterRateId { get; set; }
        
        [Column("IsMember")]
        public bool IsMember { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal EmailRate { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal TextRate { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal VoiceRate { get; set; }
    }
}
