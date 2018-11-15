using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Domain
{
    [Table("TaskPriorities")]
    public class TaskPriority : BaseEntity
    {
        public string Name { get; set; }

        public string BgColor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
    }
}
