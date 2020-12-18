using System;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;

namespace Prezent
{
    public partial class Form1 : Form
    {
        private string DirPath;
        public Form1()
        {
            InitializeComponent();
            if(File.Exists("Options.xml"))
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(new Klasa().GetType());
                    Class.klasa = (Klasa)xml.Deserialize(File.OpenRead("c:\\Users\\" + Environment.UserName + "\\Options.xml"));
                    textBox1.Text = Class.klasa.EmailAdd;
                    textBox2.Text = Class.klasa.EmailPass;
                }
                catch
                {
                    Class.klasa = new Klasa();
                }
            }
            else
            {
                Class.klasa = new Klasa();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FormClosing += Form1_FormClosing;
            button1.Click += Button1_Click;
            zapiszDoToolStripMenuItem.Click += ZapiszDoToolStripMenuItem_Click;
        }

        private void ZapiszDoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.SelectedPath = "c:\\Users\\" + Environment.UserName + "\\Desktop\\";
            if (FBD.ShowDialog() != DialogResult.OK)
                return;
            DirPath = FBD.SelectedPath;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0 || textBox2.TextLength == 0 || textBox3.TextLength == 0)
                return;
            Class.klasa.EmailAdd = textBox1.Text;
            Class.klasa.EmailPass = textBox2.Text;
            string Selected = textBox3.Text;
            Mail.Start(Selected, DirPath);
            MessageBox.Show("Skończono!");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(Class.klasa.GetType());
                xml.Serialize(File.Create("c:\\Users\\" + Environment.UserName + "\\Options.xml"), Class.klasa);
            }
            catch
            {

            }
        }
    }
}
