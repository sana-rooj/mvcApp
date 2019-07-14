using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Text;
using mvc_app.Models.Home;
using Microsoft.Extensions.FileProviders;
using mvc_app.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mvc_app.Controllers
{
    public class HomeController : Controller
    {
        

       

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MoveToTable()
        {
            UserCollection Coltemp = new UserCollection();
            string baseUrl = "https://localhost:44347/api/RegisteredUsers";
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {

                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                List<RegisteredUser> test = JsonConvert.DeserializeObject<List<RegisteredUser>>(data);
                                foreach (var item in test)
                                {
                                    Coltemp.Usercol.Add(item);
                                }

                                Console.WriteLine("data------------{0}", data);
                            }
                            else
                            {
                                Console.WriteLine("NO Data----------");
                            }

                        }
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }

            return View("Validate",Coltemp);
        }
     
        

        
        [HttpPost]
        public async Task<IActionResult> Validate(RegisteredUser u)
        {
            UserCollection Coltemp = new UserCollection();
            var client = new HttpClient();
            string Url = "https://localhost:44347/api/RegisteredUsers";
            if (ModelState.IsValid)
            {
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot/Files",
                    u.SelectedFile.GetFilename());
                u.FileName = u.SelectedFile.GetFilename();
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await u.SelectedFile.CopyToAsync(stream);
                }
                Coltemp.Usercol.Add(u);
                var response= await client.PostAsJsonAsync<RegisteredUser>(Url,u);
                return await MoveToTable();
            }

            return View("Index");
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", filename);

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
                {".csv", "text/csv"},
                {".cs","text/plain" }
            };
        }
    }
}
