using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LiveChat.Models
{
    [Table("USERS")]
    public class User : IdentityUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        //[StringLength(20)]
        //public string Password { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
    }

}
