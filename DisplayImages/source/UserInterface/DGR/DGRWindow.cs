using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayImages.source.UserInterface.DGR
{
    public partial class DIMenu : Form
    {
        private bool isCollapsed;
        private bool isCollapsed_2;

        public DIMenu()
        {
            InitializeComponent();
        }

        #region Resize DropDownMenu
        /// <summary>
        /// 32x32
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                CSImageHandler.ResizeImage(DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1), textBox1.Text, "s");
        }
        /// <summary>
        /// 64x64
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                CSImageHandler.ResizeImage(DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1), textBox1.Text, "b");
        }
        /// <summary>
        /// 96x96
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                CSImageHandler.ResizeImage(DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1), textBox1.Text, "h");
        }
        /// <summary>
        /// custom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                CSImageHandler.ResizeImage(DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1), textBox1.Text, "toGrid");
        }
        #endregion
        private void selectImageButton_Click(object sender, EventArgs e)
        {
            Thread fileBrowserThread = new Thread(OpenDialog);
            fileBrowserThread.SetApartmentState(ApartmentState.STA);
            fileBrowserThread.Start();

            void OpenDialog()
            {
                OpenFileDialog browse = new OpenFileDialog();
                if (browse.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = browse.FileName;
                    pictureBox1.Image = new Bitmap(textBox1.Text);

                    if (File.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/1/" + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1) + ".png"))
                        smallButton.BackColor = Color.FromArgb(128, 255, 128);
                    else
                        smallButton.BackColor = Color.FromArgb(255, 128, 128);

                    if (Directory.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/4/" + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1)))
                        bigButton.BackColor = Color.FromArgb(128, 255, 128);
                    else
                        bigButton.BackColor = Color.FromArgb(255, 128, 128);

                    if (Directory.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/9/" + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1)))
                        hugeButton.BackColor = Color.FromArgb(128, 255, 128);
                    else
                        hugeButton.BackColor = Color.FromArgb(255, 128, 128);

                    if (File.Exists(DisplayImages.GetModPath.directory + "/DisplayImages/ImageFiles/CustomSize/" + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1) + ".png"))
                        customButton.BackColor = Color.FromArgb(128, 255, 128);
                    else
                        customButton.BackColor = Color.FromArgb(255, 128, 128);
                }
            }
        }

        private void DIMenu_DragEnter(object sender, DragEventArgs e)
        {
            DisplayImages.Info("enter: " + e.Data.GetData(DataFormats.Text).ToString());
            DisplayImages.Info("enter: " + e.ToString());
            textBox1.Text = e.Data.GetData(DataFormats.Text).ToString();
            try
            {
                pictureBox1.Image = new Bitmap(textBox1.Text);
            }
            catch (Exception)
            {

            }
        }

        private void smallButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                DuckGame.DevConsole.RunCommand("bitmap " + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1) + " 1");
        }

        private void bigButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                DuckGame.DevConsole.RunCommand("bitmap " + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1) + " 2");
        }

        private void hugeButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                DuckGame.DevConsole.RunCommand("bitmap " + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1) + " 3"); 
        }

        private void customButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "please choose a valid image !")
                DuckGame.DevConsole.RunCommand("bitmap " + DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1));  
        }

        private void OnOffSwitch_Click(object sender, EventArgs e)
        {
            BitImageUpdater.bitmapMode = !BitImageUpdater.bitmapMode;

            displayMode.Text = BitImageUpdater.bitmapMode ? "ON" : "OFF";
            displayMode.ForeColor = BitImageUpdater.bitmapMode ? Color.Green : Color.Red;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                dropDownMenu.Height += 100;
                if (dropDownMenu.Size == dropDownMenu.MaximumSize)
                {
                    timer1.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                dropDownMenu.Height -= 100;
                if (dropDownMenu.Size == dropDownMenu.MinimumSize)
                {
                    timer1.Stop();
                    isCollapsed = true;
                }
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isCollapsed_2)
            {
                dropDownMenu2.Height += 100;
                if (dropDownMenu2.Size == dropDownMenu2.MaximumSize)
                {
                    timer2.Stop();
                    isCollapsed_2 = false;
                }
            }
            else
            {
                dropDownMenu2.Height -= 100;
                if (dropDownMenu2.Size == dropDownMenu.MinimumSize)
                {
                    timer2.Stop();
                    isCollapsed_2 = true;
                }
            }
        }

        private void resizeButton_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        /// <summary>
        /// 2x2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            CSImageHandler.SplitImage(2, 2, DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1));
        }
        /// <summary>
        /// 3x3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            CSImageHandler.SplitImage(3, 3, DIWindow.GetImageName(textBox1.Text, textBox1.Text.Length - 1));
        }

        private void splitButton_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {

        }
    }
}
