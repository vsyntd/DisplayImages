using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DuckGame;
using System.IO;
using System.Reflection;

namespace DisplayImages 
{
    internal class BitImage : IUpdateable
    {
        public const int width = 32;

        public const int height = 32;

        public Team sprite;
        public TeamHat image;
        public TeamHat[] fourGrid = new TeamHat[4];
        public Team[] fourSprites = new Team[4];
        public TeamHat[] nineGrid = new TeamHat[9];
        public Team[] nineSprites = new Team[9];
        public static List<TeamHat> addedHats = new List<TeamHat>();
        public static List<Team> extraTeams = new List<Team>();

        private bool imageDownPress;
        public static bool loaded;

        internal static int grid;
        internal static string imageName;
        internal static bool bitmapMode = false;
        /// <summary>
        /// this variable only exist because you cannot send a Message(NMSpecialHat) while adding the TeamHat to the level on the same frame
        /// </summary>
        private int duckGameology = 0;

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
        private Profile LocalUser
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
        public void Update(GameTime time)
        {
            string path = DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/";

            MousePosition mouse = new MousePosition();

            if (bitmapMode)
            {
                if (Mouse.left == InputState.Down && imageName != default(string))
                {
                    if (grid == 1)
                    {
                        if (imageDownPress)
                        {
                            loaded = LoadImages(1, mouse.xpos, mouse.ypos, path);
                            imageDownPress = false;
                        }
                        if (image != null && loaded) // protection 
                        {
                            image.velocity = new Vec2(0f, 0f);
                            image.hMax = 0f;
                            image.vMax = 0f;
                            image.gravMultiplier = 0f;
                            image.floatMultiplier = 0f;
                            image.throwSpeedMultiplier = 0f;
                            image.canPickUp = false;
                            image.destructive = false;
                            image._destroyed = false;
                            image.burnSpeed = 0;
                            image.onFire = false;
                            image.depth = 0;
                            image.layer = Layer.Foreground;
                        }
                        if (!loaded)
                        {
                            DevConsole.Log(DCSection.Mod, "|ORANGE| image not found", -1);
                            bitmapMode = false;
                            imageName = default(string);
                        }
                        if (bitmapMode)
                        {
                            SendImagesToNetwork(1);

                            UpdateImages(mouse);                            
                        }
                    }

                    if (grid == 4)
                    {
                        if (imageDownPress)
                        {
                            loaded = LoadImages(4, mouse.xpos, mouse.ypos, path);
                            if (!loaded)
                            {
                                bitmapMode = false;
                                imageName = default(string);
                            }
                            if (loaded)
                            {
                                foreach (TeamHat teamHat in fourGrid)
                                {
                                    teamHat.velocity = new Vec2(0f, 0f);
                                    teamHat.hMax = 0f;
                                    teamHat.vMax = 0f;
                                    teamHat.gravMultiplier = 0f;
                                    teamHat.floatMultiplier = 0f;
                                    teamHat.throwSpeedMultiplier = 0f;
                                    teamHat.canPickUp = false;
                                    teamHat.destructive = false;
                                    teamHat._destroyed = false;
                                    teamHat.burnSpeed = 0;
                                    teamHat.onFire = false;
                                    teamHat.depth = 0;
                                    teamHat.layer = Layer.Foreground;
                                    teamHat.enablePhysics = false;
                                }
                                imageDownPress = false;
                            }
                        }
                        if (fourGrid.Length == 4 && loaded)
                        {
                            SendImagesToNetwork(4);

                            UpdateImages(mouse);
                        }
                    }
                    if (grid == 9)
                    {
                        if (imageDownPress)
                        {
                            loaded = LoadImages(9, mouse.xpos, mouse.ypos, path);
                            if (!loaded)
                            {
                                bitmapMode = false;
                                imageName = default(string);
                            }
                            if (loaded)
                            {
                                foreach (TeamHat teamHat in nineGrid)
                                {
                                    teamHat.velocity = new Vec2(0f, 0f);
                                    teamHat.hMax = 0f;
                                    teamHat.vMax = 0f;
                                    teamHat.gravMultiplier = 0f;
                                    teamHat.floatMultiplier = 0f;
                                    teamHat.throwSpeedMultiplier = 0f;
                                    teamHat.canPickUp = false;
                                    teamHat.destructive = false;
                                    teamHat._destroyed = false;
                                    teamHat.burnSpeed = 0;
                                    teamHat.onFire = false;
                                    teamHat.depth = 0;
                                    teamHat.layer = Layer.Foreground;
                                    teamHat.enablePhysics = false;
                                }
                                imageDownPress = false;
                            }
                        }
                        if (nineGrid.Length == 9 && loaded)
                        {
                            SendImagesToNetwork(9);

                            UpdateImages(mouse);
                        }
                    }
                }
                if (Mouse.left == InputState.None && imageName != default(string))
                {
                    imageDownPress = true;
                    duckGameology = 0;
                }
            }
            if (!bitmapMode)
            {
                imageName = default(string);
                loaded = false;
            }
        }
        private void UpdateImages(MousePosition mouse)
        {
            if (grid == 9)
            {
                nineGrid[0].position.x = mouse.xpos;
                nineGrid[0].position.y = mouse.ypos;

                nineGrid[1].position.x = mouse.xpos + width;
                nineGrid[1].position.y = mouse.ypos;

                nineGrid[2].position.x = mouse.xpos + width * 2;
                nineGrid[2].position.y = mouse.ypos;

                nineGrid[3].position.x = mouse.xpos;
                nineGrid[3].position.y = mouse.ypos + height;

                nineGrid[4].position.x = mouse.xpos + width;
                nineGrid[4].position.y = mouse.ypos + height;

                nineGrid[5].position.x = mouse.xpos + width * 2;
                nineGrid[5].position.y = mouse.ypos + height;

                nineGrid[6].position.x = mouse.xpos;
                nineGrid[6].position.y = mouse.ypos + height * 2;

                nineGrid[7].position.x = mouse.xpos + width;
                nineGrid[7].position.y = mouse.ypos + height * 2;

                nineGrid[8].position.x = mouse.xpos + width * 2;
                nineGrid[8].position.y = mouse.ypos + height * 2;
            }
            if (grid == 4)
            {               
                fourGrid[0].position.x = mouse.xpos;
                fourGrid[0].position.y = mouse.ypos;

                fourGrid[1].position.x = mouse.xpos + width;
                fourGrid[1].position.y = mouse.ypos;

                fourGrid[2].position.x = mouse.xpos;
                fourGrid[2].position.y = mouse.ypos + height;

                fourGrid[3].position.x = mouse.xpos + width;
                fourGrid[3].position.y = mouse.ypos + height;
            }
            if(grid == 1)
            {
                image.position.x = mouse.xpos;
                image.position.y = mouse.ypos;
            }
        }
        private bool LoadImages(int images, float xVec, float yVec, string path)
        {
            int index = 0;

            if(images == 9) 
            {
                bool flag = Directory.Exists(path + "9/" + imageName);
                if (flag)
                {
                    foreach (string file in Directory.GetFiles(path + "9/" + imageName))    // looks
                    {
                        if ((imageName + (index + 1).ToString() + ".png") == Path.GetFileName(file))
                        {
                            switch (index)
                            {
                                case 0:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec, yVec, nineSprites[index]);

                                    break;
                                case 1:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec + width, yVec, nineSprites[index]);

                                    break;
                                case 2:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec + (width * 2), yVec, nineSprites[index]);

                                    break;
                                //////
                                case 3:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec, yVec + height, nineSprites[index]);

                                    break;
                                case 4:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec + width, yVec + height, nineSprites[index]);

                                    break;
                                case 5:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec + (width * 2), yVec + height, nineSprites[index]);

