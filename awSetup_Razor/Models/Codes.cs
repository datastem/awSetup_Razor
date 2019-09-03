using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class Codes
    {
        [Key]
        [Column("CodeID")]
        public int CodeId { get; set; }
        [Required]
        [StringLength(20)]
        public string Category { get; set; }
        [Required]
        [StringLength(30)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Label { get; set; }
        [StringLength(100)]
        public string ToolTipText { get; set; }
        [Column("ParentCodeID")]
        public int? ParentCodeId { get; set; }
        [StringLength(5)]
        public string MicrosoftCode { get; set; }
        [StringLength(5)]
        public string TwilioCode { get; set; }
        public bool IsActive { get; set; }
        public byte SortWeight { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(50)]
        public string ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool IsVoid { get; set; }
    }
}
