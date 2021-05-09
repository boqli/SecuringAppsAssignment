using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class FileUploadController : Controller
    {
        private static Guid ID;
        //itfa shit tal assignment awnekk
        private readonly IAssignmentService _assignmentService;
        private readonly ICreateTaskService _createTaskService;
        private readonly ILogger<FileUploadController> _logger;
        private IWebHostEnvironment _env;

        public FileUploadController(ILogger<FileUploadController> logger,IAssignmentService assignmentService, ICreateTaskService createTaskService, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _assignmentService = assignmentService;
            _createTaskService = createTaskService;
        }
        public IActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                var list = _assignmentService.GetAssignments();
                    //_createTaskService.GetTasks().Where(x => x.teacherEmail == User.Identity.Name && x.taskDeadline > DateTime.Now);
                return View(list);
            }
            _logger.LogInformation("Home Index accessed");
            return View();
        }

        [HttpGet]
        public IActionResult Upload(Guid taskId)
        {
            ID = taskId;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(AssignmentViewModel avm)
        {
            try
            {
                if(avm !=null)
                {
                    string filename = Guid.NewGuid() + avm.FileName;
                    string newFilenameWithAbsolutePath = _env.WebRootPath + @"\Files\" + filename;
                    
                    avm.Path = @"\Files\" + filename;
                    avm.TaskId = ID;   
                }

                _assignmentService.AddAssignment(avm);
                TempData["feedback"] = "Assignment was submitted successfully";
            }
            catch (Exception ex)
            {
                //log error
                TempData["warning"] = "Assignment was not submitted! ";
                return View();
            }
            //return View(avm);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Download(Guid id)
        {
            //1. get who is the owner of the file with id = id
            //2. you fetch the private key
            //3. call the HybridDecrypt 
            MemoryStream toDownload = new MemoryStream();// = HybridDecrypt(...)


            return File(toDownload, "application/octet-stream", Guid.NewGuid() + ".pdf");

        }

    }
}
