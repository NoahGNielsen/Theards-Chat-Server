namespace WinFormsApp1___Forside
{
    partial class Forside
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelIP = new Label();
            textBoxIP = new TextBox();
            buttonForbindIP = new Button();
            labelIngenServer = new Label();
            buttonStart = new Button();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new Point(41, 34);
            labelIP.Name = "labelIP";
            labelIP.Size = new Size(38, 32);
            labelIP.TabIndex = 0;
            labelIP.Text = "IP:";
            // 
            // textBoxIP
            // 
            textBoxIP.Location = new Point(100, 31);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.Size = new Size(200, 39);
            textBoxIP.TabIndex = 1;
            // 
            // buttonForbindIP
            // 
            buttonForbindIP.Location = new Point(41, 97);
            buttonForbindIP.Name = "buttonForbindIP";
            buttonForbindIP.Size = new Size(259, 46);
            buttonForbindIP.TabIndex = 2;
            buttonForbindIP.Text = "Forbind";
            buttonForbindIP.UseVisualStyleBackColor = true;
            // 
            // labelIngenServer
            // 
            labelIngenServer.AutoSize = true;
            labelIngenServer.Location = new Point(33, 25);
            labelIngenServer.Name = "labelIngenServer";
            labelIngenServer.Size = new Size(149, 32);
            labelIngenServer.TabIndex = 3;
            labelIngenServer.Text = "Ingen Server";
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(196, 25);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(150, 46);
            buttonStart.TabIndex = 4;
            buttonStart.Text = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(buttonForbindIP);
            panel1.Controls.Add(textBoxIP);
            panel1.Controls.Add(labelIP);
            panel1.Location = new Point(267, 133);
            panel1.Name = "panel1";
            panel1.Size = new Size(353, 187);
            panel1.TabIndex = 5;
            // 
            // Forside
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(867, 392);
            Controls.Add(panel1);
            Controls.Add(buttonStart);
            Controls.Add(labelIngenServer);
            Name = "Forside";
            Text = "  ";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelIP;
        private TextBox textBoxIP;
        private Button buttonForbindIP;
        private Label labelIngenServer;
        private Button buttonStart;
        private Panel panel1;
    }
}
