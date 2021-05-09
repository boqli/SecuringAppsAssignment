using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IStudentTeacherRepository
    {
        void AddMember(StudentTeacher m);

        StudentTeacher GetStudent(String email);

        IQueryable<StudentTeacher> GetStudents();
    }
}
