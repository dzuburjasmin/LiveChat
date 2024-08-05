using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChat
{
    [Table("MESSAGES")]
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Text { get; set; }

        [StringLength(20)]
        public string User { get; set; }


    }
}
