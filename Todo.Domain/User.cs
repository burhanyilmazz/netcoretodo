using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Domain
{
    [Table("Users")]
    public class User : BaseEntity, ICreatedDateInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
    }
}
