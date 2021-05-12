using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAssignmentService _assignmentService;
        private readonly ICommentService _commentService;
        private readonly ILogger<FileUploadController> _logger;
        private IWebHostEnvironment _env;

        public FileUploadController(ILogger<FileUploadController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager , ICommentService commentService, IAssignmentService assignmentService, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;
            _assignmentService = assignmentService;
            _commentService = commentService;
        }

        [Authorize]
        public IActionResult Index(string Id)
        {
            if (User.IsInRole("Teacher"))
            {
                Guid decriptID = new Guid(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Id)));
                return View(_assignmentService.GetAssignments(decriptID));
                //var list = _assignmentService.GetAssignments(taskId);
                //_createTaskService.GetTasks().Where(x => x.teacherEmail == User.Identity.Name && x.taskDeadline > DateTime.Now);
                //return View(list);
            }

            var ip = HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation("-->"+User.Identity.Name + " has accessed the home page from this Ip address " + ip);
            
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
        public async Task<IActionResult> UploadAsync(IFormFile file, AssignmentViewModel avm)
        {
            try
            {
                if (avm != null)
                {
                    //if(avm.TaskId)
                    avm.Description = HtmlEncoder.Default.Encode(avm.Description);
                    avm.FileName = HtmlEncoder.Default.Encode(avm.FileName);
                    var ip = HttpContext.Connection.RemoteIpAddress; ;
                    if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
                    {
                       
                        //25 50 44 46 2d >> 37 80 68 70 45 PDF
                        byte[] whitelist = new byte[] { 37, 80, 68, 70, 45 };
                        if (file != null)
                        {
                            var sign = await _userManager.FindByNameAsync(User.Identity.Name);
                            
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
                                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                avm.Path = fileName;
                                avm.TaskId = taskID;
                                MemoryStream msIn = new MemoryStream(Encoding.UTF32.GetBytes(fileName));
                                string signature = Encryption.SignData(msIn, sign.PrivateKey);
                                avm.Signature = signature;
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
                                    
                                    _logger.LogError("-->" + User.Identity.Name + " has encountered an error while saving file from this Ip address " + ip + " || " + ex);
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
                    ip = HttpContext.Connection.RemoteIpAddress;
                    _logger.LogInformation("-->" + User.Identity.Name + " has successfully saved a file from this Ip address " + ip);
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
        public async Task<IActionResult> DownloadAsync(Guid AssignId)
        {
            
            //1. get who is the owner of the file with id = id
            //2. you fetch the private key
            //3. call the HybridDecrypt 
            var assignment = _assignmentService.GetAssignment(AssignId);
            var webClient = new System.Net.WebClient();
            string absolutePath = _env.WebRootPath + @"\Files\" + assignment.Path;
            var downData = webClient.DownloadData(absolutePath);
            var sign = await _userManager.FindByNameAsync(User.Identity.Name);
            MemoryStream toDownload = new MemoryStream(downData);

            MemoryStream other = new MemoryStream(Encoding.UTF32.GetBytes(assignment.FileName));// = HybridDecrypt(...)
            other.Position = 0;

            //encrypt upload decrypt down
            //-->bool = false
           
            //return File(toDownload, "application/octet-stream", Guid.NewGuid() + "-" + fileDownloadName);
             
            if (Encryption.VerifyData(toDownload, sign.PublicKey, assignment.Signature)) {
                var fileDownloadName = assignment.FileName + ".pdf";
                var ip = HttpContext.Connection.RemoteIpAddress;
                _logger.LogInformation("-->" + User.Identity.Name + " has successfully downloaded a file from this Ip address " + ip);
                return File(toDownload, "application/octet-stream", Guid.NewGuid() + "-" + fileDownloadName);
            }
            else
            {
                TempData["error"] = "File could not be downloaded!";
                return View();
            }
            /*
            MemoryStream toDownload = new MemoryStream(Encoding.UTF32.GetBytes(fileName));
            toDownload.Position = 0;

            bool result = Encryption.VerifyData(msIn2, sign.PublicKey, signature);

          */
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
        public IActionResult Comment(CommentViewModel cvm)
        {

            cvm.AssignmentID = assignmentID;
            cvm.CommentText = HtmlEncoder.Default.Encode(cvm.CommentText);
            _commentService.AddComment(cvm);
            var ip = HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation("-->" + User.Identity.Name + " has successfully added a comment from this Ip address " + ip);
            TempData["message"] = "Comment was added successfully";
            //Guid decriptID = new Guid(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(AssignId)));
            //return View(_commentService.GetComments(decriptID));
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult ViewComments(string Id)
        {
            Guid decriptID = new Guid(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Id)));
            var ip = HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation("-->" + User.Identity.Name + " has viewed comments on an assignment from this Ip address " + ip);
            return View(_commentService.GetComments(decriptID));
            //var list = _commentService.GetComments(AssignId);
            //return View(list);  
        }

        [Authorize]
        public IActionResult AssignList()
        {
            //Guid decriptID = new Guid(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(AssignId)));
            //return View(_commentService.GetComments(decriptID));
            var list = _assignmentService.GetAssignments(User.Identity.Name);
            var ip = HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation("-->" + User.Identity.Name + " has entered the assignment list page from this Ip address " + ip);
            return View(list);
        }



    }
}
