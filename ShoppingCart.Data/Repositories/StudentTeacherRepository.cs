using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class StudentTeacherRepository : IStudentTeacherRepository
    {
        ShoppingCartDbContext _context;
        public StudentTeacherRepository(ShoppingCartDbContext context)
        { _context = context; }
        public void AddMember(StudentTeacher m)
        {
            _context.StudentTeacher.Add(m);
            _context.SaveChanges();
        }

        public StudentTeacher GetStudent(string email)
        {
            return _context.StudentTeacher.Include(x => x.StudentEmail).SingleOrDefault(x => x.TeacherEmail == email);
        }

        public IQueryable<StudentTeacher> GetStudents()
        {
            return _context.StudentTeacher;
        }

    }
}
