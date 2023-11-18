using System;

namespace DisplayImages.source.UserInterface.FormWindow
{
    partial class DIMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DIMenu));
            this.selectImageButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.smallButton = new System.Windows.Forms.Button();
            this.bigButton = new System.Windows.Forms.Button();
            this.hugeButton = new System.Windows.Forms.Button();
            this.customButton = new System.Windows.Forms.Button();
            this.OnOffSwitch = new System.Windows.Forms.Button();
            this.displayMode = new System.Windows.Forms.Label();
            this.resizeButton = new System.Windows.Forms.Button();
            this.dropDownMenu = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dropDownMenu2 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.splitButton = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.dropDownMenu.SuspendLayout();
            this.dropDownMenu2.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectImageButton
            // 
            this.selectImageButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.selectImageButton, "selectImageButton");
            this.selectImageButton.Name = "selectImageButton";
            this.selectImageButton.UseVisualStyleBackColor = false;
            this.selectImageButton.Click += new System.EventHandler(this.selectImageButton_Click);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // smallButton
            // 
            this.smallButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.smallButton, "smallButton");
            this.smallButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.smallButton.Name = "smallButton";
            this.smallButton.UseVisualStyleBackColor = false;
            this.smallButton.Click += new System.EventHandler(this.smallButton_Click);
            // 
            // bigButton
            // 
            this.bigButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.bigButton, "bigButton");
            this.bigButton.Name = "bigButton";
            this.bigButton.UseVisualStyleBackColor = false;
            this.bigButton.Click += new System.EventHandler(this.bigButton_Click);
            // 
            // hugeButton
            // 
            this.hugeButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.hugeButton, "hugeButton");
            this.hugeButton.Name = "hugeButton";
            this.hugeButton.UseVisualStyleBackColor = false;
            this.hugeButton.Click += new System.EventHandler(this.hugeButton_Click);
            // 
            // customButton
            // 
            this.customButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.customButton, "customButton");
            this.customButton.Name = "customButton";
            this.customButton.UseVisualStyleBackColor = false;
            this.customButton.Click += new System.EventHandler(this.customButton_Click);
            // 
            // OnOffSwitch
            // 
            this.OnOffSwitch.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.OnOffSwitch, "OnOffSwitch");
            this.OnOffSwitch.Name = "OnOffSwitch";
            this.OnOffSwitch.UseVisualStyleBackColor = false;
            this.OnOffSwitch.Click += new System.EventHandler(this.OnOffSwitch_Click);
            // 
            // displayMode
            // 
            resources.ApplyResources(this.displayMode, "displayMode");
            this.displayMode.ForeColor = System.Drawing.Color.Red;
            this.displayMode.Name = "displayMode";
            // 
            // resizeButton
            // 
            this.resizeButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.resizeButton, "resizeButton");
            this.resizeButton.Name = "resizeButton";
            this.resizeButton.UseVisualStyleBackColor = false;
            this.resizeButton.Click += new System.EventHandler(this.resizeButton_Click);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Controls.Add(this.button5);
            this.dropDownMenu.Controls.Add(this.button4);
            this.dropDownMenu.Controls.Add(this.button3);
            this.dropDownMenu.Controls.Add(this.button2);
            this.dropDownMenu.Controls.Add(this.resizeButton);
            resources.ApplyResources(this.dropDownMenu, "dropDownMenu");
            this.dropDownMenu.Name = "dropDownMenu";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dropDownMenu2
            // 
            this.dropDownMenu2.Controls.Add(this.button7);
            this.dropDownMenu2.Controls.Add(this.button6);
            this.dropDownMenu2.Controls.Add(this.splitButton);
            resources.ApplyResources(this.dropDownMenu2, "dropDownMenu2");
            this.dropDownMenu2.Name = "dropDownMenu2";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // splitButton
            // 
            this.splitButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.splitButton, "splitButton");
            this.splitButton.Name = "splitButton";
            this.splitButton.UseVisualStyleBackColor = false;
            this.splitButton.Click += new System.EventHandler(this.splitButton_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // DIMenu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dropDownMenu2);
            this.Controls.Add(this.dropDownMenu);
            this.Controls.Add(this.displayMode);
            this.Controls.Add(this.OnOffSwitch);
            this.Controls.Add(this.customButton);
            this.Controls.Add(this.hugeButton);
            this.Controls.Add(this.bigButton);
            this.Controls.Add(this.smallButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.selectImageButton);
            this.Name = "DIMenu";
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DIMenu_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.dropDownMenu.ResumeLayout(false);
            this.dropDownMenu2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

   


        #endregion
        private System.Windows.Forms.Button selectImageButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button smallButton;
        private System.Windows.Forms.Button bigButton;
        private System.Windows.Forms.Button hugeButton;
        private System.Windows.Forms.Button customButton;
        private System.Windows.Forms.Button OnOffSwitch;
        private System.Windows.Forms.Label displayMode;
        private System.Windows.Forms.Button resizeButton;
        private System.Windows.Forms.Panel dropDownMenu;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel dropDownMenu2;
        private System.Windows.Forms.Button splitButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label2;
    }
}