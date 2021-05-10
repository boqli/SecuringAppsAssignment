using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
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
        private readonly ILogger<FileUploadController> _logger;
        private IWebHostEnvironment _env;

        public FileUploadController(ILogger<FileUploadController> logger, ICommentService commentService, IAssignmentService assignmentService, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _assignmentService = assignmentService;
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
        [Authorize]
        public IActionResult Upload(IFormFile file, AssignmentViewModel avm)
        {
            try
            {
                if (avm != null)
                {
                    avm.Description = HtmlEncoder.Default.Encode(avm.Description);
                    string fileName;
                    if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
                    {
                        //25 50 44 46 2d >> 37 80 68 70 45 PDF
                        byte[] whitelist = new byte[] { 37, 80, 68, 70, 45 };
                        if (file != null)
                        {
                            MemoryStream userFile = new MemoryStream();

                            using (var f = file.OpenReadStream())
                            {
                                byte[] buffer = new byte[5];  //how to read an x amount of bytes at 1 go
                                f.Read(buffer, 0, 5); //offset - how many bytes you would lke the pointer to skip

                                for (int i = 0; i < whitelist.Length; i++)
                                {
                                    if (whitelist[i] == buffer[i])
                                    {
                                    }
                                    else
                                    {
                                        //the file is not acceptable
                                        ModelState.AddModelError("file", "File is not valid and acceptable");
                                        return View();
                                    }
                                }
                                //...other reading of bytes happening
                                f.Position = 0;
                                //uploading the file
                                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                avm.Path = fileName;
                                avm.TaskId = taskID;
                                string absolutePath = _env.WebRootPath + @"\Files\" + fileName;
                                try
                                {
                                    using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                                    {
                                        // throw new Exception();
                                        f.CopyTo(fsOut);
                                    }
                                    f.Close();
                                }
                                catch (Exception ex)
                                {
                                    //log
                                    _logger.LogError(ex, "Error happend while saving file");
                                    return View("Error", new ErrorViewModel() { Message = "Error while saving the file. Try again later" });
                                }
                            }
                        }
                        else
                        {
                            TempData["error"] = "Invalid File!";
                            return View();
                        }
                    }
                    _assignmentService.AddAssignment(avm);
                    TempData["message"] = "Assignment added successfully";
                    return View();
                }
                else
                {
                    TempData["message"] = "Fill in the form correctly!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Could not add assignment!";
                return View();
            }
   
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Download(Guid AssignId)
        {
            //1. get who is the owner of the file with id = id
            //2. you fetch the private key
            //3. call the HybridDecrypt 
            
            var assignment = _assignmentService.GetAssignment(AssignId);
            var webClient = new System.Net.WebClient();
            string absolutePath = _env.WebRootPath + @"\Files\" + assignment.Path;
            var downData = webClient.DownloadData(absolutePath);
            MemoryStream toDownload = new MemoryStream();// = HybridDecrypt(...)
            new System.IO.MemoryStream(downData);
            var fileDownloadName = assignment.FileName+".pdf";
            return File(toDownload, "application/octet-stream" , Guid.NewGuid() + fileDownloadName);
           
        }

        [HttpGet]
        public IActionResult Comment(Guid AssignId)
        {
            assignmentID = AssignId;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Comment(Guid AssignId, CommentViewModel cvm)
        {
            cvm.AssignmentID = assignmentID;

            _commentService.AddComment(cvm);
            TempData["message"] = "Comment was added successfully";
            return View();

        }
        [Authorize]
        public IActionResult ViewComments(Guid AssignId)
        {
            var list = _commentService.GetComments(AssignId);
            return View(list);  
        }

        [Authorize]
        public IActionResult AssignList()
        {
            var list = _assignmentService.GetAssignments(User.Identity.Name);
            return View(list);

        }


    }
}
