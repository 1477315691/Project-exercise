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
            this.DateTittle.Location = new System.Drawing.Point(21, 32);
            this.DateTittle.Name = "DateTittle";
            this.DateTittle.Size = new System.Drawing.Size(47, 12);
            this.DateTittle.TabIndex = 0;
            this.DateTittle.Text = "Date : ";
            // 
            // FileTtile
            // 
            this.FileTtile.AutoSize = true;
            this.FileTtile.Location = new System.Drawing.Point(23, 92);
            this.FileTtile.Name = "FileTtile";
            this.FileTtile.Size = new System.Drawing.Size(47, 12);
            this.FileTtile.TabIndex = 2;
            this.FileTtile.Text = "File : ";
            // 
            // FileBtn
            // 
            this.FileBtn.Location = new System.Drawing.Point(76, 87);
            this.FileBtn.Name = "FileBtn";
            this.FileBtn.Size = new System.Drawing.Size(302, 23);
            this.FileBtn.TabIndex = 3;
            this.FileBtn.Text = "click to add txt";
            this.FileBtn.UseVisualStyleBackColor = true;
            this.FileBtn.Click += new System.EventHandler(this.FileBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(163, 263);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(117, 21);
            this.PrintBtn.TabIndex = 4;
            this.PrintBtn.Text = "print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // GroupTitle
            // 
            this.GroupTitle.AutoSize = true;
            this.GroupTitle.Location = new System.Drawing.Point(21, 151);
            this.GroupTitle.Name = "GroupTitle";
            this.GroupTitle.Size = new System.Drawing.Size(53, 12);
            this.GroupTitle.TabIndex = 5;
            this.GroupTitle.Text = "Group : ";
            // 
            // groupText
            // 
            this.groupText.Location = new System.Drawing.Point(76, 142);
            this.groupText.Name = "groupText";
            this.groupText.Size = new System.Drawing.Size(302, 21);
            this.groupText.TabIndex = 6;
            // 
            // DateText
            // 
            this.DateText.Location = new System.Drawing.Point(76, 29);
            this.DateText.Name = "DateText";
            this.DateText.Size = new System.Drawing.Size(302, 21);
            this.DateText.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.comboBox1.Location = new System.Drawing.Point(76, 205);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type:";
            // 
            // Performance_GetKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 309);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.DateText);
            this.Controls.Add(this.groupText);
            this.Controls.Add(this.GroupTitle);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.FileBtn);
            this.Controls.Add(this.FileTtile);
            this.Controls.Add(this.DateTittle);
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