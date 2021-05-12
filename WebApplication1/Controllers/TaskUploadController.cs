using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class TaskUploadController : Controller
    {
        private readonly ICreateTaskService _createTaskService;
        private IWebHostEnvironment _env;
        private readonly ILogger<FileUploadController> _logger;

        public TaskUploadController(ICreateTaskService createTaskService, IWebHostEnvironment env, ILogger<FileUploadController> logger)
        {
            _createTaskService = createTaskService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                var list = _createTaskService.GetTasks().Where(x => x.teacherEmail == User.Identity.Name && x.taskDeadline > DateTime.Now);
                var ip = HttpContext.Connection.RemoteIpAddress;
                _logger.LogInformation("-->" + User.Identity.Name + " has successfully accessed the task index from this Ip address " + ip);
                return View(list);
            }
            else
            {
                var ip = HttpContext.Connection.RemoteIpAddress;
                _logger.LogInformation("-->" + User.Identity.Name + " has successfully accessed the task index from this Ip address " + ip);
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
            var ip = HttpContext.Connection.RemoteIpAddress;
            if (ct.taskDeadline > DateTime.Now)
            {
                ct.taskTitle = HtmlEncoder.Default.Encode(ct.taskTitle);
                ct.taskDescription = HtmlEncoder.Default.Encode(ct.taskDescription);
                _createTaskService.AddTask(ct);
                TempData["message"] = "Task was created successfully";
                
                _logger.LogInformation("-->" + User.Identity.Name + " has successfully added a task from this Ip address " + ip);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError("-->" + User.Identity.Name + " was unable to add a task from this Ip address " + ip);
                TempData["message"] = "Task failed to be created";
                return View();
            }
            
        }


    }
}
