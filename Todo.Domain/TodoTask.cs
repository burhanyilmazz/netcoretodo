using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Domain
{
    [Table("Tasks")]
    public class TodoTask : BaseEntity
    {
        public int TaskPriorityId { get; set; }
        public TaskPriority TaskPriority { get; set; }

        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; }

        public TaskStatus TaskStatus { get; set; }
        [Required(ErrorMessage = "Required")]
        [ForeignKey("TaskStatus")]
        public int TaskStatusId { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
    }
}
