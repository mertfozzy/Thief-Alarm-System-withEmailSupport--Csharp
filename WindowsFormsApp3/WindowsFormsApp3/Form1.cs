using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO.Ports;


namespace PIR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
                comboBox1.Items.Add(port);               //Seri portları comboBox1'e ekleme

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived); //DataReceived eventini oluşturma
        }
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort1.ReadLine();
            panel1.BackColor = Color.LightGreen;       //Panel rengini LightGreen yap
            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Credentials = new NetworkCredential("gönderen mail", "şifre");  //Buraya kendi gmail adresinizi ve şifrenizi girin
            SmtpServer.Port = 587;                              //Port Numarası
            SmtpServer.Host = "smtp.live.com";                 //Sunucu adresi
            SmtpServer.EnableSsl = true;                        //SSL ayarı
            MailMessage mail = new MailMessage();
            mail.To.Add("mertfozzy@gmail.com");            //Gönderilecek adres
            mail.From = new MailAddress("mertfozzy@outlook.com", "Arduino - PIR Sensörü");  //Mailin gönderildiği adres ve isim tanımlaması
            mail.Subject = "Hareket Algılandı!";     //Mail konusu
            mail.Body = "Ortamda hareket algılandı!\nLütfen güvenliğinizi kontrol ediniz.";//Mailin body kısmındaki metin
            SmtpServer.Send(mail);  //Maili gönder
                                    // Console.WriteLine(data);

            System.Threading.Thread.Sleep(2000);  //2000 ms bekleme süresi
            panel1.BackColor = Color.Silver;      //Panel rengini Silver yap
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    /* Seri Port Ayarları */
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 9600;
                    serialPort1.Parity = Parity.None;
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open(); //Seri portu aç
                    /* ------------------ */

                    label3.Text = "Bağlantı Sağlandı.";
                    label3.ForeColor = Color.Green;
                    button1.Text = "KES";                 //Buton1 yazısını değiştir
                }
                else
                {
                    label3.Text = "Bağlantı Kesildi.";
                    label3.ForeColor = Color.Red;
                    button1.Text = "BAĞLAN";              //Buton1 yazısını değiştir
                    serialPort1.Close();                 //Seri portu kapa

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");      //Hata mesajı
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();    //Seri port açıksa kapat
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}