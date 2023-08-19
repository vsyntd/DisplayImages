
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayImages.source.UserInterface
{

    public delegate void SimpleEvent();
    public abstract class DIComponent
    {
        /// <summary>
        /// get keyboard key state from user windows operating system
        /// </summary>
        /// <param name="VirtualKeyPressed"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int VirtualKeyPressed);

        public int windowX = 300 + 10;
        public int windowY = 300 + 30;
        public int x, y, width, height;
        public string name;
        public bool down;
        public bool isClicked;
        public bool isHovering;
        protected bool finishedHovering;

        /// <summary>
        /// execute one time to draw hovering state
        /// </summary>
        public event SimpleEvent OnHoveringEvent;

        /// <summary>
        /// execute one time to draw normal state
        /// </summary>
        public event SimpleEvent OnHoveringRelease;

        public Graphics g;

        /// <param name="msg">struct value to get mouse position based on win32 api</param>
        public virtual void Update(MESSAGE msg)
        {
            if (CursorHovering(msg))
            {
                // gateway for one time execution
                if (!isHovering)
                    // Invoke the method that is subscribed to that event - if the child class has subscribed
                    OnHoveringEvent?.Invoke();

                isHovering = true;
            }
            else
            {
                if (isHovering)
                    OnHoveringRelease?.Invoke();

                isHovering = false;
            }
        }
        private bool CursorHovering(MESSAGE msg)
        {
            int transformX = msg.pt.X - this.windowX;
            int transformY = msg.pt.Y - this.windowY;

            if (transformX > x && transformY > y &&
               transformX < x + width && transformY < y + height
               )
            {
                if (GetAsyncKeyState((int)Win32Keys.LEFT_MOUSE) == (int)Win32KeyState.UP)
                {
                    if (down)
                    {
                        OnRelease();
                    }
                    down = false;
                    isClicked = false;
                }
                else
                {
                    if (!down)
                    {
                        down = true;
                        OnClick();
                        isClicked = true;
                    }
                    else
                    {
                        OnDown();
                    }
                }
                if (this.finishedHovering)      // dont update OnHovering after 
                    return true;

                OnHovering(out bool finishedHovering);
                if (finishedHovering)
                {
                    this.finishedHovering = true;
                }
                return true;
            }
            else if(isClicked)
            {
                isClicked = false;
            }
            finishedHovering = false;
            return false;
        }
        /// <summary>
        /// continuous looping when hovering over components rectangle
        /// </summary>
        /// <param name="finishedHovering">components can dynamically and independently close the loop after an expected event has occurred </param>
        public virtual void OnHovering(out bool finishedHovering)
        {
            finishedHovering = false;
        }
        public virtual void OnClick()
        {
        }
        public virtual void OnDown()
        {
        }
        public virtual void OnRelease()
        {
        }
    }
}
