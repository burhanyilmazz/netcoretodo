using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Domain.ViewModels
{
    public class TaskViewModel
    {
        public List<TaskPriority> TaskPriorities { get; set; }
        public List<TaskStatus> TaskStatuses { get; set; }
        public List<TodoTask> Tasks { get; set; }
    }
}
