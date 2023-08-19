using DisplayImages.source.Patches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DisplayImages.source.UserInterface.DropdownMenu;

namespace DisplayImages.source.UserInterface
{
    
    [StructLayout(LayoutKind.Sequential)]
    public struct MESSAGE
    {
        public IntPtr hWnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point pt;
    }

    public class DIWindow
    {      
        const int WS_OVERLAPPED = 0x00000000;
        const int WS_SYSMENU = 0x00080000;
        const int WS_CAPTION = 0x00C00000;
        const int WS_MINIMIZEBOX = 0x00020000;
        const int WS_MAXIMIZEBOX = 0x00010000;
        const int WS_VISIBLE = 0x10000000;

        const int WM_LBUTTONDOWN = 161;
        const int WM_LBUTTONRELEASE = 162;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOZORDER = 0x0004;
        const int WM_CLOSE = 20;
        const int WE_DROPPED_FILE = 563;
        const int WM_SETICON = 0x0080;
        const int ICON_SMALL = 0;
        const int ICON_BIG = 1;
        const int WS_EX_ACCEPTFILES = 0x00000010;

        public static int x = 300;
        public static int y = 300;
        static int width = 700;
        static int height = 450;

        const int SW_SHOWNORMAL = 1;

        public static Graphics g;

        public static Color bg;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowEx(
            int dwExStyle,
            string lpClassName,
            string lpWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam
        );

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern bool GetMessage(out MESSAGE lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("shell32.dll")]
        static extern uint DragQueryFile(IntPtr hDrop, uint iFile, StringBuilder lpszFile, uint cch);

