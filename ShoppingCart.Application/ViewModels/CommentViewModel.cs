using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        public string CommentText { get; set; }


        public string CommenterId { get; set; }


        public Guid AssignmentID { get; set; }
    }
}
