using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace mvc_app.Models
{
    [DataContract]
    public class Message<T>
    {
        [DataMember (Name="IsSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember (Name="ReturnMessage")]
        public string ReturnMessage { get; set; }
        [DataMember (Name="Data")]
        public T Data { get; set; }
    }

    
    public partial class ApiHandler
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }

        public ApiHandler(Uri baseEndpoint) //Base Point: FahadApi thing
        {
            if (baseEndpoint == null)
            {
                throw new ArgumentNullException("baseEndpoint");
            }
            BaseEndpoint = baseEndpoint;
            _httpClient = new HttpClient();
        }
        private async Task<T> GetAsync<T>(Uri requestUrl) //Actual Url: Student Registeration
        {
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }
          private HttpContent CreateHttpContent<T>(T content) //For Sending Stuff
          {
              var json = JsonConvert.SerializeObject(content);
              return new StringContent(json, Encoding.UTF8, "application/json");
          }
        private async Task<Message<T>> PutAsync<T>(Uri requestUrl, T content) // For Editing
        {
            var response = await _httpClient.PutAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T>>(data);
        }
        private async Task<Message<T>> PostAsync<T>(Uri requestUrl, T content) //For Adding 
        {
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T>>(data);
        }
        
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }         
    }
    public partial class ApiHandler
    {
        public async Task<List<StudentRegisterationModel>> GetStudents() //For getting
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "StudentRegisterations")); // Student Registeration here?
            return await GetAsync<List<StudentRegisterationModel>>(requestUrl);
        }

       public async Task<Message<StudentRegisterationModel>> SaveUser(StudentRegisterationModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "StudentRegisterations"));
            return await PostAsync<StudentRegisterationModel>(requestUrl, model);
        }
        public async Task<Message<StudentRegisterationModel>> UpdateUser(StudentRegisterationModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
              "StudentRegisterations"),model.id.ToString());
            return await PutAsync<StudentRegisterationModel>(requestUrl, model);
        }
    }
}

