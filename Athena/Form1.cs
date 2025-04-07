using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace Athena
{
    public partial class Athena : Form
    {
        // Importing GDI32.dll for rounded rectangle region
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse);

        public Athena()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 30, 30));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Editor.Navigate(new Uri(string.Format("file:///{0}/Monaco/Monaco.html", Directory.GetCurrentDirectory())));
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        public class mintAPI
        {
            [DllImport("MintAPI.dll")]
            public static extern int inject();
            [DllImport("MintAPI.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void execute(string source);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) // Execute Script
        {
            string script = Editor.Document.InvokeScript("getValue").ToString();
            mintAPI.execute(script);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Editor.Navigate(new Uri(string.Format("file:///{0}/Monaco/Monaco.html", Directory.GetCurrentDirectory())));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e) // Api injecting
        {
            if (mintAPI.inject() == 0) 
            {
                ExecuteAutoexecScripts();
            }
        }

        private void ExecuteAutoexecScripts() // Autoexecute of scripts
        {
            string autoexecPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "autoexec");

            if (!Directory.Exists(autoexecPath))
            {
                Directory.CreateDirectory(autoexecPath);
            }

            string[] txtFiles = Directory.GetFiles(autoexecPath, "*.txt");
            if (txtFiles.Length > 0)
            {
                foreach (string file in txtFiles)
                {
                    string script = File.ReadAllText(file);
                    mintAPI.execute(script);
                }
            }

            string[] luaFiles = Directory.GetFiles(autoexecPath, "*.lua");
            if (luaFiles.Length > 0)
            {
                foreach (string file in luaFiles)
                {
                    string script = File.ReadAllText(file);
                    mintAPI.execute(script);
                }
            }
        }
    }
}