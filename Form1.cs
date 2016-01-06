using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace QR
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        Bitmap img;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxECC.SelectedIndex = 0;
            set_comboBox_toolTip();
            generate_QR();
        }

        private void renderQRCode()
        {
            string level = comboBoxECC.SelectedItem.ToString();
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textQRCode.Text, eccLevel);
            QRCode qrCode = new QRCode(qrCodeData);
            img = qrCode.GetGraphic(20, Color.Black, Color.White);            
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            renderQRCode();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPG | *.jpg";
            if (dialog.ShowDialog() == DialogResult.OK)
                img.Save(dialog.FileName, ImageFormat.Jpeg);
        }
        
        private void set_comboBox_toolTip()
        {
            string level = comboBoxECC.SelectedItem.ToString();
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.comboBoxECC, (level == "L" ? "Can receive up to 7% Damage" : level == "M" ? "Can receive up to 15% Damage" : level == "Q" ? "Can receive up to 25% Damage" : "Can receive up to 30% Damage"));
        }

        private void generate_QR()
        {
            Single xDpi, yDpi;
            IntPtr dc = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(dc))
            {
                xDpi = g.DpiX;
                yDpi = g.DpiY;
            }
            renderQRCode();
            label2.Text = "QR Size(PIXEL)(HxW): " + img.Size.Height + "x" + img.Size.Width;
            label3.Text = "QR Size(INCH)(XxY): " + (img.Size.Width / xDpi) + "x" + (img.Size.Height / yDpi);
            Bitmap pic = new Bitmap(img, pictureBox1.Size);
            pictureBox1.BackgroundImage = pic;
        }

        private void comboBoxECC_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_comboBox_toolTip();
            generate_QR();
        }

        private void textQRCode_TextChanged(object sender, EventArgs e)
        {
            generate_QR();
        }        
    }
}
