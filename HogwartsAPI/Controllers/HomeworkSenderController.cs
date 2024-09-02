using HogwartsAPI.Dtos;
using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/homework-results")]
    public class HomeworkSenderController : ControllerBase
    {
        private readonly IHomeworkFileService<HomeworkResultDto> _homeworkFileService;
        public HomeworkSenderController(IHomeworkFileService<HomeworkResultDto> homeworkFileService)
        {
            _homeworkFileService = homeworkFileService;
        }
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
        public async Task<ActionResult> GetFile([FromQuery] string fileName)
        {
            var fileDto = await _homeworkFileService.GetFile(fileName);
            return File(fileDto.FileContent, fileDto.ContentType, fileDto.FileName);
        }

        [HttpPost("{homeworkId}")]
        public async Task<ActionResult> Upload([FromRoute] int homeworkId, [FromBody] HomeworkResultDto dto)
        {
            string fileName = await _homeworkFileService.Upload(homeworkId, dto);
            return Created(new Uri(fileName, UriKind.Relative), "File Created");
        }
        [HttpDelete]
        public ActionResult DeleteFile([FromQuery] string fileName)
        {
            _homeworkFileService.DeleteFile(fileName);
            return NoContent();
        }
    }
}
