namespace WinFormsApp1___Forside
{
    partial class Chat
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
            buttonSend = new Button();
            label1 = new Label();
            listBoxChat = new ListBox();
            textBoxBesked = new TextBox();
            SuspendLayout();
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(509, 398);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(269, 29);
            buttonSend.TabIndex = 0;
            buttonSend.Text = "Send besked";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(386, 9);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // listBoxChat
            // 
            listBoxChat.FormattingEnabled = true;
            listBoxChat.Location = new Point(24, 42);
            listBoxChat.Name = "listBoxChat";
            listBoxChat.Size = new Size(754, 304);
            listBoxChat.TabIndex = 2;
            listBoxChat.SelectedIndexChanged += listBoxChat_SelectedIndexChanged;
            // 
            // textBoxBesked
            // 
            textBoxBesked.Location = new Point(24, 400);
            textBoxBesked.Name = "textBoxBesked";
            textBoxBesked.Size = new Size(445, 27);
            textBoxBesked.TabIndex = 3;
            textBoxBesked.TextChanged += textBoxBesked_TextChanged;
            // 
            // Chat
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxBesked);
            Controls.Add(listBoxChat);
            Controls.Add(label1);
            Controls.Add(buttonSend);
            Name = "Chat";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSend;
        private Label label1;
        private ListBox listBoxChat;
        private TextBox textBoxBesked;
    }
}