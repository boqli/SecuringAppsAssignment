using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class StudentTeacherService : IStudentTeacherService
    {
        private IStudentTeacherRepository _membersRepo;
        private IMapper _autoMapper;
        public StudentTeacherService(IStudentTeacherRepository repo, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _membersRepo = repo;
        }
        public void AddMember(StudentTeacherViewModel m)
        {
            StudentTeacher member = new StudentTeacher()
            {
                TeacherEmail = m.teacherEmail,
                StudentEmail = m.studentEmail

            };
            _membersRepo.AddMember(member);
        }

        public IQueryable<StudentTeacherViewModel> GetStudents(string keyword)
        {
            var Stud = _membersRepo.GetStudents().Where(x => x.TeacherEmail == keyword)
               .ProjectTo<StudentTeacherViewModel>(_autoMapper.ConfigurationProvider);
            return Stud;

        }

        public IQueryable<StudentTeacherViewModel> GetStudents2(string studentEmail)
        {
            var Stud = _membersRepo.GetStudents().Where(x => x.StudentEmail == studentEmail)
               .ProjectTo<StudentTeacherViewModel>(_autoMapper.ConfigurationProvider);
            return Stud;
        }

        public IQueryable<StudentTeacherViewModel> GetStudents()
        {
            var studs = _membersRepo.GetStudents().ProjectTo<StudentTeacherViewModel>(_autoMapper.ConfigurationProvider);
            return studs;
        }
    }
}