                                    break;
                                //////
                                case 6:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec, yVec + (height * 2), nineSprites[index]);

                                    break;
                                case 7:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec + width, yVec + (height * 2), nineSprites[index]);

                                    break;
                                case 8:
                                    nineSprites[index] = Team.Deserialize(file);
                                    nineGrid[index] = new TeamHat(xVec + (width * 2), yVec + (height * 2), nineSprites[index]);

                                    break;
                            }
                            Level.Add(nineGrid[index]);
                            addedHats.Add(nineGrid[index]);
                            extraTeams.Add(nineSprites[index]);
                            Teams.AddExtraTeam(nineSprites[index]);
                            DevConsole.Log(index.ToString());
                            if (index < 8) index++;
                        }
                        else
                        {
                            DevConsole.Log(DCSection.Mod, $"|ORANGE|image '{imageName + index}' not found", -1);
                            bitmapMode = false;
                            imageName = default(string);
                            return false;
                        }
                    } 
                }
                else
                {
                    DevConsole.Log(DCSection.Mod, $"|ORANGE|image '{imageName}' not found", -1);
                    bitmapMode = false;
                    imageName = default(string);
                    return false;
                }
            }
            if(images == 4)
            {
                bool flag2 = Directory.Exists(path + "4/" + imageName);
                if (flag2)
                {
                    foreach (string file in Directory.GetFiles(path + "4/" + imageName))
                    {
                        if ((imageName + (index + 1).ToString() + ".png") == Path.GetFileName(file))
                        {
                            switch (index)
                            {
                                case 0:
                                    fourSprites[index] = Team.Deserialize(file);
                                    fourGrid[index] = new TeamHat(xVec, yVec, fourSprites[index]);

                                    break;
                                case 1:
                                    fourSprites[index] = Team.Deserialize(file);
                                    fourGrid[index] = new TeamHat(xVec + width, yVec, fourSprites[index]);

                                    break;
                                case 2:
                                    fourSprites[index] = Team.Deserialize(file);
                                    fourGrid[index] = new TeamHat(xVec, yVec + height, fourSprites[index]);

                                    break;
                                case 3:
                                    fourSprites[index] = Team.Deserialize(file);
                                    fourGrid[index] = new TeamHat(xVec + width, yVec + height, fourSprites[index]);

                                    break;
                            }
                            Level.Add(fourGrid[index]);
                            addedHats.Add(fourGrid[index]);
                            extraTeams.Add(fourSprites[index]);
                            Teams.AddExtraTeam(fourSprites[index]);

                            if (index < 3) index++;
                        }
                        else
                        {
                            DevConsole.Log(DCSection.Mod, $"|ORANGE|image '{imageName + index}' not found", -1);
                            bitmapMode = false;
                            imageName = default(string);
                            return false;
                        }
                    }
                }
                else
                {
                    DevConsole.Log(DCSection.Mod, $"|ORANGE|image '{imageName}' not found", -1);
                    bitmapMode = false;
                    imageName = default(string);
                    return false;
                }
            }
            if(images == 1)
            {
                MousePosition mouse = new MousePosition();

                if(File.Exists(path + "1/" + imageName + ".png"))
                {
                    sprite = Team.Deserialize(path + "1/" + imageName + ".png");
                    image = new TeamHat(mouse.xpos, mouse.ypos, sprite);

                    Level.Add(image);
                    addedHats.Add(image);
                    extraTeams.Add(sprite);

                    Teams.AddExtraTeam(sprite);
                }
                else
                {
                    return false;
                }    
            }
            return true;    // if nothing is detected 
        }
        private void SendImagesToNetwork(int images)
        {
            if (images == 9)
            {
                if (!imageDownPress && duckGameology < 3 && bitmapMode)
                {
                    duckGameology++;
                }
                if (duckGameology == 3)
                {
                    foreach (Team team in nineSprites)
                    {
                        NMSpecialHat loadHat = new NMSpecialHat(team, LocalUser);
                        Send.Message(loadHat, null);
                    }
                    duckGameology = 5;
                }
            }
            if (images == 4)
            {
                if (!imageDownPress && duckGameology < 3 && bitmapMode)
                {
                    duckGameology++;
                }
                if (duckGameology == 3)
                {
                    foreach (Team team in fourSprites)
                    {
                        NMSpecialHat loadHat = new NMSpecialHat(team, LocalUser);
                        Send.Message(loadHat, null);
                    }
                    duckGameology = 5;
                }
            }
            if (images == 1)
            {
                if (!imageDownPress && duckGameology < 3 && bitmapMode)
                {
                    duckGameology++;
                }
                if (duckGameology == 3)
                {
                    NMSpecialHat loadHat = new NMSpecialHat(sprite, LocalUser);
                    Send.Message(loadHat, null);
                    duckGameology = 5;
                }
            }
        }
    }
    public class MousePosition
    {
        public static bool bitMapMode = false;

        public float xpos = Level.current.camera.transformScreenVector(Mouse.mousePos).x;     // get x position of cursor from DuckGamePanel
        public float ypos = Level.current.camera.transformScreenVector(Mouse.mousePos).y;     // get y position of cursor from DuckGamePanel
    }
    
}
