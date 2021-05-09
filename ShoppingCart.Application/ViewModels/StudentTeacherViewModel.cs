using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class StudentTeacherViewModel
    {
        public Guid Id { get; set; }

        [EmailAddress]
        public string teacherEmail { get; set; }

        [EmailAddress]
        public string studentEmail { get; set; }

    }
}
