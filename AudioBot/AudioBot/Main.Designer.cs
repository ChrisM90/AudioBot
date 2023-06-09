namespace AudioBot
{
    partial class Main
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
            joinChannel = new Button();
            label2 = new Label();
            channelDropdown = new ComboBox();
            label1 = new Label();
            audioDropdown = new ComboBox();
            startStopAudio = new Button();
            SuspendLayout();
            // 
            // joinChannel
            // 
            joinChannel.Location = new Point(180, 75);
            joinChannel.Name = "joinChannel";
            joinChannel.Size = new Size(68, 23);
            joinChannel.TabIndex = 12;
            joinChannel.Text = "Join";
            joinChannel.UseVisualStyleBackColor = true;
            joinChannel.Click += joinChannel_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 57);
            label2.Name = "label2";
            label2.Size = new Size(129, 15);
            label2.TabIndex = 11;
            label2.Text = "Discord channel to join";
            // 
            // channelDropdown
            // 
            channelDropdown.FormattingEnabled = true;
            channelDropdown.Location = new Point(12, 75);
            channelDropdown.Name = "channelDropdown";
            channelDropdown.Size = new Size(162, 23);
            channelDropdown.TabIndex = 10;
            channelDropdown.SelectionChangeCommitted += channelDropdown_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(172, 15);
            label1.TabIndex = 9;
            label1.Text = "Audio output device to capture";
            // 
            // audioDropdown
            // 
            audioDropdown.FormattingEnabled = true;
            audioDropdown.Location = new Point(12, 28);
            audioDropdown.Name = "audioDropdown";
            audioDropdown.Size = new Size(236, 23);
            audioDropdown.TabIndex = 8;
            audioDropdown.SelectedIndexChanged += audioDropdown_SelectedIndexChanged;
            // 
            // startStopAudio
            // 
            startStopAudio.Enabled = false;
            startStopAudio.Location = new Point(12, 116);
            startStopAudio.Name = "startStopAudio";
            startStopAudio.Size = new Size(236, 30);
            startStopAudio.TabIndex = 13;
            startStopAudio.Text = "Start Audio";
            startStopAudio.UseVisualStyleBackColor = true;
            startStopAudio.Click += startStopAudio_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(262, 157);
            Controls.Add(startStopAudio);
            Controls.Add(joinChannel);
            Controls.Add(label2);
            Controls.Add(channelDropdown);
            Controls.Add(label1);
            Controls.Add(audioDropdown);
            Name = "Main";
            Text = "AudioBot V1.0";
            Load += Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button joinChannel;
        private Label label2;
        private ComboBox channelDropdown;
        private Label label1;
        private ComboBox audioDropdown;
        private Button startStopAudio;
    }
}