using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using DuckGame;
using Microsoft.Xna.Framework;
using System.IO;
using HarmonyLib;
using BetterCommunication;
using DisplayImages.source.UserInterface;
using System.Threading.Tasks;
using System.Threading;

namespace DisplayImages
{
    public class DisplayImages : DisabledMod
    {
        private static DisplayImages instance;

        public static Form form;
        public static TextBox textBox;

        public static event SimpleEvent OnModeChange;
        public static ModConfiguration GetModPath
        {
            get { return instance.configuration; }
        }        
        public override DuckGame.Priority priority
        {
            get { return base.priority; }
        }
        protected override void OnPreInitialize()
        {
            instance = this;
            base.OnPreInitialize();
            Resolver.ResolveDependencies();
        }
        protected override void OnPostInitialize()
        {
            // Inject BitMap Class into Xna Framework in DuckGame (MonoMain.Instance)
            FieldInfo getInjection = typeof(Game).GetField("updateableComponents", BindingFlags.Instance | BindingFlags.NonPublic);
            List<IUpdateable> addUpdateble = getInjection.GetValue(MonoMain.instance) as List<IUpdateable>;  
            addUpdateble.Add(new BitImageUpdater()); 

            base.OnPostInitialize();

            new Harmony("displayImages").PatchAll();

            OnModeChange += DIWindow.ChangeMode;
                                             
            CMD bootImage = new CMD("bitmap", new CMD.Argument[] { new CMD.String("image", true), new CMD.Integer("grid1", true), new CMD.Integer("grid2", true) } , delegate(CMD command)
            {
                int columns = command.Arg<int>("grid1");
                int rows = command.Arg<int>("grid2");
                string name = command.Arg<String>("image");
                
                if(rows == default(int) && columns == default(int) && name != default(string))
                {
                    BitImageUpdater.sprites.Clear();
                    BitImageUpdater.hatObjects.Clear();
                    BitImageUpdater.loaded = false;
                    BitImageUpdater.imageName = name;
                    BitImageUpdater.grid1 = -1; //custom size
                    BitImageUpdater.grid2 = -1;
                }

                if(rows == default(int))
                {
                    rows = columns;
                }
                if (name != default(string) && BitImageUpdater.bitmapMode)  // change image
                {
                    BitImageUpdater.sprites.Clear();
                    BitImageUpdater.hatObjects.Clear();
                    BitImageUpdater.loaded = false;
                    BitImageUpdater.imageName = name;
                    BitImageUpdater.grid1 = columns;
                    BitImageUpdater.grid2 = rows;
                }
                if (name != default(string) && !BitImageUpdater.bitmapMode) // start bitmap mode
                {
                    BitImageUpdater.bitmapMode = true;
                    BitImageUpdater.imageName = name;
                    BitImageUpdater.grid1 = columns;
                    BitImageUpdater.grid2 = rows;
                }
                if (name == default(string) && BitImageUpdater.bitmapMode) 
                {
                    BitImageUpdater.bitmapMode = false;
                    BitImageUpdater.sprites.Clear();
                    BitImageUpdater.hatObjects.Clear();
                }
                OnModeChange?.Invoke();

                DevConsole.Log(DCSection.Mod, string.Concat("|ORANGE| BitImage", BitImageUpdater.bitmapMode ? "|GREEN| on" : "|RED| off"));
            });
            CMD addPNGImageAsHat= new CMD("hat", new CMD.Argument[] { new CMD.String("image", false) }, delegate (CMD command)
            {
                Profile profile = BitImageUpdater.LocalUser;
                string name = command.Arg<string>("image");
                string path = GetModPath.directory + "/content/png Hats/" + name + ".png";

                Team sprite = null;
                if (File.Exists(path))
                {
                    sprite = Team.Deserialize(path);
                }
                else
                {
                    DevConsole.Log(DCSection.Mod, "|ORANGE|image not found in 'content/png Hats'");
                    return;
                }
                TeamHat teamHat = new TeamHat( 0, 0, sprite);

                BitImageUpdater.addedHats.Add(teamHat);
                BitImageUpdater.extraTeams.Add(sprite);

                Level.Add(teamHat);
                Teams.AddExtraTeam(sprite);
                profile.duck.Equip(teamHat);

                DevConsole.Log(DCSection.Mod, "|GREEN|added Hat", -1);
            });
            CMD clearImage = new CMD("cls", delegate (CMD command)
            {
                foreach (TeamHat teamHat in BitImageUpdater.addedHats)
                {
                    Level.Remove(teamHat);
                }
                foreach (Team team in BitImageUpdater.extraTeams)
                {
                    Teams.core.extraTeams.Remove(team);
                }
            });
            CMD cursorShow = new CMD("show", delegate (CMD command)
            {
                Cursor.Show();
            });
            CMD cursorHide = new CMD("hide", delegate (CMD command)
            {
                Cursor.Hide();               
            });
            
            CMD splitImage = new CMD("split", new CMD.Argument[] { new CMD.String("image", false), new CMD.Integer("grid", false), new CMD.Integer("optionalGrid", true) }, delegate (CMD command)
            {
                string imageName = command.Arg<String>("image");
                int grid = command.Arg<int>("grid");
                int optionalGrid = command.Arg<int>("optionalGrid");

                if ((grid == 3 || grid == 2) && optionalGrid == default(int))
                {
                    CSImageHandler.SplitImage( grid, grid, imageName);
                }
                else if (optionalGrid == default(int))
                {
                    DevConsole.Log(DCSection.Mod, "|ORANGE|specify the Grid (2 or 3) -> 2x2, 3x3");
                }
                else if (optionalGrid != default(int))
                {
                    CSImageHandler.SplitImage( grid, optionalGrid, imageName);
                }
            });
            
            CMD resizeImage = new CMD("resize", new CMD.Argument[] { new CMD.String("image", false), new CMD.String("format", true), new CMD.Integer("width", true), new CMD.Integer("height", true) }, delegate (CMD command)
            {
                string imageName = command.Arg<String>("image");
                string format = command.Arg<String>("format");
                int width = command.Arg<int>("width");
                int height = command.Arg<int>("height");
                string path = GetModPath.directory + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png";

                if (format != "p" && width != default(int) && height == default(int))  // example: resize image 7 6
                {
                    int newWidth = Int32.Parse(format);
                    CSImageHandler.ResizeToGrid(imageName, path , newWidth, width);

                    CSImageHandler.SplitImage( newWidth, width, imageName);
                    return;
                }
                if (format == default(string) && width == default(int) && height == default(int))  // example: resize image 
                {
                    CSImageHandler.ResizeToGrid(imageName, path, 0, 0, "toGrid");
                    return;
                }
                int grid = -1;
                if (File.Exists(path)) 
                {
                    
                    if (format != "p") // for basic formats
                    {
                        CSImageHandler.ResizeImage(imageName, path, format);
                        switch (format)
                        {
                            case "b": grid = 2; break;
                            case "h": grid = 3; break;
                        }
                        if (grid != -1) CSImageHandler.SplitImage( grid, grid, imageName);
                    }
                    else // example resize image p x y
                    {
                        CSImageHandler.ResizeImage(imageName, path, format, width, height);
                    }
                }
                else
                {
                    DevConsole.Log(DCSection.Mod, "|ORANGE| image not found");
                }                                    
            });
            CMD cropImage = new CMD("crop", new CMD.Argument[] { new CMD.String("image", false), new CMD.String("format", true) }, delegate (CMD command)
            {
                string imageName = command.Arg<String>("image");
                string format = command.Arg<String>("format");

                if (format.Contains(":"))
                {
                    CSImageHandler.CutOutByFormat(imageName, format);
                }
                else
                {
                    DevConsole.Log("|ORANGE| wrong format (examples: 1:2, 2:1, 16:9)");
                }
            });

            Cursor.Show();
            DevConsole.AddCommand(cursorShow);
            DevConsole.AddCommand(cursorHide);
            DevConsole.AddCommand(clearImage);
            DevConsole.AddCommand(bootImage);
            DevConsole.AddCommand(splitImage);
            DevConsole.AddCommand(resizeImage);
            DevConsole.AddCommand(cropImage);
            DevConsole.AddCommand(addPNGImageAsHat);
        }
        public static void Info(string text)
        {
            DevConsole.Log("|PURPLE|DisplayImages    |ORANGE|" + text);
        }
    }
}
