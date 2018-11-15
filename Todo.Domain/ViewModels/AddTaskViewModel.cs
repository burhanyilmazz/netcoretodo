 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.UI.ViewModels
{
    public class AddTaskViewModel
    {
        public int? TaskId { get; set; }
        public int? TaskStatusId { get; set; }
        public string Name { get; set; }
        public int? TaskPriorityId { get; set; }
    }
}
