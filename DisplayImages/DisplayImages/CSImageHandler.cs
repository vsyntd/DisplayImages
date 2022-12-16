using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using DuckGame;

namespace DisplayImages
{
    public class CSImageHandler
    {
        public static void SplitImage(string path, int absoluteRow, int absoluteColum, string imageName)
        {
            string sizeFolder = "";
            string saveFolder = "";
            switch (absoluteRow)
            {
                case 2: if (absoluteColum == 2) sizeFolder = "64x"; saveFolder = "4"; break;
                case 3: if (absoluteColum == 3) sizeFolder = "96x"; saveFolder = "9"; break;
            }
            Directory.CreateDirectory(path + "/DisplayImages/ImageFiles/" + saveFolder + "/" + imageName);
                
            //get image data (pixel arrangement etc.)
            Bitmap image = new Bitmap(path + "/DisplayImages/ImageFiles/" + sizeFolder + "/" + imageName + ".png");
            Bitmap[,] pieces = new Bitmap[absoluteRow, absoluteColum];

            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColum; col++)
                {
                    pieces[row, col] = new Bitmap(BitImage.width, BitImage.height);       // initialize empty Objects
                }
            }
            // read each pixel in each 32x32 Pixel-Block 
            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColum; col++)
                {
                    for (int x = 0; x < image.Width / absoluteRow; x++)
                    {
                        for (int y = 0; y < image.Height / absoluteColum; y++)
                        {
                            System.Drawing.Color originalColour = image.GetPixel(x + ((int)BitImage.width * row), y + ((int)BitImage.height * col));
                            pieces[row, col].SetPixel(x, y, originalColour);      
                        }
                    }
                }
            }
            int counter = 1;
            
            for(int row = 0; row < absoluteRow; row++)
            {
                for(int col = 0; col < absoluteColum; col++)
                {
                    pieces[col, row].Save(path + "/DisplayImages/ImageFiles/" + saveFolder + "/" + imageName + "/" + imageName + counter.ToString() + ".png");
                    counter++;
                }
            }
            DevConsole.Log(DCSection.Mod, "|GREEN| finished splitting");
        }
        public static void ResizeImage(string imageName, string path, string size = "", int width = 32, int height = 32)
        {
            string folder = "1";    // 32 pixels on default
            switch (size)
            {
                case "s": width = 32; height = 32; folder = "1"; break;
                case "b": width = 64; height = 64; folder = "64x"; break;
                case "h": width = 96; height = 96; folder = "96x"; break;

                case "p": folder = "CustomSize"; break;

                default: DevConsole.Log(DCSection.Mod,"|RED| wrong size format use (s,b,h or p)"); break;
            }
            //System Classes
            Image image = Image.FromStream(File.OpenRead(path + "/DisplayImages/ImageFiles/rawImage/"+imageName+".png"));
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, width, height);
            Bitmap output = new Bitmap(width, height);

            output.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphic = System.Drawing.Graphics.FromImage(output))    //process new pixel arrangement
            {
                graphic.CompositingMode = CompositingMode.SourceCopy;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic; 
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.SmoothingMode = SmoothingMode.HighQuality;

                graphic.DrawImage(image, destRect, 0, 0, image.Height, image.Width, GraphicsUnit.Pixel);
            }
            image.Dispose();
            output.Save(path + "/DisplayImages/ImageFiles/"+ folder + "/"+ imageName + ".png");

            DevConsole.Log(DCSection.Mod, "|GREEN| finished resizing");
        }
    }
}
