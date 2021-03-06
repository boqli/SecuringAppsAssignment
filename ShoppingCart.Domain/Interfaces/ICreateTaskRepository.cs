using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICreateTaskRepository
    {
        void AddTask(Task assigned);

        IQueryable<Task> GetTasks();

        Task GetTask(Guid id);
    }
}
