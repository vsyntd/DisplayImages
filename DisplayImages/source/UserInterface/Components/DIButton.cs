using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source.UserInterface
{
    public class DIButton : DIComponent
    {
        private int clickCounter = 0;
        private bool clickDelay = true;
        public DIImage image = null;
        public Action clickAction = null;
        public Action imageAction = null;
        private bool switch_1;

        public event SimpleEvent DrawDefault;
        /// <summary>
        /// send invokes to related components - if any (e.g DropdownMenu)
        /// </summary>
        public event SimpleEvent OnClickGLobal;
        public DIButton(Graphics g, string name, int x, int y, int width, int height)
        {
            base.name = name;
            base.g = g;
            base.x = x;
            base.y = y;
            base.width = width;
            base.height = height;

            OnHoveringEvent += DrawHovering;
            OnHoveringRelease += Draw;
            DIWindow.OnNewImage += ExcecuteImageAction;

            DrawDefault += DrawText;
            OnHoveringEvent += DrawText;
            OnHoveringRelease += DrawText;

            DrawText();
        }
        public DIButton(Graphics g, string name, int x, int y, int width, int height, DIImage image)
        {
            base.g = g;
            base.name = name;
            base.x = x;
            base.y = y;
            base.width = width;
            base.height = height;
            this.image = image;

            OnHoveringEvent += DrawHovering;
            OnHoveringRelease += Draw;

        }
        public void DrawHovering()
        {
            Console.WriteLine("hovering event " + name);
            Rectangle browseRect = new Rectangle(x, y, width, height);
            g.FillRectangle(new SolidBrush(Color.DarkGray), browseRect);
            g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), browseRect);

            if (image != null)
            {
                g.DrawImage(image.img, image.imgRect.X, image.imgRect.Y, image.imgRect.Width, image.imgRect.Height);
            }
        }
        public void Draw()
        {
            Rectangle browseRect = new Rectangle(x, y, width, height);
            g.FillRectangle(new SolidBrush(Color.AntiqueWhite), browseRect);
            g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), browseRect);

            if (image != null)
            {
                g.DrawImage(image.img, image.imgRect.X, image.imgRect.Y, image.imgRect.Width, image.imgRect.Height);
            } 
        }
        private void ExcecuteImageAction()
        {
            switch_1 = true;
        }
        public override void Update(MESSAGE msg)
        {
            base.Update(msg);

            if (switch_1)
            {
                imageAction?.Invoke();
                switch_1 = false;
            }

            if (clickDelay)
            {
                clickCounter++;
                if (clickCounter == 5)
                {
                    Draw();
                    DrawDefault?.Invoke();
                    clickCounter = 0;
                    clickDelay = false;
                }
            }
        }
        public override void OnClick()
        {
            OnClickGLobal?.Invoke();

            Rectangle browseRect = new Rectangle(x, y, width, height);
            g.FillRectangle(new SolidBrush(DIWindow.bg), browseRect);
            g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), browseRect);

            clickDelay = true;

            if(clickAction != null)
                clickAction.Invoke();
            
        }
        public void DrawText()
        {
            DIWindow.DrawText(name, x,  height < 40 ? y + 5 : y + ((height / 2) - 10), 9, StringAlignment.Center, width, height);
        }
    }
}
