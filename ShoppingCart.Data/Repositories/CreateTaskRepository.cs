using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CreateTaskRepository : ICreateTaskRepository
    {
        ShoppingCartDbContext _context;
        public CreateTaskRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }

        public void AddTask(Task assigned)
        {
            _context.Task.Add(assigned);
            
            _context.SaveChanges();
            //return assigned.taskId;
        }

        public IQueryable<Task> GetTasks()
        {
            return _context.Task;
        }

        public Task GetTask(Guid id)
        {
            return _context.Task.SingleOrDefault(x => x.taskId == id);
        }


    }
}
