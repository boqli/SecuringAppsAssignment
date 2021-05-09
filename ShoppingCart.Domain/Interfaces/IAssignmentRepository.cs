using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IAssignmentRepository
    {

        Guid AddAssignment(Assignment a);


        IQueryable<Assignment> GetAssignments();

        Task GetTask(Guid id);
    }
}
