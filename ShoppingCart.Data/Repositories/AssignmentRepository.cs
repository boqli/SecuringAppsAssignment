using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {

        ShoppingCartDbContext _context;
        public AssignmentRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }

        public Guid AddAssignment(Assignment a)
        {
            _context.Assignment.Add(a);
            _context.SaveChanges();
            return a.Id;
        }

        public Task GetTask(Guid id)
        {
            return _context.Task.SingleOrDefault(x => x.taskId == id);
        }


        public IQueryable<Assignment> GetAssignments()
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            return _context.Assignment;
        }

    }
}
