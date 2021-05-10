using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICommentRepository
    {

        IQueryable<Comment> GetComments();

        IQueryable<Comment> GetComments(Guid id);

        Guid AddComment(Comment c);
    }
}
