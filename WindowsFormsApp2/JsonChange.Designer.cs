namespace WindowsFormsApp2
{
    partial class JsonChange
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
            this.SelecJsonChangeBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.AllJaonChangeBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ChageRegion = new System.Windows.Forms.Button();
            this.RegionCcb = new System.Windows.Forms.ComboBox();
            this.returnBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelecJsonChangeBtn
            // 
            this.SelecJsonChangeBtn.Location = new System.Drawing.Point(64, 117);
            this.SelecJsonChangeBtn.Name = "SelecJsonChangeBtn";
            this.SelecJsonChangeBtn.Size = new System.Drawing.Size(113, 23);
            this.SelecJsonChangeBtn.TabIndex = 0;
            this.SelecJsonChangeBtn.Text = "Selected json";
            this.SelecJsonChangeBtn.UseVisualStyleBackColor = true;
            this.SelecJsonChangeBtn.Click += new System.EventHandler(this.SelecJsonChangeBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // AllJaonChangeBtn
            // 
            this.AllJaonChangeBtn.Location = new System.Drawing.Point(64, 175);
            this.AllJaonChangeBtn.Name = "AllJaonChangeBtn";
            this.AllJaonChangeBtn.Size = new System.Drawing.Size(113, 23);
            this.AllJaonChangeBtn.TabIndex = 2;
            this.AllJaonChangeBtn.Text = "All json";
            this.AllJaonChangeBtn.UseVisualStyleBackColor = true;
            this.AllJaonChangeBtn.Click += new System.EventHandler(this.AllJsonChangeBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Region";
            // 
            // ChageRegion
            // 
            this.ChageRegion.Location = new System.Drawing.Point(75, 302);
            this.ChageRegion.Name = "ChageRegion";
            this.ChageRegion.Size = new System.Drawing.Size(84, 20);
            this.ChageRegion.TabIndex = 4;
            this.ChageRegion.Text = "Select";
            this.ChageRegion.UseVisualStyleBackColor = true;
            this.ChageRegion.Click += new System.EventHandler(this.ChageRegion_Click);
            // 
            // RegionCcb
            // 
            this.RegionCcb.FormattingEnabled = true;
            this.RegionCcb.IntegralHeight = false;
            this.RegionCcb.ItemHeight = 12;
            this.RegionCcb.Items.AddRange(new object[] {
            "East Asia",
            "Southeast Asia",
            "Australia East",
            "Australia Southeast",
            "Brazil South",
            "Canada Central",
            "Canada East",
            "China East",
            "China North",
            "North Europe",
            "West Europe",
            "Germany Central",
            "Germany Northeast",
            "USGov Virginia",
            "USGov Iowa",
            "Central India",
            "South India",
            "West India",
            "Japan East",
            "Japan West",
            "Korea Central",
            "Korea South   ",
            "UK South",
            "UK West",
            "Central US",
            "East US",
            "East US 2",
            "North Central US",
            "South Central US",
            "West US",
            "West US 2",
            "West Central US",
            "Central US EUAP",
            "East US 2 EUAP",
            "Brazil Southeast",
            "France Central",
            "Australia Central",
            "South Africa North",
            "UAE North",
            "Switzerland North",
            "Germany North",
            "Norway East",
            "Jio India West",
            "Sweden Central",
            "France South",
            "Australia Central 2",
            "South Africa West",
            "UAE Central",
            "Switzerland West",
            "Germany West Central",
            "Norway West",
            "Jio India Central",
            "West US 3",
            "Sweden South",
            "Qatar Central",
            "Poland Central",
            "Italy North",
            "Malaysia South",
            "Israel Central"});
            this.RegionCcb.Location = new System.Drawing.Point(41, 264);
            this.RegionCcb.Name = "RegionCcb";
            this.RegionCcb.Size = new System.Drawing.Size(154, 20);
            this.RegionCcb.TabIndex = 5;
            // 
            // returnBtn
            // 
            this.returnBtn.BackgroundImage = global::WindowsFormsApp2.Properties.Resources._return;
            this.returnBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.returnBtn.Location = new System.Drawing.Point(23, 22);
            this.returnBtn.Name = "returnBtn";
            this.returnBtn.Size = new System.Drawing.Size(26, 28);
            this.returnBtn.TabIndex = 6;
            this.returnBtn.UseVisualStyleBackColor = true;
            this.returnBtn.Click += new System.EventHandler(this.returnBtn_Click);
            // 
            // JsonChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 367);
            this.Controls.Add(this.returnBtn);
            this.Controls.Add(this.RegionCcb);
            this.Controls.Add(this.ChageRegion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AllJaonChangeBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelecJsonChangeBtn);
            this.Name = "JsonChange";
            this.Text = "JsonChange";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelecJsonChangeBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AllJaonChangeBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ChageRegion;
        private System.Windows.Forms.ComboBox RegionCcb;
        private System.Windows.Forms.Button returnBtn;
    }
}