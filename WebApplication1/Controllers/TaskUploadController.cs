using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class TaskUploadController : Controller
    {
        private readonly ICreateTaskService _createTaskService;
        private IWebHostEnvironment _env;


        public TaskUploadController(ICreateTaskService createTaskService, IWebHostEnvironment env)
        {
            _createTaskService = createTaskService;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                var list = _createTaskService.GetTasks().Where(x => x.teacherEmail == User.Identity.Name && x.taskDeadline > DateTime.Now);
                return View(list);
            }
            else
            {
                var list = _createTaskService.GetTasks().Where(x => x.studentEmail == User.Identity.Name && x.taskDeadline > DateTime.Now);
                return View(list);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public IActionResult Upload(IFormFile file, CreateTaskViewModel ct)
        {
            if(ct.taskDeadline > DateTime.Now)
            {
                _createTaskService.AddTask(ct);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
            
        }


    }
}
