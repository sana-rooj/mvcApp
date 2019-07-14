using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace mvc_app.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "please enter username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please enter Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter comments")]
        public string Comments { get; set; }
        [Required(ErrorMessage = "please enter a choice")]
        public string Choice { get; set; }
        public string FileNames { get; set; }

    }

}
