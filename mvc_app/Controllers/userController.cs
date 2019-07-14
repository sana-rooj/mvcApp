using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Helper;
using mvc_app.Models;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;
namespace mvc_app.Controllers
{
    public class UserController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        UserApi _api = new UserApi();
        public UserController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
       
        
       

        public async Task<IActionResult> DownloadDiffFiles(string FileToDownload)
        {
            if (FileToDownload == null)
                return Content("filename not present");

            var path = Path.Combine(hostingEnvironment.WebRootPath, "images", FileToDownload);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        [HttpGet]
        public IActionResult userpage()
        {
            return View();
           
        }
        public async Task <IActionResult> ShowData(string sort = "Id",int pageno=1)
        {

           
            ViewBag.PageToLoad = pageno;
            List<UserModel> Users = new List<UserModel>();
            HttpClient client = _api.initial();
            HttpResponseMessage res = await client.GetAsync($"api/UserModels?page={pageno}&limit=5&sort={sort}");
            HttpResponseMessage count = await client.GetAsync($"api/UserModels/count");
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                Users = JsonConvert.DeserializeObject<List<UserModel>>(results);
                var resultcount= count.Content.ReadAsStringAsync().Result;
                int Count = JsonConvert.DeserializeObject<int>(resultcount);
                ViewBag.PageCount = Count;
            }
            return View(Users);
        }
       

        [HttpPost]
        public IActionResult PostUser(IFormFile file, UserModel usermodel)
        {


            if (ModelState.IsValid)
            {

                var path = Path.Combine(hostingEnvironment.WebRootPath, "images", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    //for file data into model 
                    file.CopyToAsync(stream);
                    usermodel.FileNames = file.FileName;
                    //for http post
                    HttpClient client = _api.initial();
                    var PostData = client.PostAsJsonAsync<UserModel>("api/UserModels", usermodel);
                    PostData.Wait();
                    var result = PostData.Result;
                    if(result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ShowData");
                    }


                }
                return View("userpage");
            }
            else
                return View("userpage");
        }
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var User = new UserModel();
            HttpClient client = _api.initial();
            HttpResponseMessage msg = await client.DeleteAsync($"api/UserModels/{Id}");
            return RedirectToAction("ShowData");
        }
        // SHOW THE DATA TO BE EDITED
        public ActionResult EditUser(int Id)
        {
            UserModel User = new UserModel();
            HttpClient Client = _api.initial();
            var Response = Client.GetAsync($"api/UserModels/{Id}");
            Response.Wait();
            var Result = Response.Result;
            if(Result.IsSuccessStatusCode)
            {
                var Read = Result.Content.ReadAsAsync<UserModel>();
                Read.Wait();
                User = Read.Result;
            }

            return View(User);
        }
        //POSTING UPDATED DATA 
        [HttpPost]
        public ActionResult EditUser(UserModel UserObj)
        {
            UserModel model = UserObj;
            HttpClient Client = _api.initial();
            var PutData=Client.PutAsJsonAsync<UserModel>("api/UserModels/"+UserObj.Id.ToString(), UserObj);
            PutData.Wait();
            var Result = PutData.Result;
            if(Result.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowData");
            }
            return View(UserObj);
        }

       
    }
}