using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid taskId { get; set; }

        [Required]
        public string taskTitle { get; set; }

        [Required]
        public string taskDescription { get; set; }

        [Required]
        public DateTime taskDeadline { get; set; }

        [Required]
        public string TeacherEmail { get; set; }

        [Required]
        public string StudentEmail { get; set; }
    }
}
