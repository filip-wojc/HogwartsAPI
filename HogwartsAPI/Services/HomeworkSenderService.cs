using HogwartsAPI.Dtos.HomeworksDto;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Dtos.FileDtos;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Microsoft.EntityFrameworkCore;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Services
{
    public class HomeworkSenderService : IHomeworkFileService<HomeworkResultDto>
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

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out var contentType);
            var fileContent = await File.ReadAllBytesAsync(filePath);
            return new FileDto(fileContent, fileName, contentType);
        }

        public async Task<string> Upload(int homeworkId, HomeworkResultDto dto)
        {
            var homework = await GetHomeworkById(homeworkId);

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 16);
            XFont fontBold = new XFont("Verdana", 16, XFontStyle.Bold);
            double margin = 30;
            double yPos = margin + 50;
            double lineHeight = font.GetHeight();

            gfx.DrawString($"{dto.SendDate.ToShortDateString()}", font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString($"{dto.FullName}", font, XBrushes.Black, new XRect(-10, 10, page.Width, page.Height), XStringFormats.TopRight);
            gfx.DrawString($"{homework.Course.Name}", font, XBrushes.Black, new XRect(0, 10, page.Width, page.Height), XStringFormats.TopCenter);

            DrawWrappedText($"homework: {homework.Description}", font, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopLeft);
            yPos += lineHeight * Math.Ceiling(gfx.MeasureString(homework.Description, font).Width / (page.Width - 2 * margin)) + 40;

            DrawWrappedText($"{dto.Title}", fontBold, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopCenter);
            yPos += lineHeight + 20;


            DrawWrappedText($"{dto.Content}", font, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopLeft);
            yPos += lineHeight;


            var rootPath = Directory.GetCurrentDirectory();
            var fullPath = $"{rootPath}/PrivateFiles/Homeworks/homework-{homeworkId}-{dto.FullName}-{_userContext.UserId}.pdf";
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

            int startIdIndex = fileName.LastIndexOf("-") + 1;
            int lastIdIndex = fileName.IndexOf(".");
            int userId = int.Parse(fileName.Substring(startIdIndex, lastIdIndex - startIdIndex));

            if (userId != _userContext.UserId)
            {
                throw new ForbidException("You can't delete homework result that you didn't create");
            }

            File.Delete(filePath);
        }
        private async Task<Homework> GetHomeworkById(int homeworkId)
        {
            var homework = await _context.Homeworks.Include(h => h.Course).FirstOrDefaultAsync(h => h.Id == homeworkId);
            if (homework is null)
            {
                throw new NotFoundException("Homework not found");
            }
            return homework;
        }
        private void DrawWrappedText(string text, XFont textFont, double x, double y, double width, double lineHeight, PdfPage page, XGraphics gfx, XStringFormat format)
        {
            var rect = new XRect(x, y, width, page.Height);

            var size = gfx.MeasureString(text, textFont);
            if (size.Width > rect.Width)
            {
                var words = text.Split(' ');
                var line = string.Empty;

                foreach (var word in words)
                {
                    var testLine = string.IsNullOrEmpty(line) ? word : $"{line} {word}";
                    var testSize = gfx.MeasureString(testLine, textFont);

                    if (testSize.Width > rect.Width)
                    {
                        gfx.DrawString(line, textFont, XBrushes.Black, new XRect(x, y, rect.Width, rect.Height), format);
                        y += lineHeight;
                        line = word;
                    }
                    else
                    {
                        line = testLine;
                    }
                }

                if (!string.IsNullOrEmpty(line))
                {
                    gfx.DrawString(line, textFont, XBrushes.Black, new XRect(x, y, rect.Width, rect.Height), format);
                }
            }
            else
            {
                gfx.DrawString(text, textFont, XBrushes.Black, rect, format);
            }
        }

    }
}
