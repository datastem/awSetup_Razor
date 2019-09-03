using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class ScriptSchedules
    {
        [Column("ScriptScheduleID")]
        public int ScriptScheduleId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("ScriptID")]
        public int ScriptId { get; set; }
        [Column("DOW")]
        public int Dow { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndTime { get; set; }

        [ForeignKey("ScriptId")]
        [InverseProperty("ScriptSchedules")]
        public virtual Scripts Script { get; set; }
    }
}
