using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICreateTaskService
    {

        void AddTask(CreateTaskViewModel task);

        IQueryable<CreateTaskViewModel> GetTasks();

        CreateTaskViewModel GetTask(Guid id);
    }
}
