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
        public IQueryable<Assignment> GetAssignments(Guid taskid)
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            //single or default will return ONE product! or null
            return _context.Assignment.Where(x => x.TaskId == taskid);
        }


        public IQueryable<Assignment> GetAssignments(string owner)
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            //single or default will return ONE product! or null
            return _context.Assignment.Where(x => x.Owner == owner);
        }

        public Assignment GetAssignment(Guid id)
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            //single or default will return ONE product! or null
            return _context.Assignment.SingleOrDefault(x => x.Id == id);
        }


    }
}
