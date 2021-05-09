using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IAssignmentService
    {

        void AddAssignment(AssignmentViewModel assignment);


        AssignmentViewModel GetTask(Guid id);

        IQueryable<AssignmentViewModel> GetAssignments();
    }
}
