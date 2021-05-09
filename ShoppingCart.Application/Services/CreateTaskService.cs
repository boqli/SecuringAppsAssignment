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
    public class CreateTaskService : ICreateTaskService
    {
        private IMapper _autoMapper;
        private ICreateTaskRepository _createTaskRepo;
        private IStudentTeacherRepository _studentTeacherRepository;
        
        public CreateTaskService(ICreateTaskRepository createTaskRepo, IMapper autoMapper, IStudentTeacherRepository studentTeacherRepository)
        {
            _createTaskRepo = createTaskRepo;
            _autoMapper = autoMapper;
            _studentTeacherRepository = studentTeacherRepository;
        }

        public void AddTask(CreateTaskViewModel task)
        {
            var student = _studentTeacherRepository.GetStudents().Where(x => x.TeacherEmail == task.teacherEmail).ToArray();
            foreach(var students in student)
            {
                Task tasks = new Task()
                {
                    taskTitle = task.taskTitle,
                    taskDescription = task.taskDescription,
                    taskDeadline = task.taskDeadline,
                    StudentEmail = students.StudentEmail,
                    TeacherEmail = task.teacherEmail

                };
                _createTaskRepo.AddTask(tasks);
            }
        }

        public IQueryable<CreateTaskViewModel> GetTasks()
        {
            var tasks = _createTaskRepo.GetTasks().ProjectTo<CreateTaskViewModel>(_autoMapper.ConfigurationProvider);
            return tasks;
        }

        public CreateTaskViewModel GetTask(Guid id)
        {
            var myTask = _createTaskRepo.GetTask(id);
            var result = _autoMapper.Map<CreateTaskViewModel>(myTask);
            return result;
        }





    }
}
