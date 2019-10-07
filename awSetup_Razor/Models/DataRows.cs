using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class DataRows
    {
        [Column("DataRowID")]
        public int DataRowId { get; set; }
        [Column("DataFileID")]
        public int DataFileId { get; set; }
        public int? LineNumber { get; set; }
        [Column("ScriptID")]
        public int? ScriptId { get; set; }
        [StringLength(1)]
        public string DeliveryTypeCode { get; set; }
        [StringLength(3)]
        public string LanguageCode { get; set; }
        [Column("PersonUID")]
        public int? PersonUid { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [Column("ExportID")]
        public int? ExportId { get; set; }
    }
}
