﻿using HogwartsAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] {"fileName"})]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/PrivateFiles/{fileName}";
            if (!System.IO.File.Exists(filePath)) 
            {
                return NotFound();
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out var contentType);  
            var fileContent = System.IO.File.ReadAllBytes(filePath);
            return File(fileContent, contentType, fileName);  
        }
        
        [HttpPost]
        public ActionResult Upload([FromForm] FileUploadDto dto)
        {
            var file = dto.File;
            if(file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fullPath = $"{rootPath}/PrivateFiles/{file.FileName}";
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok();
            }
            return BadRequest();
        }
        
    }
}