        [DllImport("shell32.dll")]
        static extern void DragFinish(IntPtr hDrop);

        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWindow);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int x, int y);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        public static string filePath = "";

        public static List<DIComponent> totalComponents = new List<DIComponent>();

        private static DIButton[] sizeButtons = new DIButton[4];

        private static Queue<Action> messageQueue = new Queue<Action>();

        public static event SimpleEvent OnNewImage;
        public static bool leftMouseUp = false;

        private static int errorCounter = 0;
        public static Pen BlackPen
        {
            get
            {
                return new Pen(new SolidBrush(Color.Black));
            }
        }
        public static IntPtr globalWindowHandle;

        [STAThread]
        public unsafe static void Entry()
        {
            if (DuckGame.Content.path.Contains("DuckGameRebuilt"))
            {
                Application.Run(new DGR.DIMenu());
                return;
            }

        Retry:

            bool barClicked = false;
            bg = Color.Gray;

            // main window handle from windows API
            IntPtr hWnd = CreateWindowEx(
                WS_EX_ACCEPTFILES,
                "STATIC",
                "Menu",
                WS_OVERLAPPED | WS_SYSMENU | WS_CAPTION | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_VISIBLE,
                x,
                y,
                width,
                height,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            );

            InitializeIcon(hWnd);

            ShowWindow(hWnd, SW_SHOWNORMAL);

            // only draw after this
            g = Graphics.FromHwnd(hWnd);
            g.FillRectangle(new SolidBrush(bg), 0, 0, width, height); // background

            // initialize components
            InitializeComponents();

            if(errorCounter == 5)
            {
                DisplayImages.Info("can not open DI Menu window !");
                DestroyWindow(hWnd);
                return;
            }
            if (CheckError(hWnd))
            {
                DisplayImages.Info("error occurred with DI Menu");
                DestroyWindow(hWnd);
                errorCounter++;
                goto Retry;
            }
            globalWindowHandle = hWnd;

            DrawOff();

            // main loop to handle events
            // wParam = 8 -> hovering over minimize Button
            // wParam = 9 -> hovering over maximize Button
            // wParam = 20 -> hovering over exit Button
            while (GetMessage(out MESSAGE msg, IntPtr.Zero, 0, 0))
            {
                Thread.Sleep(16);  // ~60 fps 

                if (GetActiveWindow() != hWnd)
                    continue;
    
                foreach (DIComponent component in totalComponents)
                {
                    component.windowX = x + 10;
                    component.windowY = y + 30;
                    component.Update(msg);
                }
                // DuckGame.DevConsole.Log("message " + msg.message.ToString() + " x " + msg.pt.X.ToString() + " y " + msg.pt.Y.ToString() + " wParam: " + msg.wParam.ToString() + " lParam: " + msg.lParam.ToString());
                if (msg.message == WE_DROPPED_FILE)
                {
                    IntPtr handleDrop = msg.wParam;
                    HandleDroppedFiles(handleDrop);
                }

                if (msg.wParam.ToInt32() == WM_CLOSE && msg.message == WM_LBUTTONDOWN)
                {
                    DisplayImages.Info("exit DI Menu");
                    DestroyWindow(hWnd);
                    break;
                }

                barClicked = msg.message == WM_LBUTTONDOWN || barClicked;
                
                UpdateWindowPos(hWnd, msg, &barClicked);
            }
        }

        public static void DrawOn()
        {
            Font drawFont = new Font("Arial", 15);
            SolidBrush drawBrush = new SolidBrush(Color.Green);  // text color

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;                  // left centered -  default

            RectangleF drawRect = new RectangleF(20, 40, 60, 60);

            //g.DrawRectangle(BlackPen, new Rectangle(x , y, width, height));
            g.DrawString("ON", drawFont, drawBrush, drawRect, drawFormat);
        }
        public static void DrawOff()
        {
            Font drawFont = new Font("Arial", 15);
            SolidBrush drawBrush = new SolidBrush(Color.Red);  // text color

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;                  // left centered -  default

            RectangleF drawRect = new RectangleF(10, 30, 80, 60);

            //g.DrawRectangle(BlackPen, new Rectangle(x , y, width, height));
            g.DrawString("OFF", drawFont, drawBrush, drawRect, drawFormat);
        }
        public static void ChangeMode()
        {
            g.FillRectangle(new SolidBrush(bg), new Rectangle(10, 10, 80, 100));

            if (BitImageUpdater.bitmapMode)
                DrawOn();
            else
                DrawOff();
        }

        public static void InitializeComponents()
        {
            DrawText("System File Path:", 100, 30, 10);

            DIButton resize = new DIButton(g, "resize", 300, 130, 60, 30);

            #region DropdownMenu resize
            List<DMButton> buttonsResize = new List<DMButton>();
            buttonsResize.Add(new DMButton("32x32", delegate {

                if (filePath != "" && filePath != "please choose a valid image !")
                    CSImageHandler.ResizeImage(GetImageName(filePath, filePath.Length - 1), filePath, "s");

            }));
            buttonsResize.Add(new DMButton("64x64", delegate {

                if (filePath != "" && filePath != "please choose a valid image !")
                    CSImageHandler.ResizeImage(GetImageName(filePath, filePath.Length - 1), filePath, "b");

            }));
            buttonsResize.Add(new DMButton("96x96", delegate {

                if (filePath != "" && filePath != "please choose a valid image !")
                    CSImageHandler.ResizeImage(GetImageName(filePath, filePath.Length - 1), filePath, "h");

            }));
            buttonsResize.Add(new DMButton("to grid", delegate {

                if (filePath != "" && filePath != "please choose a valid image !")
                    CSImageHandler.ResizeToGrid(GetImageName(filePath, filePath.Length - 1), filePath, 0, 0, "toGrid");

            }));

            DropdownMenu subMenu = new DropdownMenu(g, buttonsResize, resize);
            #endregion

            totalComponents.Add(resize);
            totalComponents.Add(subMenu);

            DIButton split = new DIButton(g, "split", 380, 130, 60, 30);

            #region DropdownMenu split
            List<DMButton> buttonsSplit = new List<DMButton>();
            buttonsSplit.Add(new DMButton("2x2", delegate
            {
                CSImageHandler.SplitImage( 2, 2, GetImageName(filePath, filePath.Length - 1));
            }));
            buttonsSplit.Add(new DMButton("3x3", delegate
            {
                CSImageHandler.SplitImage( 3, 3, GetImageName(filePath, filePath.Length - 1));
            }));

            DropdownMenu subMenuSplit = new DropdownMenu(g, buttonsSplit, split);
            #endregion

            totalComponents.Add(split);
            totalComponents.Add(subMenuSplit);

            int delta = 30 + 20;

            DIButton small = new DIButton(g, "small", 180, 130, 60, 30);
            small.clickAction = delegate
            {
                if (filePath != "" && filePath != "please choose a valid image !")
                    DuckGame.DevConsole.RunCommand("bitmap " + GetImageName(filePath, filePath.Length - 1) + " 1");
            };
            small.imageAction = delegate
            {
                small.Draw();
                small.DrawText();
                Rectangle rect = new Rectangle(small.x, small.y, small.width, small.height);
                if (File.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/1/" + GetImageName(filePath, filePath.Length - 1) + ".png"))
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentGreen.png"), rect);
                else
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentRed.png"), rect);
            };
            totalComponents.Add(small);
            sizeButtons[0] = small;

            DIButton big = new DIButton(g, "big", 180, 130 + delta, 60, 30);
            big.clickAction = delegate
            {
                if (filePath != "" && filePath != "please choose a valid image !")
                    DuckGame.DevConsole.RunCommand("bitmap " + GetImageName(filePath, filePath.Length - 1) + " 2");
            };
            big.imageAction = delegate
            {
                big.Draw();
                big.DrawText();
                Rectangle rect = new Rectangle(big.x, big.y, big.width, big.height);
                if (Directory.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/4/" + GetImageName(filePath, filePath.Length - 1)))
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentGreen.png"), rect);
                else
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentRed.png"), rect);
            };
            totalComponents.Add(big);
            sizeButtons[1] = big;

            DIButton huge = new DIButton(g, "huge", 180, 130 + delta * 2, 60, 30);
            huge.clickAction = delegate
            {
                if (filePath != "" && filePath != "please choose a valid image !")
                    DuckGame.DevConsole.RunCommand("bitmap " + GetImageName(filePath, filePath.Length - 1) + " 3");
            };
            huge.imageAction = delegate
            {
                huge.Draw();
                huge.DrawText();
                Rectangle rect = new Rectangle(huge.x, huge.y, huge.width, huge.height);
                if (Directory.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/9/" + GetImageName(filePath, filePath.Length - 1)))
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentGreen.png"), rect);
                else
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentRed.png"), rect);
            };
            totalComponents.Add(huge);
            sizeButtons[2] = huge;

            DIButton custom = new DIButton(g, "custom", 180, 130 + delta * 3, 60, 30);
            custom.clickAction = delegate
            {
                if (filePath != "" && filePath != "please choose a valid image !")
                    DuckGame.DevConsole.RunCommand("bitmap " + GetImageName(filePath, filePath.Length - 1));
            };
            custom.imageAction = delegate
            {
                custom.Draw();
                custom.DrawText();
                Rectangle rect = new Rectangle(custom.x, custom.y, custom.width, custom.height);
                if (File.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/CustomSize/" + GetImageName(filePath, filePath.Length - 1) + ".png"))
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentGreen.png"), rect);
                else
                    g.DrawImage(new Bitmap(DisplayImages.GetModPath.directory + "/content/transparentRed.png"), rect);
            };
            totalComponents.Add(custom);
            sizeButtons[3] = custom;

            DITextBox tBox1 = new DITextBox(g, "filePath", 100, 60, 500, 30);
            totalComponents.Add(tBox1);

            DIImage brwsImg = new DIImage(new Bitmap(DisplayImages.GetModPath.contentDirectory + "/browse.png"), 610, 50, 50, 50);
            DIButton browseBtn = new DIButton(g, "browser", 610, 60, 50, 30, brwsImg);
            bool isOpened = false;
            browseBtn.clickAction = delegate
            {
                if (!isOpened) isOpened = true;
                    else return;

                Thread fileBrowserThread = new Thread(OpenDialog);
                fileBrowserThread.SetApartmentState(ApartmentState.STA);
                fileBrowserThread.Start();

                void OpenDialog()
                {
                    OpenFileDialog browse = new OpenFileDialog();
                    if (browse.ShowDialog() == DialogResult.OK)
                    {
                        filePath = browse.FileName;
                        DisplayImages.Info("finished filedialog");
                        OnNewImage?.Invoke();
                    }

                    isOpened = false;
                }
            };  
            totalComponents.Add(browseBtn);

            DIButton toggleMode = new DIButton(g, "ON/OFF", 560, 300, 70, 60);
            toggleMode.clickAction = delegate
            {
                BitImageUpdater.bitmapMode = !BitImageUpdater.bitmapMode;
                ChangeMode();
            };
            totalComponents.Add(toggleMode);

            ProgressBar pB = new ProgressBar(g, 280, 310, 200, 40);
            totalComponents.Add(pB);

            DIImageBox imgBox = new DIImageBox(g, "showImage", 30, 130, 130, 130);
            totalComponents.Add(imgBox);
        }
        /// <summary>
        /// get image name from filepath, independent on file extension
        /// </summary>
        /// <param name="path"></param>
        /// <param name="depth"></param>
        /// <param name="trueCompare"></param>
        /// <returns></returns>
        public static string GetImageName(string path, int depth, bool trueCompare = false)
        {
            if (path[depth] == '.')     // switch recursion mode - collect letters
                return GetImageName(path, depth - 1, true);

            if (path[depth] == '\\')    // recursion break
            {
                folderPoint = depth;
                return "";
            }   

            if (trueCompare)
                return GetImageName(path, depth - 1, true) + path[depth];
            else
                return GetImageName(path, depth - 1);
        }
        private static void HandleDroppedFiles(IntPtr hDrop)
        {
            // get the length of the file path
            uint filePathLength = DragQueryFile(hDrop, 0, null, 0);

            // accomondate characters into a string
            StringBuilder fileName = new StringBuilder(260);

            // get the file path
            DragQueryFile(hDrop, 0, fileName, filePathLength + 1);

            Console.WriteLine("Dropped File #1: ");
            Console.WriteLine(fileName);
            filePath = fileName.ToString();

            DragFinish(hDrop);
        }

        static bool firstOffset = true;
        static int xOffset = -1000;
        static int yOffset = -1000;
        private static int folderPoint;

        // you could also use the memory reference to barClicked via the ref keyword in a safe context
        static unsafe void UpdateWindowPos(IntPtr hWnd, MESSAGE msg, bool* barClicked)
        {
            // get/set the value of the variable by dereferencing the pointer

            if (*barClicked && msg.message == WM_LBUTTONRELEASE)
            {
                *barClicked = false;
                ResetOffset();
            }

            // drag window Feature
            if (*barClicked)
            {
                if (firstOffset)
                {
                    GetFirstOffset(out xOffset, out yOffset, hWnd, msg);
                    firstOffset = false;
                }

                //  Console.WriteLine("xOff: " + xOffset + "--->" + msg.pt.X + "==" + (msg.pt.X - xOffset));
                //  Console.WriteLine("yOff: " + yOffset + "--->" + msg.pt.Y + "==" + (msg.pt.Y - yOffset));
                int newX = msg.pt.X - xOffset;
                int newY = msg.pt.Y - yOffset;
                SetWindowPos(hWnd, IntPtr.Zero, newX, newY, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                //SendMessage(hWnd, 0x0010, IntPtr.Zero, IntPtr.Zero);
                x = newX;
                y = newY;
            }
        }
        /// <summary>
        /// only gets called one time after the top bar has been clicked
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="hWnd">window pointer</param>
        /// <param name="msg">window events</param>
        static void GetFirstOffset(out int xOffset, out int yOffset, IntPtr hWnd, MESSAGE msg)
        {
            Rectangle rect;
            int xPos = 0;
            int yPos = 0;

            if (GetWindowRect(hWnd, out rect))
            {
                xPos = rect.X;
                yPos = rect.Y;
                //Console.WriteLine("top: " + rect.Top + "  " + "left: " + rect.Left + "  " + "bottom: " + rect.Bottom + "   " + "right: " + rect.Right);
            }

            xOffset = msg.pt.X - xPos;
            yOffset = msg.pt.Y - yPos;
        }
        static void ResetOffset()
        {
            xOffset = -1000;
            yOffset = -1000;
            firstOffset = true;
        }
        static void InitializeIcon(IntPtr hWnd)
        {
            Icon icon = new Icon(DisplayImages.GetModPath.directory + "/content/DIMenu.ico");
            IntPtr hIcon = icon.Handle;
            SendMessage(hWnd, WM_SETICON, (IntPtr)ICON_SMALL, hIcon);
            SendMessage(hWnd, WM_SETICON, (IntPtr)ICON_BIG, hIcon);
        }

        public static void DrawText(string text, int x, int y, int size, StringAlignment preference = StringAlignment.Near, int width = -1, int height = -1)
        {
            Font drawFont = new Font("Arial", size);
            SolidBrush drawBrush = new SolidBrush(Color.Black);  // text color

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = preference;                  // left centered -  default

            if (width == -1)
                width = size * text.Length;
            if (height == -1)
                height = size + 8;
            RectangleF drawRect = new RectangleF(x, y, width, height);

            //g.DrawRectangle(BlackPen, new Rectangle(x , y, width, height));
            g.DrawString(text, drawFont, drawBrush, drawRect, drawFormat);
        }
        public static bool CheckError(IntPtr hWnd)
        {
            IntPtr hdc = GetDC(hWnd);
            uint COLORREF = GetPixel(hdc, 10, 10);

            // colorref -> 0x00BBGGRR
            // e.g Gray background color: raw GetPixel Value is 8 421 504, 0
            // this number is 1000 0000 1000 0000 1000 0000 in binary, the first bits/bytes are counted from the right 
            // for the r value we cast this big number to a byte data type in order to get the value of the first 8 bits (first 2x4 bit blocks on the right), since a byte has the range from 0-255, and get the number 128
            // when bitshifting this big number 8 bits to the right, the first 8 bits will be gone and there will be left this: 1000 0000 1000 0000 - casting this into a byte and get the number 128 again
            // bitshifting 16 bits to the right and we will get the number 1000 0000 wich we dont need to cast into a byte because it already is cut into 8 bits
            // GetPixel can hold values from 0 (black) - 16 777 215 (white wich is 1111 1111 1111 1111 1111 1111 in binary or 255,255,255 in RGB) 

            int r = (byte)COLORREF;
            int g = (byte)(COLORREF >> 8);
            uint b = COLORREF >> 16;

            if (r == bg.R && g == bg.G && b == bg.B)
            {
                DisplayImages.Info("started with no errors!");
                return false;
            }
            else
                return true;
        }
    }
}
