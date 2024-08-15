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
        [StringLength(20)]
        public string? Receiver { get; set; }
        public DateTime DateTime { get; set; }
        [StringLength(20)]
        public string ChatName { get; set; }

    }
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Receiver { get; set; }
        public DateTime DateTime { get; set; }

    }
}
