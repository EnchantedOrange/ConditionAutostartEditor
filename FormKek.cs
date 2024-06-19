using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConditionedAutostartEditor.Properties
{
    public partial class FormKek : Form
    {
        public int speed = 50;
        public int speedX;
        public int speedY;

        public FormKek()
        {
            speedX = speed;
            speedY = speed;
            InitializeComponent();
            var rand = new Random();
            buttonKek.Left = rand.Next(ClientSize.Width - buttonKek.Width);
            buttonKek.Top = rand.Next(ClientSize.Height - buttonKek.Height);
            MoveButton();
        }

        private async void MoveButton()
        {
            while (true)
            {
                buttonKek.Left += speedX;
                buttonKek.Top += speedY;
                if (buttonKek.Left + buttonKek.Width >= ClientSize.Width)
                {
                    buttonKek.Left = ClientSize.Width - buttonKek.Width;
                    speedX = -speed;
                }
                else if (buttonKek.Left <= 0)
                {
                    speedX = speed;
                }

                if (buttonKek.Top + buttonKek.Height >= ClientSize.Height)
                {
                    buttonKek.Top = ClientSize.Height - buttonKek.Height;
                    speedY = -speed;
                }
                else if (buttonKek.Top <= 0)
                {
                    speedY = speed;
                }

                await Task.Delay(1);
            }
        }

        private void buttonKek_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
