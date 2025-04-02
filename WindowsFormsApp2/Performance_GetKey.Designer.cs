namespace WindowsFormsApp2
{
    partial class Performance_GetKey
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
            this.DateTittle = new System.Windows.Forms.Label();
            this.FileTtile = new System.Windows.Forms.Label();
            this.FileBtn = new System.Windows.Forms.Button();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.GroupTitle = new System.Windows.Forms.Label();
            this.groupText = new System.Windows.Forms.TextBox();
            this.DateText = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DateTittle
            // 
            this.DateTittle.AutoSize = true;
            this.DateTittle.Location = new System.Drawing.Point(21, 35);
            this.DateTittle.Name = "DateTittle";
            this.DateTittle.Size = new System.Drawing.Size(39, 13);
            this.DateTittle.TabIndex = 0;
            this.DateTittle.Text = "Date : ";
            // 
            // FileTtile
            // 
            this.FileTtile.AutoSize = true;
            this.FileTtile.Location = new System.Drawing.Point(23, 100);
            this.FileTtile.Name = "FileTtile";
            this.FileTtile.Size = new System.Drawing.Size(32, 13);
            this.FileTtile.TabIndex = 2;
            this.FileTtile.Text = "File : ";
            // 
            // FileBtn
            // 
            this.FileBtn.BackColor = System.Drawing.SystemColors.Info;
            this.FileBtn.Location = new System.Drawing.Point(76, 94);
            this.FileBtn.Name = "FileBtn";
            this.FileBtn.Size = new System.Drawing.Size(302, 25);
            this.FileBtn.TabIndex = 3;
            this.FileBtn.Text = "click to add txt";
            this.FileBtn.UseVisualStyleBackColor = false;
            this.FileBtn.Click += new System.EventHandler(this.FileBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.BackColor = System.Drawing.SystemColors.Info;
            this.PrintBtn.Location = new System.Drawing.Point(163, 285);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(117, 23);
            this.PrintBtn.TabIndex = 4;
            this.PrintBtn.Text = "print";
            this.PrintBtn.UseVisualStyleBackColor = false;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // GroupTitle
            // 
            this.GroupTitle.AutoSize = true;
            this.GroupTitle.Location = new System.Drawing.Point(21, 164);
            this.GroupTitle.Name = "GroupTitle";
            this.GroupTitle.Size = new System.Drawing.Size(45, 13);
            this.GroupTitle.TabIndex = 5;
            this.GroupTitle.Text = "Group : ";
            // 
            // groupText
            // 
            this.groupText.BackColor = System.Drawing.SystemColors.Info;
            this.groupText.Location = new System.Drawing.Point(76, 154);
            this.groupText.Name = "groupText";
            this.groupText.Size = new System.Drawing.Size(302, 20);
            this.groupText.TabIndex = 6;
            // 
            // DateText
            // 
            this.DateText.BackColor = System.Drawing.SystemColors.Info;
            this.DateText.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.DateText.Location = new System.Drawing.Point(76, 35);
            this.DateText.Name = "DateText";
            this.DateText.Size = new System.Drawing.Size(302, 20);
            this.DateText.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.Info;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.comboBox1.Location = new System.Drawing.Point(76, 222);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type:";
            // 
            // Performance_GetKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(477, 334);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.DateText);
            this.Controls.Add(this.groupText);
            this.Controls.Add(this.GroupTitle);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.FileBtn);
            this.Controls.Add(this.FileTtile);
            this.Controls.Add(this.DateTittle);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "Performance_GetKey";
            this.Text = "GetKey";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DateTittle;
        private System.Windows.Forms.Label FileTtile;
        private System.Windows.Forms.Button FileBtn;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Label GroupTitle;
        private System.Windows.Forms.TextBox groupText;
        private System.Windows.Forms.TextBox DateText;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
    }
}