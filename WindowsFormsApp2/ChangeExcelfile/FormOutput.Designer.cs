using System.Windows.Forms;

namespace WindowsFormsApp2.ChangeExcelfile
{
    partial class FormOutput
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
            this.TextBoxOutput1 = new System.Windows.Forms.TextBox();
            this.ReturnBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxOutput1
            // 
            this.TextBoxOutput1.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxOutput1.ForeColor = System.Drawing.Color.Black;
            this.TextBoxOutput1.Location = new System.Drawing.Point(49, 12);
            this.TextBoxOutput1.Multiline = true;
            this.TextBoxOutput1.Name = "TextBoxOutput1";
            this.TextBoxOutput1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxOutput1.Size = new System.Drawing.Size(707, 364);
            this.TextBoxOutput1.TabIndex = 0;
            this.TextBoxOutput1.TextChanged += new System.EventHandler(this.TextBoxOutput1_TextChanged);
            // 
            // ReturnBtn
            // 
            this.ReturnBtn.BackColor = System.Drawing.SystemColors.Control;
            this.ReturnBtn.BackgroundImage = global::WindowsFormsApp2.Properties.Resources._return;
            this.ReturnBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ReturnBtn.Font = new System.Drawing.Font("Microsoft Yi Baiti", 6F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReturnBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.ReturnBtn.Location = new System.Drawing.Point(369, 398);
            this.ReturnBtn.Name = "ReturnBtn";
            this.ReturnBtn.Size = new System.Drawing.Size(37, 40);
            this.ReturnBtn.TabIndex = 10;
            this.ReturnBtn.UseVisualStyleBackColor = false;
            this.ReturnBtn.Click += new System.EventHandler(this.ReturnBtn_Click);
            // 
            // FormOutput
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ReturnBtn);
            this.Controls.Add(this.TextBoxOutput1);
            this.Name = "FormOutput";
            this.Text = "输出结果";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox TextBoxOutput1;
        private Button ReturnBtn;
    }
}