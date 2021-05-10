using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICommentService
    {

        IQueryable<CommentViewModel> GetComments();


        IQueryable<CommentViewModel> GetComments(Guid AssignmentId);

        void AddComment(CommentViewModel comment);

    }
}
