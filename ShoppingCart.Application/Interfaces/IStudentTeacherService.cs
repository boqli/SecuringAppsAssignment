using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IStudentTeacherService
    {
        void AddMember(StudentTeacherViewModel m);

        IQueryable<StudentTeacherViewModel> GetStudents(string keyword);

        IQueryable<StudentTeacherViewModel> GetStudents();
    }
}
