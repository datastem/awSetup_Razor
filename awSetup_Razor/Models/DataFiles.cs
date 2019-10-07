using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class DataFiles
    {
        [Column("DataFileID")]
        public int DataFileId { get; set; }
        [Required]
        [StringLength(50)]
        public string DataFileName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReceiveDateTime { get; set; }
        [Column("CustomerID")]
        public int? CustomerId { get; set; }
        [Column("MessageTypeID")]
        public int? MessageTypeId { get; set; }
        public int? RecordCount { get; set; }
    }
}
