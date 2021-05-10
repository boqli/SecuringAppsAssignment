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
using ShoppingCart.Domain.Models;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class FileUploadController : Controller
    {
        private static Guid taskID;
        private static Guid assignmentID;
        //itfa shit tal assignment awnekk
        private readonly IAssignmentService _assignmentService;
        private readonly ICommentService _commentService;
        private readonly ICreateTaskService _createTaskService;
        private readonly ILogger<FileUploadController> _logger;
        private IWebHostEnvironment _env;

        public FileUploadController(ILogger<FileUploadController> logger, ICommentService commentService, IAssignmentService assignmentService, ICreateTaskService createTaskService, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _assignmentService = assignmentService;
            _createTaskService = createTaskService;
            _commentService = commentService;
        }
        public IActionResult Index(Guid taskId)
        {
            if (User.IsInRole("Teacher"))
            {
                var list = _assignmentService.GetAssignments(taskId);
                    //_createTaskService.GetTasks().Where(x => x.teacherEmail == User.Identity.Name && x.taskDeadline > DateTime.Now);
                return View(list);
            }
            _logger.LogInformation("Home Index accessed");
            return View();
        }

        [HttpGet]
        public IActionResult Upload(Guid taskId)
        {
            taskID = taskId;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(IFormFile file, AssignmentViewModel avm)
        {
            try
            {
                if(avm !=null)
                {
                    //string filename = Guid.NewGuid() + avm.FileName;
                    string filename = Path.GetFileName(avm.Path);
                    string ext = Path.GetExtension(avm.Path);
                    if(ext.ToLower() != ".pdf")
                    {
                        TempData["message"] = "Submission was not a .pdf";
                        return View();
                    }
                    string filepath = Path.Combine(_env.WebRootPath, "files", filename);
                    //string newFilenameWithAbsolutePath = _env.WebRootPath + @"\Files\" + filename;

                    //using (var fileSteam = new FileStream(filepath, FileMode.Create))
                    //{
                    //    file.CopyTo(fileSteam);
                    //}

                    avm.Path = filepath;
                    avm.TaskId = taskID;
                    //avm.Path = @"\Files\" + filename;
                    _assignmentService.AddAssignment(avm);
                }

                TempData["message"] = "Assignment was submitted successfully";
            }
            catch (Exception ex)
            {
                //log error
                TempData["message"] = "Assignment was not submitted! ";
                return View();
            }
            //return View(avm);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Download(Guid id)
        {
            //1. get who is the owner of the file with id = id
            //2. you fetch the private key
            //3. call the HybridDecrypt 
            
            //var list = _assignmentService.GetAssignments(id);
            string newFilename = _env.WebRootPath + @"\Files\";
            var net = new System.Net.WebClient();
            var data = net.DownloadData(newFilename);
            var down = new System.IO.MemoryStream(data);
            var fname = "hfhfhf.pdf";
            
            return File(down, "application/octet-stream", fname);

        }

        [HttpGet]
        public IActionResult Comment(Guid AssignId)
        {
            assignmentID = AssignId;
            return View();
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Comment(Guid AssignId, CommentViewModel cvm)
        {
            cvm.AssignmentID = assignmentID;

            _commentService.AddComment(cvm);
            TempData["message"] = "Comment was added successfully";
            return View();

        }

        public IActionResult ViewComments(Guid AssignId)
        {
            var list = _commentService.GetComments(AssignId);
            return View(list);  
        }


        public IActionResult AssignList()
        {
            var list = _assignmentService.GetAssignments(User.Identity.Name);
            return View(list);

        }


    }
}
