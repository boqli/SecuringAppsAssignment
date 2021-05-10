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
    public class CommentService : ICommentService
    {

        private IMapper _mapper;
        private ICommentRepository _commentRepo;
        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _mapper = mapper;
            _commentRepo = commentRepository;
        }

        public void AddComment(CommentViewModel comm)
        {
            var myCom = _mapper.Map<Comment>(comm);
            _commentRepo.AddComment(myCom);
        }

        public IQueryable<CommentViewModel> GetComments()
        {
            var comms = _commentRepo.GetComments().ProjectTo<CommentViewModel>(_mapper.ConfigurationProvider);
            return comms;
        }

        public IQueryable<CommentViewModel> GetComments(Guid assingmentId)
        {
            var assignments = _commentRepo.GetComments().Where(x => x.AssignmentID == assingmentId)
                        .ProjectTo<CommentViewModel>(_mapper.ConfigurationProvider);
            return assignments;

        }

    }
}
