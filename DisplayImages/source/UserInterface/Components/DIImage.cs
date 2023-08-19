using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source.UserInterface
{
    public class DIImage
    {
        public Rectangle imgRect;
        public Image img;
        public DIImage(Image image, int x, int y, int width, int height)
        {
            this.img = image;
            imgRect.X = x;
            imgRect.Y = y;
            imgRect.Width = width;
            imgRect.Height = height;
        }
    }
}
