using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace BobWei.CSharp.Common.Utility
{
    public static class GraphicsUtility
    {
        /// <summary>
        /// 空缩略图
        /// </summary>
        /// <param name="text">显示文字</param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <returns></returns>
        public static Bitmap EmptyThumbnail(string text, int desiredWidth, int desiredHeight)
        {
            var b = new Bitmap(desiredWidth, desiredHeight);
            using (Graphics g = Graphics.FromImage(b))
            {
                var font = new Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel);
                SizeF size = g.MeasureString(text, font);
                g.DrawRectangle(new Pen(Brushes.Gray), 0, 0, desiredWidth - 2, desiredHeight - 2);
                g.DrawString(text, font, Brushes.Black,
                             new PointF((desiredWidth - size.Width)/2, (desiredHeight - size.Height)/2));
            }
            return b;
        }

        /// <summary>
        /// Creates a thumbnail from an existing image. Sets the biggest dimension of the
        /// thumbnail to either desiredWidth or Height and scales the other dimension down
        /// to preserve the aspect ratio
        /// </summary>
        /// <param name="originalBmp">bitmap to create thumbnail for</param>
        /// <param name="desiredWidth">maximum desired width of thumbnail</param>
        /// <param name="desiredHeight">maximum desired height of thumbnail</param>
        /// <returns>Bitmap thumbnail</returns>
        public static Bitmap CreateThumbnail(Bitmap originalBmp, int desiredWidth, int desiredHeight)
        {
            // If the image is smaller than a thumbnail just return it
            if (originalBmp.Width == desiredWidth && originalBmp.Height == desiredHeight)
            {
                return originalBmp;
            }

            int newWidth, newHeight;
            if (originalBmp.Width > desiredWidth || originalBmp.Height > desiredHeight)
            {
                // scale down the smaller dimension
                if (desiredWidth*originalBmp.Height < desiredHeight*originalBmp.Width)
                {
                    newWidth = desiredWidth;
                    newHeight = (int) Math.Round((decimal) originalBmp.Height*desiredWidth/originalBmp.Width);
                }
                else
                {
                    newHeight = desiredHeight;
                    newWidth = (int) Math.Round((decimal) originalBmp.Width*desiredHeight/originalBmp.Height);
                }
            }
            else
            {
                newWidth = originalBmp.Width;
                newHeight = originalBmp.Height;
            }

            // This code creates cleaner (though bigger) thumbnails and properly
            // and handles GIF files better by generating a white background for
            // transparent images (as opposed to black)
            // This is preferred to calling Bitmap.GetThumbnailImage()
            var bmpOut = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(bmpOut))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
            }

            return bmpOut;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalBmp"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <param name="backgroundColor">空白的填充颜色</param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(Bitmap originalBmp, int desiredWidth, int desiredHeight,
                                             Brush backgroundColor)
        {
            //if (originalBmp.Width <= desiredWidth && originalBmp.Height <= desiredHeight)
            if (originalBmp.Width == desiredWidth && originalBmp.Height == desiredHeight)
            {
                return originalBmp;
            }

            int newWidth, newHeight;
            if (originalBmp.Width > desiredWidth || originalBmp.Height > desiredHeight)
            {
                // scale down the smaller dimension
                if (desiredWidth*originalBmp.Height < desiredHeight*originalBmp.Width)
                {
                    newWidth = desiredWidth;
                    newHeight = (int) Math.Round((decimal) originalBmp.Height*desiredWidth/originalBmp.Width);
                }
                else
                {
                    newHeight = desiredHeight;
                    newWidth = (int) Math.Round((decimal) originalBmp.Width*desiredHeight/originalBmp.Height);
                }
            }
            else
            {
                newWidth = originalBmp.Width;
                newHeight = originalBmp.Height;
            }

            var newX = (int) Math.Round((desiredWidth - newWidth)/2.0);
            var newY = (int) Math.Round((desiredHeight - newHeight)/2.0);

            var bmpOut = new Bitmap(desiredWidth, desiredHeight);

            using (Graphics graphics = Graphics.FromImage(bmpOut))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(backgroundColor, 0, 0, desiredWidth, desiredHeight);
                //graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, newX, newY, newWidth, newHeight);
            }

            return bmpOut;
        }

        public static Image ToImage(byte[] byt)
        {
            Image image = null;
            if (byt != null && byt.Length > 0)
            {
                var ms = new MemoryStream(byt);
                try
                {
                    image = Image.FromStream(ms);
                }
                catch (Exception ex)
                {
                    Debug.Print("内存流转成图片(Image)失败" + ex.Message);
                }
            }
            return image;
        }
    }
}