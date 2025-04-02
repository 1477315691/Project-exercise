using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace WindowsFormsApp2.ChangeExcelfile
{
    public partial class FormOutput : Form
    {
       
        public FormOutput(List<string> outputData)
        {
            InitializeComponent();
            TextBoxOutput1.Text = string.Join(Environment.NewLine, outputData);
        }
        

        private void TextBoxOutput1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ReturnBtn_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            //f1.ShowDialog();
        }
    }
}
