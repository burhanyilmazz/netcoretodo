using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Domain
{
    [Table("TaskStatuses")]
    public class TaskStatus : BaseEntity
    {
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public int TaskCount { get; set; }
    }
}
