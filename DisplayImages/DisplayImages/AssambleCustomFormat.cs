using System;
using System.Collections.Generic;
using System.IO;
using DuckGame;

namespace DisplayImages
{
    public class AssambleCustomFormat      // for images in CustomSize that are not 32x32, 64x64, 128x128
    {
        List<TeamHat> teamHats = new List<TeamHat>();
        List<Team> sprites = new List<Team>();
        public static string path = DisplayImages.GetModPath.directory + "/DisplayImages/";
        //under construction
        public static void Assamble()
        {
            int row = 0;
            int col = 0;    
            foreach (string file in Directory.GetFiles(path + "/ImageFiles/CustomSize/" + BitImage.imageName))
            {
                if ((BitImage.imageName + (row + 1).ToString() + (col+ 1).ToString() + ".png") == Path.GetFileName(file))
                {
                }
            }
        }
    }
}
