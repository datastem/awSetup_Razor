using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class ScriptTags
    {
        [Key]
        [Column("ScriptTagID")]
        public int ScriptTagId { get; set; }
        [Column("ScriptID")]
        public int ScriptId { get; set; }
        [Required]
        [StringLength(50)]
        public string TagName { get; set; }
        [StringLength(20)]
        public string FormatString { get; set; }
        [StringLength(25)]
        public string QueueMapColumn { get; set; }
        public bool Active { get; set; }
        [StringLength(1)]
        public string DeliveryTypeCode { get; set; }
        [StringLength(1)]
        public string DataTypeCode { get; set; }

        [ForeignKey("ScriptId")]
        [InverseProperty("ScriptTags")]
        public virtual Scripts Script { get; set; }
    }
}
