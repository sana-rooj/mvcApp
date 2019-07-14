using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace mvc_app.Models
{
    public class StudentRegisterationModel
    {
        public StudentRegisterationModel() { id = -1; }
        public int id { get; set; }
        [Required(ErrorMessage = "Enter your Name")]
        public string name { get; set; }
        [Required(ErrorMessage = "Select a Program")]
        public string program { get; set; }
        public string detail { get; set; }
        
        public IFormFile file { get; set; }
        public string filename { get; set; }

    }
}
