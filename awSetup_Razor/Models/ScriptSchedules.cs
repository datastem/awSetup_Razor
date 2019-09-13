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

        [NotMapped]
        public bool DowSwitch { get; set; }

        [NotMapped]
        public string DowLabel
        {
            get
            {
                string label = "";
                switch (Dow)
                {
                    case 1: label = "Sunday"; break;
                    case 2: label = "Monday"; break;
                    case 3: label = "Tuesday"; break;
                    case 4: label = "Wednesday"; break;
                    case 5: label = "Thursday"; break;
                    case 6: label = "Friday"; break;
                    case 7: label = "Saturday"; break;
                };
                return label;
            }
        }
    }
}