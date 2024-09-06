using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Dtos.FileDtos;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Entities;
using HogwartsAPI.Tools;

namespace HogwartsAPI.Services
{
    public class HomeworkSenderService : IFileService<HomeworkResultDto>
    {
        private readonly HogwartDbContext _context;
        private readonly IUserContextService _userContext;
        public HomeworkSenderService(HogwartDbContext context, IUserContextService userContext)
        {
            _context = context;
            _userContext = userContext;
        }
        public async Task<FileDto> GetFile(string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/PrivateFiles/Homeworks/{fileName}";

            if (!File.Exists(filePath))
            {
                throw new NotFoundException("File not found");
            }

            int userId = GetUserIdFromFileName(fileName);

            if (userId != _userContext.UserId && _userContext.UserRole != "CourseManager" && _userContext.UserRole != "Admin")
            {
                throw new ForbidException("You can't see homeworks results that aren't yours");
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out var contentType);
            var fileContent = await File.ReadAllBytesAsync(filePath);
            return new FileDto(fileContent, fileName, contentType);
        }

        public string Upload(HomeworkResultDto dto)
        {
            var homework = _context.Homeworks.Include(h => h.Course).First(h => h.Id == dto.HomeworkId);
            
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 13);
            XFont fontBold = new XFont("Verdana", 14, XFontStyle.Bold);
            double margin = 30;
            double yPos = margin + 50;
            double lineHeight = font.GetHeight();

            gfx.DrawString(dto.SendDate.ToShortDateString(), font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(dto.FullName, font, XBrushes.Black, new XRect(-10, 10, page.Width, page.Height), XStringFormats.TopRight);
            gfx.DrawString(homework.Course.Name, fontBold, XBrushes.Black, new XRect(0, 10, page.Width, page.Height), XStringFormats.TopCenter);

            PdfHandler.DrawWrappedText($"homework: {homework.Description}", font, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopLeft);
            yPos += lineHeight * Math.Ceiling(gfx.MeasureString(homework.Description, font).Width / (page.Width - 2 * margin)) + 40;

            PdfHandler.DrawWrappedText(dto.Title, fontBold, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopCenter);
            yPos += lineHeight + 20;


            PdfHandler.DrawWrappedText(dto.Content, font, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopLeft);
            yPos += lineHeight;


            var rootPath = Directory.GetCurrentDirectory();
            var fullPath = $"{rootPath}/PrivateFiles/Homeworks/homework-{dto.HomeworkId}-{dto.FullName}-{_userContext.UserId}.pdf";
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                document.Save(stream);
            }

           string fileName = Path.GetFileName(fullPath);
           return fileName;
        }

        public void DeleteFile(string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/PrivateFiles/Homeworks/{fileName}";
            if (!File.Exists(filePath))
            {
                throw new NotFoundException("File not found");
            }

            int userId = GetUserIdFromFileName(fileName);

            if (userId != _userContext.UserId && _userContext.UserRole != "CourseManager" && _userContext.UserRole != "Admin")
            {
                throw new ForbidException("You can't delete homework result that you didn't create");
            }

            File.Delete(filePath);
        }

        private int GetUserIdFromFileName(string fileName)
        {
            int startIdIndex = fileName.LastIndexOf("-") + 1;
            int lastIdIndex = fileName.IndexOf(".");
            int userId = int.Parse(fileName.Substring(startIdIndex, lastIdIndex - startIdIndex));
            return userId;
        }

    }
}
