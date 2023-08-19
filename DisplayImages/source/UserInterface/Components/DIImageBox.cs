using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source.UserInterface
{
    public class DIImageBox : DIComponent
    {
        private string prevPath = "";
        public DIImageBox(Graphics g, string name, int x, int y, int width, int height)
        {
            base.name = name;
            base.g = g;
            base.x = x;
            base.y = y;
            base.width = width;
            base.height = height;
        }
        public override void Update(MESSAGE msg)
        {
            Bitmap imageFromPath = null;
            if (DIWindow.filePath != "")
            {
                try
                {
                    imageFromPath = new Bitmap(DIWindow.filePath);
                }
                catch (ArgumentException)
                {
                    DIWindow.filePath = "please choose a valid image !";
                }
                finally
                {
                    if (imageFromPath != null && DIWindow.filePath != prevPath)
                    {
                        g.FillRectangle(new SolidBrush(DIWindow.bg), new RectangleF( x, y, width ,height)); // reset old image 
                        g.DrawImage(imageFromPath, x, y, width, height);
                        prevPath = DIWindow.filePath;
                    }
                }
            }
            else
            {
                DrawSelect();
            }
            g.DrawRectangle(DIWindow.BlackPen, new Rectangle(x, y, width, height));

        }
        private void DrawSelect()
        {
            string text = "Select";
            DIWindow.DrawText(text, x + (width / 2) - (12 * text.Length) / 2, y + (height / 2) - 9, 9, StringAlignment.Center, 70,30);
            string text2 = "Image";
            DIWindow.DrawText(text2, x + (width / 2) - (12 * text2.Length) / 2, y + (height / 2) + 7, 9, StringAlignment.Center, 60, 30);
        }
    }
}
