using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace HogwartsAPI.Tools
{
    public static class PdfHandler
    {

        public static void DrawWrappedText(string text, XFont textFont, double x, double y, double width, double lineHeight, PdfPage page, XGraphics gfx, XStringFormat format)
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
