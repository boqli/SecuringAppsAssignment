using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class AssignmentViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } //optional
        public string Description { get; set; } //optional
        public string Path { get; set; }
        public string Signature { get; set; }
        public string Owner { get; set; } //student email

        public Guid TaskId { get; set; }

    }
}
