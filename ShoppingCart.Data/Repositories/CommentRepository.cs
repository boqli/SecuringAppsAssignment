using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        ShoppingCartDbContext _context;
        public CommentRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public Guid AddComment(Comment c)
        {
            _context.Comment.Add(c);
            _context.SaveChanges();
            return c.Id;
        }

        public IQueryable<Comment> GetComments()
        {
            return _context.Comment;
        }

        public IQueryable<Comment> GetComments(Guid assignId)
        {
            return _context.Comment.Where(x => x.AssignmentID == assignId);
        }

    }
}
