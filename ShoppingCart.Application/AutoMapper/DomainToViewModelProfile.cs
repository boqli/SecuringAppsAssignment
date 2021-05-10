using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.AutoMapper
{
    public class DomainToViewModelProfile: Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Task, CreateTaskViewModel>();
            CreateMap<Assignment, AssignmentViewModel>();
            CreateMap<StudentTeacher, StudentTeacherViewModel>();
            CreateMap<Comment, CommentViewModel>();
            //Product class was used to model the database
            //ProductViewModel class was used to pass on the data to/from the Presentation project/layer
        }

    }
}
