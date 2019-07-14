using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc_app.Models;

namespace mvc_app.Controllers
{
    public class RegisterController : Controller
    {
        private  static List<StudentRegisterationModel> StudentList = new List<StudentRegisterationModel>();

        private FileManager FManager;
        private IConfiguration configuration;
        private  readonly string ApiUrl;
        private ApiHandler Handler; 

        public RegisterController(IConfiguration iConfig)
        {
            configuration = iConfig;
            FManager = new FileManager(configuration.GetSection("Manual_Settings").GetSection("FilePath").Value);
            ApiUrl = configuration.GetSection("Manual_Settings").GetSection("ApiUrl").Value;
            Handler = new ApiHandler(new Uri(ApiUrl));
        }

        public IActionResult InputView()
        {          
            return View();
        }
        public IActionResult InputView(StudentRegisterationModel M)
        {       
            return View(M);
        }


        public IActionResult OutputView()
        {
            StudentList = Handler.GetStudents().Result;
            return View(StudentList);
        }
        public void UpdateRecord()
        {

        }
        [HttpPost]
        public async Task<IActionResult> OutputView(StudentRegisterationModel M)
        {
            if (M.file == null) return Content("files not selected");
            await FManager.UploadFileAsync(M);

            if (M.id != -1)
                await Handler.UpdateUser(M);
            else
                await Handler.SaveUser(M);

            StudentList.Add(M);
            return View(StudentList);
        } 

        public IActionResult DownloadFile(string filename)
        {
            if (filename == null) return Content("filename is empty.");
           return FManager.DownloadFileAsync(filename).Result;
        }    
    }

}                                                              