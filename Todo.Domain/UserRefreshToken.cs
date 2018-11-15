using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Domain
{
    [Table("UserRefreshTokens")]
    public class UserRefreshToken : BaseEntity, ICreatedDateInfo
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Required")]

        public DateTime IssuedUtc { get; set; }

        [Required(ErrorMessage = "Required")]

        public DateTime ExpiresUtc { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Required")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
