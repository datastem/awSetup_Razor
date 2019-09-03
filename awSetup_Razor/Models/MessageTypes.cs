using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awSetup_Razor.Models
{
    public partial class MessageTypes
    {
        public MessageTypes()
        {
            Scripts = new HashSet<Scripts>();
        }

        [Key]
        [Column("MessageTypeID")]
        public int MessageTypeId { get; set; }

        [Column("CustomerID")]
        public int CustomerId { get; set; }

        [Column("CustomerPhoneID")]
        public int? CustomerPhoneId { get; set; }

        [StringLength(50)]
        public string MessageLabel { get; set; }

        [Column("useEmail")]
        public bool UseEmail { get; set; }

        [Column("useText")]
        public bool UseText { get; set; }

        [Column("useVoice")]
        public bool UseVoice { get; set; }

        [StringLength(10)]
        public string FilenameCode { get; set; }

        public bool Active { get; set; }

        [StringLength(50)]
        public string DeliveryTypeTag { get; set; }

        [StringLength(50)]
        public string LanguageTag { get; set; }

        public int? ReQueueDelay { get; set; }
        public int? CallAttempts { get; set; }

        [InverseProperty("MessageType")]
        public virtual ICollection<Scripts> Scripts { get; set; }
    }
}