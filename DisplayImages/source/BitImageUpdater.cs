using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DuckGame;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Linq;
using DisplayImages.source.UserInterface;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DisplayImages 
{
    internal class BitImageUpdater : IUpdateable
    {
        public const int width = 32;

        public const int height = 32;

        public static event SimpleEvent OnProgress;

        public static List<Team> sprites = new List<Team>();
        public static List<TeamHat> hatObjects = new List<TeamHat>();
        public static List<TeamHat> addedHats = new List<TeamHat>();
        public static List<Team> extraTeams = new List<Team>();

        public static bool imageDownPress = true;
        public static bool loaded;
        public static bool windowOpen = false;
        private static int loadCounter = 0;

        internal static string imageName;
        internal static bool bitmapMode = false;
        
        internal static bool updateAssambledImage = false;
        private int duckGameology;
        /// <summary>
        /// used to display 4-phase progress
        /// </summary>
        public static byte progress;

        public static string path;

        [DllImport("user32.dll")]
        static extern bool IsWindow(IntPtr hWnd);

        #region mandatory Attributes for IUpdateable

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool Enabled
        {
            get
            {
                return true;
            }
        }

        public int UpdateOrder
        {
            get
            {
                return 1;
            }
        }
        #endregion
        public static Profile LocalUser
        {
            get
            {
                if (!Network.isActive)
                {
                    return Profiles.DefaultPlayer1;
                }
                else return DuckNetwork.localProfile;
            }
        }
        public void Update(GameTime gameTime)
        {
            MousePosition mouse = new MousePosition();

            if (Keyboard.Pressed(Keys.F7) && !IsWindow(DIWindow.globalWindowHandle))
            {
                DisplayImages.Info("new window");
                Task.Run(delegate { DIWindow.Entry(); });
            }

            if (bitmapMode)
            {
                if (Mouse.left == InputState.Down && imageName != default(string))
                {
                    if (imageDownPress)
                    {
                        loaded = LoadImages(mouse.xpos, mouse.ypos);
                        imageDownPress = false;
                    }
                }
                if (!imageDownPress)
                {
                    loadCounter++;
                }
                bool mandatoryWaitingTime = !imageDownPress && loadCounter > 120;
                if (Mouse.left == InputState.None && imageName != default(string) && mandatoryWaitingTime)
                {
                    AssembleImages.columNum.Clear();
                    hatObjects.Clear();
                    sprites.Clear();
                    duckGameology = 0;
                    loadCounter = 0;
                    imageDownPress = true;
                }

                bool isNotEmpty = hatObjects.Any();
                if (isNotEmpty && loaded)
                {
                    foreach (TeamHat hat in hatObjects)
                    {
                        hat.velocity = new Vec2(0f, 0f);
                        hat.hMax = 0f;
                        hat.vMax = 0f;
                        hat.gravMultiplier = 0f;
                        hat.floatMultiplier = 0f;
                        hat.throwSpeedMultiplier = 0f;
                        hat.canPickUp = false;
                        hat.destructive = false;
                        hat._destroyed = false;
                        hat.burnSpeed = 0;
                        hat.onFire = false;
                        hat.depth = 0;
                        hat.layer = Layer.Foreground;
                        hat.enablePhysics = false;
                    }

                    SendImagesToNetwork();

                    if(Mouse.left == InputState.Down)
                        UpdateImages(mouse);
                }
                else if (loaded)
                {
                    // send images occasionally over the Network (every 2 seconds)

                }
                
            }
            if (!bitmapMode)
            {
                imageName = default(string);
                loaded = false;
            }
        }
        internal static int grid1;
        internal static int grid2;

        /// <summary>
        /// updates the position of each Image 
        /// </summary>
        /// <param name="mouse"></param>
        private void UpdateImages(MousePosition mouse)
        {
            int colCounter = 0;
            int rowCounter = 0;

            if (grid1 == grid2 && (grid1 == 1 || grid1 == 2 || grid1 == 3))
            {
                foreach (TeamHat image in hatObjects)
                {
                    Bitmap source = null;
                    if (grid1 != 1)
                    {
                        source = new Bitmap(filename: path + imageName + "/" + imageName + "_" + (colCounter + 1).ToString() + "_" + (rowCounter + 1).ToString() + ".png");
                    }
                    else
                    {
                        source = new Bitmap(filename: path + imageName + ".png");
                    }
                    int width = source.Width;
                    int height = source.Height;

                    image.position.x = mouse.xpos + (width * colCounter);
                    image.position.y = mouse.ypos + (height * rowCounter);

                    if (colCounter < grid1 - 1)
                    {
                        colCounter++;
                    }
                    else
                    {
                        if (rowCounter < grid2 - 1) rowCounter++;  
                        colCounter = 0;
                    }
                }
            }
            else
            {
                
                foreach (TeamHat team in hatObjects)  
                {
                    if(AssembleImages.width == 0)
                    {
                        AssembleImages.width = BitImageUpdater.width;
                    }
                    if (AssembleImages.height == 0)
                    {
                        AssembleImages.height = BitImageUpdater.height;
                    }
                    team.position.x = mouse.xpos + (AssembleImages.width * colCounter);
                    team.position.y = mouse.ypos + (AssembleImages.height * rowCounter);

                    if (colCounter < AssembleImages.columNum[rowCounter] - 1) 
                    {
                        colCounter++;
                    }
                    else
                    {
                        if (rowCounter < AssembleImages.columNum.Count - 1) rowCounter++;  // -1, since indexing starts from 0 and not 1
                        colCounter = 0;
                    }
                }
                
            }
        }
        /// <summary>
        /// Loads images from files and adds them into the Level
        /// </summary>
        /// <param name="xVec">x vector of mouse position</param>
        /// <param name="yVec">y vector of mouse position</param>
        /// <returns>true, if all images are loaded properly, false if not</returns>
        private bool LoadImages(float xVec, float yVec)
        {
            int index = 0;

            string path = DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/";

            if (grid1 == grid2 && (grid1 == 1 || grid1 == 2 || grid1 == 3)) // default assembly
            {
                string folder = (grid1 * grid2).ToString();
                if(grid1 == 1)
                {
                    if(File.Exists(path + folder + "/" + imageName + ".png"))
                    {
                        Bitmap image = new Bitmap(filename: path + folder + "/" + imageName + ".png"); // get image for measurement
                        int width = image.Width;
                        int height = image.Height;

                        Team texture = Team.DeserializeFromPNG((byte[])new ImageConverter().ConvertTo(new Bitmap(path + folder + "/" + imageName + ".png"), typeof(byte[])), AssembleImages.indicatorForPatcher, path + folder + "/" + imageName + ".png");
                        TeamHat obj = new TeamHat(xVec , yVec , texture);

                        sprites.Add(texture);
                        hatObjects.Add(obj);
                        addedHats.Add(obj);
                        extraTeams.Add(texture);

                        Level.Add(obj);
                        Teams.AddExtraTeam(texture);

                        BitImageUpdater.path = DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/" + folder + "/";

                        return true;
                    }
                    else
                    {
                        DevConsole.Log(DCSection.Mod, $"|ORANGE|image is missing in '{folder}' folder");
                        return false;
                    }
                }
                if (Directory.Exists(path + folder + "/" + imageName))
                {
                    int checkCol = 1;
                    int checkRow = 1;
                    foreach (string file in Directory.GetFiles(path + folder + "/" + imageName))
                    {
                        if ((imageName + "_" + checkRow.ToString() + "_" + checkCol.ToString() + ".png") == Path.GetFileName(file))
                        {
                            Bitmap image = new Bitmap(filename: file); // get image for measurement
                            int width = image.Width;
                            int height = image.Height;

                            Team texture = Team.DeserializeFromPNG((byte[])new ImageConverter().ConvertTo(new Bitmap(file), typeof(byte[])), AssembleImages.indicatorForPatcher, file);
                            TeamHat obj = new TeamHat(xVec + (width * (checkCol - 1)), yVec + (height * (checkRow - 1)), texture);
                            
                            sprites.Add(texture);
                            hatObjects.Add(obj);
                            addedHats.Add(obj);
                            extraTeams.Add(texture);

                            Level.Add(obj);
                            Teams.AddExtraTeam(texture);

                            if (checkCol < grid1)
                            {
                                checkCol++;
                            }
                            else if (checkRow < grid2)
                            {
                                checkRow++;
                                checkCol = 1;
                            }
                            index++;
                        }
                        else
                        {
                            DevConsole.Log(DCSection.Mod, $"|ORANGE|some images are missing in '{folder}' folder");
                            bitmapMode = false;
                            return false;
                        }
                    }
                }
                else
                {
                    DevConsole.Log(DCSection.Mod, $"|ORANGE| {imageName} folder is missing");
                    bitmapMode = false;
                    return false;
                }
                BitImageUpdater.path = DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/" + folder + "/"; // update specified path
            }
            else
            {
                AssembleImages.AssembleCustomImage(imageName);  // assamble on mouse click
            }
            return true;   
        }
        private void SendImagesToNetwork()
        {
            if (loaded && duckGameology < 3)
            {
                duckGameology++;
            }
            if (duckGameology == 3)
            {
                foreach (Team team in sprites)
                {
                    NMSpecialHat loadHat = new NMSpecialHat(team, LocalUser);
                    Send.Message(loadHat, null);
                }
                duckGameology = 5;
            }
        }
        public static void UpdateProgress()
        {

            OnProgress?.Invoke();
            progress++;
        }
    }
    public class MousePosition
    {
        public static bool bitMapMode = false;

        public float xpos = Level.current.camera.transformScreenVector(Mouse.mousePos).x;     // get accurate x position 
        public float ypos = Level.current.camera.transformScreenVector(Mouse.mousePos).y;     // get accurate y position 
    }
    
}
