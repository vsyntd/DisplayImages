using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;


namespace DisplayImages
{
    public class CSImageHandler
    {
        public static void SplitImage( int absoluteColumn, int absoluteRow, string imageName)
        {
            string sizeFolder;
            string saveFolder;
            string folderPath = DisplayImages.GetModPath.directory;
            switch (absoluteRow)
            {
                case 2 when absoluteColumn == 2: sizeFolder = "64x"; saveFolder = "4"; break;
                case 3 when absoluteColumn == 3: sizeFolder = "96x"; saveFolder = "9"; break;

                default: sizeFolder = "CustomSize"; saveFolder = "CustomSize"; Out("customSize"); break;
            }
            BitImageUpdater.UpdateProgress();
            var dir = new DirectoryInfo(folderPath + "/DisplayImages/ImageFiles/" + saveFolder + "/" + imageName);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(folderPath + "/DisplayImages/ImageFiles/" + saveFolder + "/" + imageName);
            }
            else 
            {
                dir.Delete(true); // recursive delete
                Directory.CreateDirectory(folderPath + "/DisplayImages/ImageFiles/" + saveFolder + "/" + imageName);
            }
            string path = folderPath + "/DisplayImages/ImageFiles/" + sizeFolder + "/" + imageName + ".png";
            if (!File.Exists(path))
            {
                return;
            }
            //get image data (pixel arrangement etc.)
            Bitmap image = new Bitmap(filename: path);
            Out("original width: " + image.Width.ToString());
            Out("original height: " + image.Height.ToString());
            Bitmap[,] pieces = new Bitmap[absoluteRow, absoluteColumn];

            BitImageUpdater.UpdateProgress();

