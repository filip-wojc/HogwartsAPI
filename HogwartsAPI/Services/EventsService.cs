using HogwartsAPI.Dtos.EventsDtos;
using HogwartsAPI.Dtos.FileDtos;
using HogwartsAPI.Exceptions;
using HogwartsAPI.Interfaces;
using HogwartsAPI.Tools;
using Microsoft.AspNetCore.StaticFiles;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace HogwartsAPI.Services
{
    public class EventsService : IFileService<EventUploadDto>
    {
        public async Task<FileDto> GetFile(string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/PrivateFiles/Events/{fileName}";

            if (!File.Exists(filePath))
            {
                throw new NotFoundException("File not found");
            }       
            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out var contentType);
            var fileContent = await File.ReadAllBytesAsync(filePath);
            return new FileDto(fileContent, fileName, contentType);
        }

        public string Upload(EventUploadDto dto)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 13);
            XFont fontBold = new XFont("Verdana", 14, XFontStyle.Bold);
            double margin = 30;
            double yPos = margin + 50;
            double lineHeight = font.GetHeight();

            gfx.DrawString(dto.Date.ToString("dd.MM.yyyy"), fontBold, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(dto.Date.ToString("HH:mm"), fontBold, XBrushes.Black, new XRect(10, 30, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(dto.FullName, fontBold, XBrushes.Black, new XRect(-10, 10, page.Width, page.Height), XStringFormats.TopRight);
            gfx.DrawString(dto.Place, fontBold, XBrushes.Black, new XRect(0, 10, page.Width, page.Height), XStringFormats.TopCenter);

            PdfHandler.DrawWrappedText(dto.Title, fontBold, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopCenter);
            yPos += lineHeight * Math.Ceiling(gfx.MeasureString(dto.Title, font).Width / (page.Width - 2 * margin)) + 40;

            PdfHandler.DrawWrappedText(dto.Description, font, margin, yPos, page.Width - 2 * margin, lineHeight, page, gfx, XStringFormats.TopLeft);
            yPos += lineHeight;


            var rootPath = Directory.GetCurrentDirectory();
            var fullPath = $"{rootPath}/PrivateFiles/Events/event-{dto.Title}-{dto.FullName}.pdf";
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
            var filePath = $"{rootPath}/PrivateFiles/Events/{fileName}";
            if (!File.Exists(filePath))
            {
                throw new NotFoundException("File not found");
            }
            File.Delete(filePath);
        }
    }
}
