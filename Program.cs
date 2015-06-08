using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace MassScaler
{
    class Program
    {
        static void Main(string[] args)
        {
            int targetWidth = 1980;
            int targetHeight = 1200;

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.jpg", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Console.WriteLine("Processing " + Path.GetFileName(file));
                Image original = Image.FromFile(file);
                Image scaled = ImageScaler.ScaleImage(targetWidth, targetHeight, original);
                string fileName = Path.Combine(Path.GetDirectoryName(file),
                    Path.GetFileNameWithoutExtension(file) + string.Format("_opt_{0}x{1}.jpg", targetWidth, targetHeight));
                scaled.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                scaled.Dispose();
                original.Dispose();
            }
        }
    }

    class ImageScaler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bmp"></param>
        /// <exception cref="OutOfMemoryException">TODO</exception>
        /// <returns></returns>
        public static Image ScaleImage(int width, int height, Image bmp)
        {
            int targetWidth;
            int targetHeight;
            if (bmp.Width > width || bmp.Height > height)
            {
                double wrate = (double)bmp.Width / (double)width;
                double hrate = (double)bmp.Height / (double)height;
                if (wrate > hrate)
                {
                    targetWidth = width;
                    targetHeight = (int)Math.Round(((double)width * (double)bmp.Height / (double)bmp.Width));
                }
                else
                {
                    targetHeight = height;
                    targetWidth = (int)Math.Round(((double)height * (double)bmp.Width / (double)bmp.Height));
                }
            }
            else
                return (Image)bmp.Clone();

            Bitmap image = new Bitmap(targetWidth, targetHeight);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(bmp, 0, 0, targetWidth, targetHeight);
            }
            return image;
        }
    }
}
