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


        IQueryable<AssignmentViewModel> GetAssignments();

        IQueryable<AssignmentViewModel> GetAssignments(string owner);

        IQueryable<AssignmentViewModel> GetAssignments(Guid taskid);

        AssignmentViewModel GetAssignment(Guid id);
    }
}
