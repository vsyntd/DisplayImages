using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using DuckGame;
using Microsoft.Xna.Framework;
using System.IO;

namespace DisplayImages
{
    public class DisplayImages : DisabledMod
    {
        private static DisplayImages instance;
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
        }
        protected override void OnPostInitialize()
        {
            // Inject BitMap Class into Xna Framework in DuckGame (MonoMain.Instance)
            FieldInfo getInjection = typeof(Game).GetField("updateableComponents", BindingFlags.Instance | BindingFlags.NonPublic);
            List<IUpdateable> addUpdateble = (getInjection.GetValue(MonoMain.instance) as List<IUpdateable>);  
            addUpdateble.Add(new BitImage());
            // overwrite new content
            getInjection.SetValue(MonoMain.instance, addUpdateble);

            base.OnPostInitialize();
                                             
            CMD bootImage = new CMD("bitmap", new CMD.Argument[] { new CMD.String("image", true), new CMD.Integer("gridLayout", true) } , delegate(CMD command)
            {
                int layout = command.Arg<int>("gridLayout");
                string name = command.Arg<String>("image");

                BitImage.bitmapMode = !BitImage.bitmapMode;                

                if (layout == default(int)) layout = 1;

                if (name != default(string) && !BitImage.bitmapMode)
                {
                    BitImage.bitmapMode = true;
                    BitImage.loaded = false;
                }
                BitImage.imageName = name;
                BitImage.grid = layout;

                if (BitImage.bitmapMode) DevConsole.Log(DCSection.Mod, "|ORANGE| BitImage |GREEN| on", -1);
                if (!BitImage.bitmapMode) DevConsole.Log(DCSection.Mod, "|ORANGE| BitImage |RED| off", -1);
            });
            CMD clearImage = new CMD("clear", delegate (CMD command)
            {
                foreach (TeamHat teamHat in BitImage.addedHats)
                {
                    Level.Remove(teamHat);
                }
                foreach (Team team in BitImage.extraTeams)
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
                    CSImageHandler.SplitImage(GetModPath.directory, grid, grid, imageName);
                }
                else if(optionalGrid == default(int)) 
                {
                    DevConsole.Log(DCSection.Mod, "|ORANGE|specify the Grid (2 or 3) -> 2x2, 3x3");
                }
                else if(optionalGrid != default(int))
                {
                    CSImageHandler.SplitImage(GetModPath.directory, grid, optionalGrid, imageName);
                }
            });
            CMD resizeImage = new CMD("resize", new CMD.Argument[] { new CMD.String("image", false), new CMD.String("format", true), new CMD.Integer("width", true), new CMD.Integer("height", true) }, delegate (CMD command)
            {
                string imageName = command.Arg<String>("image");
                string format = command.Arg<String>("format");
                int width = command.Arg<int>("width");
                int height = command.Arg<int>("height");

                if (format == default(string)) format = "s";

                string grid = "1";
                if (File.Exists(GetModPath.directory + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png")) 
                {
                    if(format != "p")CSImageHandler.ResizeImage(imageName, GetModPath.directory, format);
                }
                else
                {
                    DevConsole.Log(DCSection.Mod, "|ORANGE| image not found");
                }   
                if(format == "p" && File.Exists(GetModPath.directory + "/DisplayImages/ImageFiles/rawImage/" + imageName + ".png"))
                {
                    CSImageHandler.ResizeImage(imageName,  GetModPath.directory, format, width, height);
                }                                     
            });

            Cursor.Show();
            DevConsole.AddCommand(cursorShow);
            DevConsole.AddCommand(cursorHide);
            DevConsole.AddCommand(clearImage);
            DevConsole.AddCommand(bootImage);
            DevConsole.AddCommand(splitImage);
            DevConsole.AddCommand(resizeImage);
        }
    }
}
