using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace qdtest._Library
{
    public class ImageLibrary
    {
        public static Image ScaleBySize(Image imgPhoto, int max_size)
        {
            int logoSize = max_size;

            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // Resize Image to have the height = logoSize/2 or width = logoSize.
            // Height is greater than width, set Height = logoSize and resize width accordingly
            if (sourceWidth > (2 * sourceHeight))
            {
                destWidth = logoSize;
                destHeight = (float)(sourceHeight * logoSize / sourceWidth);
            }
            else
            {
                int h = logoSize / 2;
                destHeight = h;
                destWidth = (float)(sourceWidth * h / sourceHeight);
            }
            // Width is greater than height, set Width = logoSize and resize height accordingly

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }
    }
}