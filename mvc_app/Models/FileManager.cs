using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mvc_app.Models
{
    public class FileManager :ControllerBase
    {
        private readonly string FilePath;
        public FileManager(string filepath)
        {
            FilePath = filepath;
        }
        public async Task<StudentRegisterationModel> UploadFileAsync(StudentRegisterationModel M)
        {
          
            var path = Path.Combine(Directory.GetCurrentDirectory(), FilePath, M.file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await M.file.CopyToAsync(stream);
            }
            M.filename = M.file.FileName;
            return M;
        }
        public async Task<IActionResult> DownloadFileAsync(string filename)
        {
            var path =
               Path.Combine(Directory.GetCurrentDirectory(), FilePath, filename);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;           
            return  File(memory, GetContentType(path), Path.GetFileName(path));
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
