using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{

    public partial class ScriptSchedules : IValidatableObject
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

        public bool IsActive { get; set; }
            

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
        /*
           https://dontpaniclabs.com/blog/post/2017/10/25/validating-multiple-fields-in-asp-net-mvc/
       */

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsActive)
            {
                if (!StartTime.HasValue)
                {
                    yield return new ValidationResult("Start Time is required.", new List<string> { "StartTime" });
                }
                if (!EndTime.HasValue)
                {
                    yield return new ValidationResult("End Time is required.", new List<string> { "EndTime" });
                }
                if (DateTime.Compare(Convert.ToDateTime(StartTime), Convert.ToDateTime(EndTime)) >= 0)
                {
                    yield return new ValidationResult("Start Time must be less than End Time", new List<string> { "EndTime" });
                }
            }
        }
    }
}