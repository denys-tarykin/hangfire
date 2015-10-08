using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Hangfire.Services.Api;

namespace Hangfire.Services.Impl
{
    public class FileSystemStorage : IFileSystemStorage
    {
        private string _location;
        private string _locationWithWotermark;
        private string _watermarkText = "© ASPSnippets.com";
        public FileSystemStorage()
        {
            _location = @"C:\PictureStorage";
            _locationWithWotermark = @"C:\PictureStorage\Watermark\";
            _watermarkText = "Test Watermark";
        }

        public string[] GetAllFiles()
        {
            return Directory.GetFiles(_location);
        }

        public void ApplyWatermark(string picture)
        {
            using (Bitmap bmp = new Bitmap(picture, false))
            {
                using (Graphics grp = Graphics.FromImage(bmp))
                {
                    //Set the Color of the Watermark text.
                    Brush brush = new SolidBrush(Color.Red);

                    //Set the Font and its size.
                    Font font = new System.Drawing.Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel);

                    //Determine the size of the Watermark text.
                    SizeF textSize = new SizeF();
                    textSize = grp.MeasureString(_watermarkText, font);

                    //Position the text and draw it on the image.
                    Point position = new Point((bmp.Width - ((int)textSize.Width + 10)), (bmp.Height - ((int)textSize.Height + 10)));
                    grp.DrawString(_watermarkText, font, brush, position);
                    
                }
                var filename = _locationWithWotermark + Guid.NewGuid().ToString() + ".jpeg";
                bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}