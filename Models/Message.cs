using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChat.Models
{
    [Table("MESSAGES")]
    public class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Text { get; set; }

        [StringLength(20)]
        public string User { get; set; }
        public string Receiver { get; set; }
    }
    public class MessageDTO
    {
        public string Text { get; set; }
        public string User { get; set; }
        public string To { get; set; }
        public string DateTime { get; set; }
        public string Receiver { get; set; }

    }
}
