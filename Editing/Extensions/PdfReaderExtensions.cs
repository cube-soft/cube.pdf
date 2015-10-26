using Size = System.Drawing.Size;
using PdfReader = iTextSharp.text.pdf.PdfReader;

namespace Cube.Pdf.Editing.Extensions
{
    internal static class PdfReaderExtensions
    {
        public static Page CreatePage(this PdfReader reader, string path, string password, int pagenum)
        {
            var size = reader.GetPageSize(pagenum);
            var dest = new Page();
            dest.Path = path;
            dest.Size = new Size((int)size.Width, (int)size.Height);
            dest.Rotation = reader.GetPageRotation(pagenum);
            dest.Password = password;
            dest.PageNumber = pagenum;
            return dest;
        }
    }
}
