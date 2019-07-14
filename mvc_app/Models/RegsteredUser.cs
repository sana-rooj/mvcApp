using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace mvc_app.Models
{
    [DataContract]
    public class RegisteredUser
    {
        //public RegisteredUser()
        //{
        //    id = 0;
        //    Name = "";
        //    email_address = "";
        //    phone_number = "";
        //    job_type = "";
        //    password = "";
        //    FileName = "";
        //}

        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "name")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be within 3 and 60 letters")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [DataMember(Name = "email_address")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        [Required(ErrorMessage = "Email is required")]
        public string email_address { get; set; }

        [DataMember(Name = "phone_number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string phone_number { get; set; }

        [DataMember(Name = "job_type")]
        [Required(ErrorMessage = "Job Type is required")]
        public string job_type { get; set; }

        [DataMember(Name = "password")]
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        public IFormFile SelectedFile{ get; set; }

        [DataMember(Name = "filename")]
        public string FileName { get; set; }
        
    }
    
}
