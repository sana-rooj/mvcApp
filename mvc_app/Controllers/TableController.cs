using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;
using mvc_app.Models.Home;
using Newtonsoft.Json;

namespace mvc_app.Controllers
{
    public class TableController : Controller
    {
        private static int tempid;
        public async Task<IActionResult> DeleteRecord(string selectedid)
        {
            var client = new HttpClient();

            UserCollection Coltemp = new UserCollection();
            string baseUrl = "https://localhost:44347/api/RegisteredUsers";// for get
            string Url = "https://localhost:44347/api/RegisteredUsers/" + selectedid;//for delete
            
            try
            {
                var response = await client.DeleteAsync(Url);
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
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }

            return View("Validate", Coltemp);
        }

        public async Task<IActionResult> Update(RegisteredUser user)
        {
            user.id = tempid;
            UserCollection Coltemp = new UserCollection();
            var client = new HttpClient();
            string Url = "https://localhost:44347/api/RegisteredUsers/"+user.id;
            string baseUrl = "https://localhost:44347/api/RegisteredUsers";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Files",
                user.SelectedFile.GetFilename());
            user.FileName = user.SelectedFile.GetFilename();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await user.SelectedFile.CopyToAsync(stream);
            }
            Coltemp.Usercol.Add(user);
            try
            {
                var response = client.PutAsJsonAsync<RegisteredUser>(Url, user);
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

                            
                        }
                        else
                        {
                            Console.WriteLine("NO Data----------");
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }

            return View("Validate", Coltemp);
        }


        public IActionResult MoveToEdit(int id)
        {
            RegisteredUser user=new RegisteredUser();
            tempid = id;
            return View("EditView",user);
        }
        public async Task<IActionResult> RegisterUser()
        {
            return View("Index");
        }
        
    }
}