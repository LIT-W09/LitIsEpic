using LITIsEpic.Data;
using LITIsEpic.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LITIsEpic.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<ImagesHub> _hubContext;

        public ImagesController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment,
            IHubContext<ImagesHub> hubContext)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
            _hubContext = hubContext;
        }

        [Route("upload")]
        [HttpPost]
        public object Upload(UploadViewModel viewModel)
        {
            var fileName = $"{Guid.NewGuid()}.{viewModel.FileExtension}";
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads", fileName);
            int firstComma = viewModel.Base64File.IndexOf(',');
            string base64 = viewModel.Base64File.Substring(firstComma + 1);
            var image = Convert.FromBase64String(base64);
            System.IO.File.WriteAllBytes(filePath, image);
            var repo = new ImagesRepository(_connectionString);
            int id = repo.Add(fileName);
            return new { id };
        }

        [Route("getrandom")]
        [HttpGet]
        public Image GetRandom()
        {
            var repo = new ImagesRepository(_connectionString);
            var image = repo.GetRandomImage();
            return image;
        }

        [Route("getbyid/{id}")]
        [HttpGet]
        public Image GetById(int id)
        {
            var repo = new ImagesRepository(_connectionString);
            return repo.GetById(id);
        }

        [Route("view/{id}")]
        [HttpGet]
        public IActionResult ViewImage(int id)
        {
            var repo = new ImagesRepository(_connectionString);
            repo.UpdateImageCount(id);
            var image = repo.GetById(id);
            _hubContext.Clients.All.SendAsync("UpdateCount", image).Wait();
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads", image.FileName);
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "image/jpeg");
        }
    }
}
