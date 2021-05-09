using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICreateTaskService _createTaskService;
        private readonly IStudentTeacherService _studentTeacherService;
        private IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, ICreateTaskService createTaskService, IStudentTeacherService studentTeacherService)
        {
            _logger = logger;
            _env = env;
            _createTaskService = createTaskService;
            _studentTeacherService = studentTeacherService;
            //_assignmentService = assignmentService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home Index accessed");
            //var list = _createTaskService.GetTasks();
           // var list2 = _studentTeacherService.GetStudents();
           // ViewBag.StudentTeacher = list2;
            //var list = _studentTeacherService.GetStudents(User.Identity.Name);
            return View();//list
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult ContactUs()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ContactUs(string query, string email)
        {
            //store in db

            return View();
        }
    }
}
