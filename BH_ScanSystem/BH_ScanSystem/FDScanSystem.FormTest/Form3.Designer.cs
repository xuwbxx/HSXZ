namespace FDScanSystem.FormTest
{
    partial class Form3
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
            this.listView = new FDScanSystem.FormTest.DoubleBufferListView();
            this.label4 = new System.Windows.Forms.Label();
            this.timer_getData = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label_XCoordinateMax = new System.Windows.Forms.Label();
            this.label_YCoordinateMin = new System.Windows.Forms.Label();
            this.label_YCoordinateMax = new System.Windows.Forms.Label();
            this.label_YCoordinateMiddle = new System.Windows.Forms.Label();
            this.label_XCoordinateMin = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Location = new System.Drawing.Point(73, 387);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(680, 50);
            this.listView.TabIndex = 68;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 421);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 67;
            this.label4.Text = "8";
            // 
            // timer_getData
            // 
            this.timer_getData.Tick += new System.EventHandler(this.timer_getData_Tick_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 391);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 66;
            this.label3.Text = "0";
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox.Location = new System.Drawing.Point(92, 25);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(660, 340);
            this.pictureBox.TabIndex = 64;
            this.pictureBox.TabStop = false;
            // 
            // label_XCoordinateMax
            // 
            this.label_XCoordinateMax.AutoSize = true;
            this.label_XCoordinateMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_XCoordinateMax.Location = new System.Drawing.Point(713, 367);
            this.label_XCoordinateMax.Name = "label_XCoordinateMax";
            this.label_XCoordinateMax.Size = new System.Drawing.Size(40, 13);
            this.label_XCoordinateMax.TabIndex = 62;
            this.label_XCoordinateMax.Text = "10 Sec";
            // 
            // label_YCoordinateMin
            // 
            this.label_YCoordinateMin.Location = new System.Drawing.Point(42, 350);
            this.label_YCoordinateMin.Name = "label_YCoordinateMin";
            this.label_YCoordinateMin.Size = new System.Drawing.Size(52, 15);
            this.label_YCoordinateMin.TabIndex = 61;
            this.label_YCoordinateMin.Text = "0V";
            this.label_YCoordinateMin.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_YCoordinateMax
            // 
            this.label_YCoordinateMax.Location = new System.Drawing.Point(40, 21);
            this.label_YCoordinateMax.Name = "label_YCoordinateMax";
            this.label_YCoordinateMax.Size = new System.Drawing.Size(52, 15);
            this.label_YCoordinateMax.TabIndex = 60;
            this.label_YCoordinateMax.Text = "5V";
            this.label_YCoordinateMax.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_YCoordinateMiddle
            // 
            this.label_YCoordinateMiddle.Location = new System.Drawing.Point(42, 188);
            this.label_YCoordinateMiddle.Name = "label_YCoordinateMiddle";
            this.label_YCoordinateMiddle.Size = new System.Drawing.Size(52, 15);
            this.label_YCoordinateMiddle.TabIndex = 65;
            this.label_YCoordinateMiddle.Text = "0V";
            this.label_YCoordinateMiddle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_XCoordinateMin
            // 
            this.label_XCoordinateMin.AutoSize = true;
            this.label_XCoordinateMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_XCoordinateMin.Location = new System.Drawing.Point(91, 367);
            this.label_XCoordinateMin.Name = "label_XCoordinateMin";
            this.label_XCoordinateMin.Size = new System.Drawing.Size(34, 13);
            this.label_XCoordinateMin.TabIndex = 63;
            this.label_XCoordinateMin.Text = "0 Sec";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 459);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.label_XCoordinateMax);
            this.Controls.Add(this.label_YCoordinateMin);
            this.Controls.Add(this.label_YCoordinateMax);
            this.Controls.Add(this.label_YCoordinateMiddle);
            this.Controls.Add(this.label_XCoordinateMin);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferListView listView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer_getData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox;
        public System.Windows.Forms.Label label_XCoordinateMax;
        private System.Windows.Forms.Label label_YCoordinateMin;
        private System.Windows.Forms.Label label_YCoordinateMax;
        private System.Windows.Forms.Label label_YCoordinateMiddle;
        private System.Windows.Forms.Label label_XCoordinateMin;
    }
}