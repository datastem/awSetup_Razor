using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class Scripts
    {
        public Scripts()
        {
            ScriptActions = new List<ScriptActions>();
            ScriptSchedules = new List<ScriptSchedules>();
            ScriptTags = new List<ScriptTags>();
        }

        [Key]
        [Column("ScriptID")]
        public int ScriptId { get; set; }

        [Column("MessageTypeID")]
        public int MessageTypeId { get; set; }

        [StringLength(1)]
        public string LanguageCode { get; set; }

        [StringLength(1)]
        public string DeliveryTypeCode { get; set; }

        public string MessageScript { get; set; }

        [StringLength(16)]
        public string MessagePrefix { get; set; }

        public int? CallAttempts { get; set; }
        public int? RequeueDelay { get; set; }

        [ForeignKey("MessageTypeId")]
        [InverseProperty("Scripts")]
        public virtual MessageTypes MessageType { get; set; }

        [InverseProperty("Script")]
        public virtual ICollection<ScriptActions> ScriptActions { get; set; }

        [InverseProperty("Script")]
        public virtual IList<ScriptSchedules> ScriptSchedules { get; set; }

        [InverseProperty("Script")]
        public virtual ICollection<ScriptTags> ScriptTags { get; set; }
    }
}