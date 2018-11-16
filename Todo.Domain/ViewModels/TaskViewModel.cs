using System.Collections.Generic;

namespace Todo.Domain.ViewModels
{
    public class TaskViewModel
    {
        public List<TaskPriority> TaskPriorities { get; set; }
        public List<TaskStatus> TaskStatuses { get; set; }
        public List<TodoTask> Tasks { get; set; }
    }
}
