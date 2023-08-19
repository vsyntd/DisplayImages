using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source.UserInterface
{
    internal class ProgressBar : DIComponent
    {
        ushort delayCounter = 0;
        public ProgressBar(Graphics g, int x, int y, int width, int height)
        {
            base.g = g;
            base.x = x;
            base.y = y;
            base.width = width;
            base.height = height;
        } 

        public override void Update(MESSAGE msg)
        {
            base.Update(msg);

            byte num = BitImageUpdater.progress;
            switch (num)
            {
                case 1: Reset();
                    DrawBar(num);
                break;
                case 4:
                    DrawBar(num);
                    delayCounter++;
                    if (delayCounter >= 60 * 1.5f)
                    {
                        Reset();
                        BitImageUpdater.progress = 0;
                        delayCounter = 0;
                    }
                break;
                default: DrawBar(num); break;
            }
        }
        private void Reset()
        {
            g.FillRectangle(new SolidBrush(DIWindow.bg), new Rectangle(x, y, width + 3, height + 3));
        }
        private void DrawBar(byte  length)
        {
            g.FillRectangle(new SolidBrush(Color.Green), new Rectangle(x, y, (width / 4) * length, height));
            g.DrawRectangle(DIWindow.BlackPen, new Rectangle(x, y, width, height));
        }
    }
}
