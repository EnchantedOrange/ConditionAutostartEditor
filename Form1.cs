using ConditionedAutostartEditor.Properties;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConditionedAutostartEditor
{
    public partial class Form1 : Form
    {
        private List<Dictionary<string, string>> officePrograms = new List<Dictionary<string, string>>();
        private string fileName = "";

        public Form1()
        {
            InitializeComponent();
            /*using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    {*/
                    /*fileName = ofd.FileName;*/
                    fileName = "D:\\Soft\\ConditionedAutostart.bat";
                    int ind = 0;
                    IEnumerable<string> lines = File.ReadLines(fileName);
                    foreach (var line in lines)
                    {
                        Match match = Regex.Match(line, @"officeTime=(\d+)");
                        if (match.Success)
                        {
                            numericUpDown1.Value = int.Parse(match.Groups[1].Value);
                        }

                        bool is_match = Regex.IsMatch(line, @"start");
                        if (is_match)
                        {
                            Dictionary<string, string> elem = new Dictionary<string, string>();
                            string result = line.Replace("start /d", "");
                            result = result.Replace("REM ", "");
                            match = Regex.Match(result, "\"(.*?)\"");
                            string filePath = match.Groups[1].Value;
                            elem.Add("path", filePath);

                            match = Regex.Match(result, @"""[^""]*""\s+([^\s]+)");
                            string fileName = match.Groups[1].Value;
                            elem.Add("name", fileName);

                            string parameters = "";
                            match = Regex.Match(result, fileName + @"\s+(.*)");
                            if (match.Success)
                            {
                                parameters = match.Groups[1].Value;
                            }
                            elem.Add("parameters", parameters);

                            bool isChecked = !line.StartsWith("REM");
                            elem.Add("isChecked", isChecked.ToString());
                            officePrograms.Add(elem);
                            var item = new ListViewItem(new[] { isChecked.ToString(), filePath, fileName, parameters });
                            listView1.Items.Add(item);
                            ind++;
                        }

                        if (line.Equals("goto :END"))
                        {
                            break;
                        }
                    }
                /*}
            }*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Active", 80);
            listView1.Columns.Add("Path", 350);
            listView1.Columns.Add("Name", 200);
            listView1.Columns.Add("Parameters", 200);
            listView1.FullRowSelect = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Load file
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    officePrograms.Clear();
                    fileName = ofd.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Add program
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, string> elem = new Dictionary<string, string>();
                    Match match = Regex.Match(ofd.FileName, @"^(.*)\\[^\\]*$");
                    string filePath = "";
                    if (match.Success)
                    {
                        filePath = match.Groups[1].Value;
                    }
                    elem.Add("path", filePath);
                    elem.Add("name", ofd.SafeFileName);
                    elem.Add("parameters", "");
                    elem.Add("isChecked", "True");
                    officePrograms.Add(elem);

                    var item = new ListViewItem(new[] { "True", filePath, ofd.SafeFileName, "" });
                    listView1.Items.Add(item);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Save file
            string programs = "";
            for (int i = 0; i < listView1.Items?.Count; i++)
            {
                string program = "";

                if (listView1.Items[i].Text == "False")
                {
                    program += "REM ";
                }
                program += $"start /d\"{officePrograms[i]["path"]}\" {officePrograms[i]["name"]} {officePrograms[i]["parameters"]}";
                programs += $"{program}\r\n";
            }

            string doc = $"@echo off\r\n\r\nset \"officeTime={numericUpDown1.Value.ToString()}\"\r\n\r\nREM Get current time\r\nfor /f \"tokens=1,2 delims=:.\" %%H in (\"%time%\") do (set \"currentHour=%%H\")\r\n\r\nif %currentHour% GEQ %officeTime% (goto :HOME)\r\n\r\nREM Get current day of the week\r\nfor /f %%d in ('wmic path Win32_LocalTime get DayOfWeek ^| findstr /r \"[0-9]\"') do set /a \"dow=%%d\"\r\n\r\nREM Check if it's the weekend (Saturday or Sunday)\r\nif %dow% equ 0 goto :HOME\r\nif %dow% equ 6 goto :HOME\r\n\r\nECHO Office\r\n\r\n{programs}\r\n\r\ngoto :END\r\n\r\n:HOME\r\nECHO Home\r\nstart /d\"D:\\ProgramFiles\\MSI Afterburner\" MSIAfterburner.exe\r\n\r\n:END";

            File.WriteAllText(fileName, doc);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            // Edit programs
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;
            if (item != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DialogResult dialogResult = MessageBox.Show("Delete?", "Delete???", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        officePrograms.RemoveAt(item.Index);
                        listView1.Items[item.Index].Remove();
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    // Determine which sub-item was clicked
                    int subItemIndex = item.SubItems.IndexOf(info.SubItem);
                    string text;

                    switch (subItemIndex)
                    {
                        case 0:
                            if (item.Text == "True")
                            {
                                item.Text = "False";
                            }
                            else if (item.Text == "False")
                            {
                                item.Text = "True";
                            }

                            officePrograms[item.Index]["isChecked"] = item.Text;
                            break;
                        case 1:
                            text = InputDialog.Show(item.SubItems[subItemIndex].Text);
                            item.SubItems[subItemIndex].Text = text;
                            officePrograms[item.Index]["path"] = text;
                            break;
                        case 2:
                            text = InputDialog.Show(item.SubItems[subItemIndex].Text);
                            item.SubItems[subItemIndex].Text = text;
                            officePrograms[item.Index]["name"] = text;
                            break;
                        case 3:
                            text = InputDialog.Show(item.SubItems[subItemIndex].Text);
                            item.SubItems[subItemIndex].Text = text;
                            officePrograms[item.Index]["parameters"] = text;
                            break;
                    }
                }
            }
        }

        private void buttonKek_Click(object sender, EventArgs e)
        {
            var m = new FormKek();
            m.ShowDialog();
        }
    }
}