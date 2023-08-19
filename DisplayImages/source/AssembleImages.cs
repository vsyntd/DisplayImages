using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using DuckGame;

namespace DisplayImages
{
    public class AssembleImages    
    {
        public static List<TeamHat> teamHats = new List<TeamHat>();
        public static List<Team> sprites = new List<Team>();

        public static readonly string indicatorForPatcher = "customImageOfDisplayImages";
        /// <summary>
        /// counts colums in each row. columNum.Count = rows 
        /// </summary>
        public static List<int> columNum = new List<int>();

        public static int width;
        public static int height;
        public static void AssembleCustomImage(string imageName)  // for images in CustomSize that are not 32x32, 64x64, 96x96
        { 
            int row = 1;
            int col = 1;

            MousePosition mouse = new MousePosition();
            string path = DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/CustomSize/" + imageName ;

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!Directory.Exists(path) || dirInfo.GetFiles().Length < 1)
            {
                DevConsole.Log(DCSection.Mod, $"couldnt load images check {imageName} folder in CustomSize");
                return;
            }
            Bitmap source = null;
            if (File.Exists(path + "/" + imageName + "_1_1" + ".png"))
            {
                 source = new Bitmap(path + "/" + imageName + "_1_1" + ".png");   // get real image measurements on one example
            }
            else 
            {
                DevConsole.Log(DCSection.Mod, "missing entry point");
                return;
            }
            for (int y = 0; y < source.Height; y++)
            {
                System.Drawing.Color originalColour = source.GetPixel(0, y);
                if (originalColour.R == 0 &&
                    originalColour.G == 0 &&
                    originalColour.B == 0 &&
                    originalColour.A == 0
                   )
                {
                    height = y;
                    break;
                }
            }
            for (int x = 0; x < source.Width; x++)
            {
                System.Drawing.Color originalColour = source.GetPixel(x, 0);
                if (originalColour.R == 0 &&
                    originalColour.G == 0 &&
                    originalColour.B == 0 &&
                    originalColour.A == 0
                   )
                {
                    width = x;
                    break;
                }
            }
            //load images
            List<int> cols = new List<int>();
            List<int> rows = new List<int>();
            foreach (string file in Directory.GetFiles(path))
            {
                string image = imageName + "_" + row.ToString() + "_" + col.ToString();              

                if ((image + ".png") == Path.GetFileName(file))
                {
                    
                    try
                    {
                        cols.Add(col);
                        Bitmap map = new Bitmap(file);
                        var team = Team.DeserializeFromPNG((byte[])new ImageConverter().ConvertTo(new Bitmap(file), typeof(byte[])), indicatorForPatcher, file);
                        var teamHat = new TeamHat(mouse.xpos + (width * (col - 1)), mouse.ypos + (height * (row - 1)), team);
                        BitImageUpdater.sprites.Add(team);
                        BitImageUpdater.hatObjects.Add(teamHat);
                        BitImageUpdater.extraTeams.Add(team);
                        BitImageUpdater.addedHats.Add(teamHat);
                    }
                    catch(Exception e)
                    {
                        StreamWriter write = new StreamWriter(path + "/DisplayImages/error.txt");
                        DevConsole.Log(DCSection.Mod, "|RED| something went wrong. Check error.txt for more details");
                        write.WriteLine(e.Message);
                        write.Close();
                        return;
                    }
                    col++;
                }
                else if (File.Exists(path + "/" + imageName + "_" + (row + 1).ToString() + "_" + "1" + ".png"))       //check if next row exist
                {
                    string localPath = path + "/" + imageName + "_" + (row + 1).ToString() + "_" + "1" + ".png";
                    try 
                    {
                        rows.Add(row);
                        Bitmap map = new Bitmap(localPath);
                        var team = Team.DeserializeFromPNG((byte[])new ImageConverter().ConvertTo(new Bitmap(localPath), typeof(byte[])), indicatorForPatcher, localPath);
                        var teamHat = new TeamHat(mouse.xpos, mouse.ypos + (height * row), team);
                        BitImageUpdater.sprites.Add(team);
                        BitImageUpdater.hatObjects.Add(teamHat);
                        BitImageUpdater.addedHats.Add(teamHat);
                        BitImageUpdater.extraTeams.Add(team);
                    }
                    catch (Exception e)
                    {
                        StreamWriter write = new StreamWriter(path + "/DisplayImages/error.txt");
                        DevConsole.Log(DCSection.Mod, "|RED| something went wrong. Check error.txt for more details");
                        write.WriteLine(e.Message);
                        write.Close();
                        return;
                    }

                    columNum.Add(col-1);
                    row++;      
                    col = 2;    // go to second image of new row, since the first has already been processed
                }
            }
            rows.Add(row);
            columNum.Add(col); // add the rest colums of the last row

            if(columNum[0] > 9) // all colums have to be the same
            {
                DevConsole.Log(DCSection.Mod, "|ORANGE|image is too big to load. maximum column and row size is 9");
                return;
            }
            //Assamble images
            foreach (Team team in BitImageUpdater.sprites)
            {
                Teams.AddExtraTeam(team);
            }
            foreach(TeamHat team in BitImageUpdater.hatObjects) 
            { 
                Level.Add(team);

                team.velocity = new Vec2(0f, 0f);
                team.hMax = 0f;
                team.vMax = 0f;
                team.gravMultiplier = 0f;
                team.floatMultiplier = 0f;
                team.throwSpeedMultiplier = 0f;
                team.canPickUp = false;
                team.destructive = false;
                team._destroyed = false;
                team.burnSpeed = 0;
                team.onFire = false;
                team.depth = 0;
                team.layer = Layer.Foreground;
                team.enablePhysics = false;
            }
            DevConsole.Log(BitImageUpdater.sprites.Count.ToString());
            DevConsole.Log(BitImageUpdater.hatObjects.Count.ToString());
            DevConsole.Log(cols.Count.ToString());
            DevConsole.Log(rows.Count.ToString());
            DevConsole.Log(DCSection.Mod,"|GREEN|finished");
        }
    }
}