            if (sizeFolder == "CustomSize") // break out of this Method for custom image splitting
            {
                SplitCustomImage(image, absoluteColumn, absoluteRow, imageName);
                return;
            }
            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColumn; col++)
                {
                    pieces[row, col] = new Bitmap(width: BitImageUpdater.width, height: BitImageUpdater.height);       // initialize Objects
                }
            }
            BitImageUpdater.UpdateProgress();
            Out("pieces width: " + (image.Width / absoluteRow).ToString());
            Out("pieces height: " + (image.Height / absoluteColumn).ToString());
            // read each pixel in each 32x32 Pixel-Block of a piece
            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColumn; col++)
                {
                    for (int x = 0; x < image.Width / absoluteRow; x++)
                    {
                        for (int y = 0; y < image.Height / absoluteColumn; y++)
                        {
                            Color originalColour = image.GetPixel(x + (BitImageUpdater.width * row), y + (BitImageUpdater.height * col));
                            pieces[row, col].SetPixel(x, y, originalColour);      // set pixels for Objects
                        }
                    }
                }
            }
            int counter = 1;

            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColumn; col++)
                {
                    pieces[col, row].Save(folderPath + "/DisplayImages/ImageFiles/" + saveFolder + "/" + imageName + "/" + imageName + "_" + (row + 1).ToString() + "_" + (col + 1).ToString() + ".png");
                    counter++;
                }
            }
            Out("|GREEN|finished splitting");
            BitImageUpdater.UpdateProgress();
        }
        public static void ResizeImage(string imageName, string path, string size, int width = BitImageUpdater.width, int height = BitImageUpdater.height)
        {
            string folder;   
            switch (size)
            {
                case "s": width = 32; height = 32; folder = "1"; break;
                case "b": width = 64; height = 64; folder = "64x"; break;
                case "h": width = 96; height = 96; folder = "96x"; break;

                case "p": folder = "CustomSize"; break;

                default: DuckGame.DevConsole.Log(DuckGame.DCSection.Mod, "|RED| wrong size format use (s,b,h or p)");return;
            }
            BitImageUpdater.UpdateProgress();
            //System Classes
            Image image = Image.FromStream(File.OpenRead(path));
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap output = new Bitmap(width, height);

            BitImageUpdater.UpdateProgress();

            output.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphic = Graphics.FromImage(output))    //process new pixel arrangement for BitMap output
            {
                graphic.CompositingMode = CompositingMode.SourceCopy;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                
                graphic.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }
            BitImageUpdater.UpdateProgress();
            image.Dispose();
            output.Save(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/" + folder + "/" + imageName + ".png");

            BitImageUpdater.UpdateProgress();

            Out("|GREEN|finished resizing");
        }
        private static void SplitCustomImage(Bitmap image, int absoluteColumn, int absoluteRow, string imageName)
        {
            int width = (int)(image.Width / absoluteColumn);
            int height = (int)(image.Height / absoluteRow);
            Bitmap[,] pieces = new Bitmap[absoluteRow, absoluteColumn];

            BitImageUpdater.UpdateProgress();

            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColumn; col++)
                {
                    pieces[row, col] = new Bitmap(BitImageUpdater.width, BitImageUpdater.height);       // initialize empty Objects
                }
            }
            Out("pieces width: " + width.ToString());
            Out("pieces height: " + height.ToString());

            BitImageUpdater.UpdateProgress();
            // read each pixel in each Pixel-Block of a piece
            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColumn; col++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Color originalColour = image.GetPixel(x + (width * col), y + (height * row));
                            pieces[row, col].SetPixel(x, y, originalColour);
                        }
                    }
                }
            }
            BitImageUpdater.UpdateProgress();

            for (int row = 0; row < absoluteRow; row++)
            {
                for (int col = 0; col < absoluteColumn; col++)
                {
                    pieces[row, col].Save(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/CustomSize/" + imageName + "/" + imageName + "_" + (row + 1).ToString() + "_" + (col + 1).ToString() + ".png");
                }
            }
            BitImageUpdater.UpdateProgress();
            Out("|GREEN|finished splitting");
        }
        public static void CutOutByFormat(string imageName, string format)
        {
            string save = "";
            int one = 0;
            int width = 32;
            int height = 32;

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == ':')
                {
                    one = Int32.Parse(save);
                    save = "";
                }
                else
                {
                    save = save + format[i];
                }
            }
            int two = Int32.Parse(save);
            Bitmap image = Image.FromFile(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/rawImages/" + imageName + ".png") as Bitmap;

            int xOffset, yOffset;
            xOffset = -1;
            yOffset = -1;
            if (one > two)   // landscape format
            {
                float ratio = (float)two / (float)one;
                height = (int)(image.Height * ratio);

                width = image.Width;
                xOffset = 0;
                yOffset = (int)(height / 2); // center Offset
            }
            if (one < two)   // portrait format
            {
                float ratio = (float)one / (float)two;
                width = (int)(image.Width * ratio);

                height = image.Height;
                xOffset = (int)(width / 2); // center Offset
                yOffset = 0;
            }
            if (one == two)
            {
                return;
            }
            Rectangle cutOutSection = new Rectangle(new Point(xOffset, yOffset), new Size(width, height));
            Bitmap bitmap = new Bitmap(cutOutSection.Width, cutOutSection.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(image, 0, 0, cutOutSection, GraphicsUnit.Pixel);
            }
            bitmap.Save(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/CustomSize/" + imageName + ".png");
            bitmap.Dispose();
        }
        public static void ResizeToGrid(string imageName,string imagePath, int columns, int rows, string toGrid = null)
        {
            Image image = Image.FromStream(File.OpenRead(imagePath));

            int width = image.Width;
            int height = image.Height;
            bool resizeToNextPossibleGrid = toGrid != null;
            BitImageUpdater.UpdateProgress();
            if (!resizeToNextPossibleGrid)
            {
                width = columns * BitImageUpdater.width;
                height = rows * BitImageUpdater.height;
            }
            
            else if (toGrid == "toGrid")
            {
                int moduloWidth = image.Width % BitImageUpdater.width;
                int moduloHeight = image.Height % BitImageUpdater.height;

                if(moduloHeight != 0)
                {
                    height -= moduloHeight;
                }
                if (moduloWidth != 0)
                {
                    width -= moduloWidth;
                }
            }
            BitImageUpdater.UpdateProgress();
            Out("new width: "+width.ToString());
            Out("new height: "+height.ToString());

            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap output = new Bitmap(width, height);

            output.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphic = Graphics.FromImage(output))    //process new pixel arrangement for BitMap output
            {
                graphic.CompositingMode = CompositingMode.SourceCopy;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.SmoothingMode = SmoothingMode.HighQuality;

                graphic.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }
            BitImageUpdater.UpdateProgress();
            Out("total Grid: " + "colums: " + (width / BitImageUpdater.width).ToString() + " " + "rows: " + (height / BitImageUpdater.height).ToString());

            if (width > image.Width || height > image.Height) Out("|ORANGE|note: image is upscaled");

            image.Dispose();
            output.Save(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/CustomSize/" + imageName + ".png");
            Out("|GREEN|finished resizing");

            if (toGrid != null)
            {
                SplitImage( width / BitImageUpdater.width, height / BitImageUpdater.height, imageName);
            }
            BitImageUpdater.UpdateProgress();
        }
        public static void Out(string output)
        {
            DuckGame.DevConsole.Log(DuckGame.DCSection.Mod, output);
        }
    }
}
