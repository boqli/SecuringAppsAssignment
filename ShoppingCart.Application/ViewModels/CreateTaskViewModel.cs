using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CreateTaskViewModel
    {
 
        public Guid taskId { get; set; }
        public string taskTitle { get; set; }
        public string taskDescription { get; set; }
        public DateTime taskDeadline { get; set; }

        public string teacherEmail { get; set; }
        public string studentEmail { get; set; }

    }
}
