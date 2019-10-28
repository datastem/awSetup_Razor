using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class Settings
    {
        [Key]
        [Column("SettingID")]
        public int SettingId { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemValue { get; set; }

        [Column("UserID")]
        public int? UserId { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime ValidFrom { get;}

        [Column(TypeName = "datetime2(0)")]
        public DateTime ValidTo { get;}
    }
}
