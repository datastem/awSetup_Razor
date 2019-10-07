using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class DataValues
    {
        [Column("DataValueID")]
        public int DataValueId { get; set; }
        [Column("DataFileID")]
        public int? DataFileId { get; set; }
        [Column("DataRowID")]
        public int? DataRowId { get; set; }
        [StringLength(50)]
        public string ColumnName { get; set; }
        [StringLength(255)]
        public string ColumnValue { get; set; }
    }
}
