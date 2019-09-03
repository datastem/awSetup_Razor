using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public class ScriptActions
    {
        [Key]
        [Column("ScriptActionID")]
        public int ScriptActionId { get; set; }

        [Column("ScriptID")]
        public int ScriptId { get; set; }

        [StringLength(160)]
        public string Response { get; set; }

        [Required]
        [StringLength(30)]
        public string ActionCode { get; set; }

        [StringLength(10)]
        public string Dial { get; set; }

        [StringLength(50)]
        public string DialTag { get; set; }

        [StringLength(160)]
        public string ReplyText { get; set; }

        [StringLength(50)]
        public string StoredProcedure { get; set; }

        [ForeignKey("ScriptId")]
        [InverseProperty("ScriptActions")]
        public virtual Scripts Script { get; set; }
    }
}