using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayImages.source.UserInterface
{
    internal class DropdownMenu : DIComponent
    {
        List<DMButton> buttonList = new List<DMButton>();
        DIButton superButton;
        DMButton prevButton;
        private int buttonHeight;
        /// <summary>
        /// changes state when parent button is clicked or an option has been pressed
        /// </summary>
        private bool visible = false;

        int mouseX;
        int mouseY;

        public DropdownMenu(Graphics g, List<DMButton> list, DIButton parentButton)
        {
            base.g = g;
            buttonHeight = 20;
            x = parentButton.x;
            y = parentButton.y + parentButton.height;
            width = parentButton.width;
            height = buttonHeight * list.Count;
            superButton = parentButton;

            int counter = 0;
            foreach (DMButton b in list)
            {
                b.x = x;
                b.y = y + buttonHeight * counter;
                b.width = width;
                b.height = buttonHeight;

                counter++;
            }

            buttonList = list;
            parentButton.OnClickGLobal += OnParentClickEvent;
            OnHoveringRelease += DefaultDraw;
        }
        public void OnParentClickEvent()
        {
            visible = !visible;

            if (!visible)
            {
                ResetDraw();
                return;
            }       
            
            g.DrawRectangle(DIWindow.BlackPen, new Rectangle(x, y, width, height));
            DefaultDraw();
        }
        public override void Update(MESSAGE msg)
        {
            if (!visible)
                return;

            base.Update(msg);

            mouseX = msg.pt.X - 10;
            mouseY = msg.pt.Y - 30;
        }
        public override void OnHovering(out bool finishedHovering)
        {
            DisplayImages.Info("event hovering");
            int transformedXPos = mouseX - DIWindow.x;
            int transformedYPos = mouseY - DIWindow.y;

            foreach (DMButton button in buttonList)
            {
                if (button.x < transformedXPos && transformedXPos < button.x + width &&
                    button.y < transformedYPos && transformedYPos < button.y + buttonHeight)
                {
                    if (isClicked)
                    {
                        visible = false;
                        ResetDraw();
                        superButton.Draw();
                        superButton.DrawText();
                        button.action.Invoke();
                        finishedHovering = true;
                        return;
                    }

                    if (prevButton == button)    
                        continue;

                    Rectangle buttonRect = new Rectangle(button.x, button.y, width, buttonHeight);
                    g.FillRectangle(new SolidBrush(Color.DarkGray), buttonRect);
                    DIWindow.DrawText(button.name, button.x, button.y + 3, 8, StringAlignment.Center, button.width, buttonHeight);
                    g.DrawRectangle(DIWindow.BlackPen, buttonRect);

                    if(prevButton != null)
                        ResetButton(prevButton);

                    prevButton = button;
                }
            }
            finishedHovering = false;
        }
        private void DefaultDraw()
        {
            foreach (DMButton button in buttonList)
            {
                ResetButton(button);
            }
            prevButton = null;
        }
        public void ResetDraw()
        {
            g.FillRectangle(new SolidBrush(DIWindow.bg), new Rectangle(x, y, width + 5, height + 5));
        }
        public void ResetButton(DMButton button)
        {
            Rectangle buttonRect = new Rectangle(button.x, button.y, button.width, buttonHeight);
            g.FillRectangle(new SolidBrush(Color.AliceBlue), buttonRect);
            g.DrawRectangle(DIWindow.BlackPen, buttonRect);

            DIWindow.DrawText(button.name, button.x, button.y + 3, 8, StringAlignment.Center, button.width, buttonHeight);
        }

        public class DMButton
        {
            public string name;
            public Action action;
            public int x, y, width, height;
            public DMButton(string name, Action action)
            {
                this.name = name;
                this.action = action;
            }
        }
    }
}
