using HogwartsAPI.Dtos.EventsDtos;
using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsAPI.Controllers
{
    [ApiController]
    [Route("/api/events")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IFileService<EventUploadDto> _eventsFileService;
        public EventsController(IFileService<EventUploadDto> eventsFileService)
        {
            _eventsFileService = eventsFileService;
        }

        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
        public async Task<ActionResult> GetFile([FromQuery] string fileName)
        {
            var fileDto = await _eventsFileService.GetFile(fileName);
            return File(fileDto.FileContent, fileDto.ContentType, fileDto.FileName);
        }

        [Authorize(Roles = "CourseManager,StudentManager,HouseManager,Admin")]
        [HttpPost]
        public ActionResult Upload([FromBody] EventUploadDto dto)
        {
            string fileName = _eventsFileService.Upload(dto);
            return Created(new Uri(fileName, UriKind.Relative), "File Created");
        }

        [Authorize(Roles = "CourseManager,StudentManager,HouseManager,Admin")]
        [HttpDelete]
        public ActionResult DeleteFile([FromQuery] string fileName)
        {
            _eventsFileService.DeleteFile(fileName);
            return NoContent();
        }
    }
}
