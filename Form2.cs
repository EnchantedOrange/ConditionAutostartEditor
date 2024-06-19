using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConditionedAutostartEditor
{
    public partial class InputDialog : Form
    {
        public string InputText { get; private set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.InputText = textBox1.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string Show(string text)
        {
            InputDialog dialog = new InputDialog();
            dialog.InputText = text;
            dialog.textBox1.Text = text;
            dialog.ShowDialog();
            return dialog.InputText;
        }
    }
}
