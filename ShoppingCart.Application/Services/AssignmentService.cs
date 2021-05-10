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
    public class AssignmentService : IAssignmentService
    {

        private IMapper _autoMapper;
        private IAssignmentRepository _assignmentRepo;

        public AssignmentService(IAssignmentRepository createTaskRepo, IMapper mapper)
        {
            _assignmentRepo = createTaskRepo;
            _autoMapper = mapper;
        }

        public void AddAssignment(AssignmentViewModel assignment)
        {
            Assignment assign = new Assignment()
            {
                FileName = assignment.FileName,
                Description = assignment.Description,
                Path = assignment.Path,
                Signature = assignment.Signature,
                Owner = assignment.Owner,
                TaskId = assignment.TaskId

            };
            _assignmentRepo.AddAssignment(assign);

        }

        public AssignmentViewModel GetTask(Guid id)
        {
            var myTask = _assignmentRepo.GetTask(id);
            var result = _autoMapper.Map<AssignmentViewModel>(myTask);

            return result;
        }


        public IQueryable<AssignmentViewModel> GetAssignments()
        {
            var products = _assignmentRepo.GetAssignments().ProjectTo<AssignmentViewModel>(_autoMapper.ConfigurationProvider);
            return products;

        }

        public IQueryable<AssignmentViewModel> GetAssignments(Guid taskId)
        {
            var assignments = _assignmentRepo.GetAssignments().Where(x => x.Task.taskId == taskId)
                       .ProjectTo<AssignmentViewModel>(_autoMapper.ConfigurationProvider);
            return assignments;

        }

        public IQueryable<AssignmentViewModel> GetAssignments(string owner)
        {
            var assignments = _assignmentRepo.GetAssignments().Where(x => x.Owner == owner)
                       .ProjectTo<AssignmentViewModel>(_autoMapper.ConfigurationProvider);
            return assignments;

        }


        public AssignmentViewModel GetAssignment(Guid id)
        {
            //AutoMapper

            var myAssign = _assignmentRepo.GetAssignment(id);
            var result = _autoMapper.Map<AssignmentViewModel>(myAssign);
            return result;
        }


    }
}
