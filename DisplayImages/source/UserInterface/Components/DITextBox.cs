using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source.UserInterface
{
    public class DITextBox : DIComponent
    {
        public string text;
        private string prevText;
        public bool canWrite;
        private int flashingUnderscore = 0;
        public DITextBox(Graphics g, string name, int x, int y, int width, int height, bool canWrite = false)
        {
            this.canWrite = canWrite;
            base.name = name;
            base.g = g;
            base.x = x;
            base.y = y;
            base.width = width;
            base.height = height;

            g.FillRectangle(new SolidBrush(Color.DarkGray), new RectangleF(x, y, 500, 30));
            g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(x, y, 500, 30));
        }
        public override void Update(MESSAGE msg)
        {
            if (canWrite)
            {
                // future feature
            }
            text = DIWindow.filePath;
            Font drawFont = new Font("Arial", 8);
            SolidBrush drawBrush = new SolidBrush(Color.Black);  // text color

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Near;         // left centered

            RectangleF drawRect = new RectangleF(x, y, 500, 30);

            if (text != "" && text != prevText)
            {
                g.FillRectangle(new SolidBrush(Color.DarkGray), drawRect);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(x, y, 500, 30));
                g.DrawString(text, drawFont, drawBrush, drawRect, drawFormat);
            }
            prevText = text;
        }
    }
}
