using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;

namespace client1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bw1.DoWork += new DoWorkEventHandler(bw1_I_DONT_WANT_TO_WORK);
            //bw1.RunWorkerAsync("Message to Worker");
        }

        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        delegate void SetTextCallback(String text);
        BackgroundWorker bw1 = new BackgroundWorker();

        private void bw1_I_DONT_WANT_TO_WORK(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                try
                {
                    string inputStream = sr.ReadLine();       // Read onyl reads into byte array
                    
                    WriteToScreen("Server said:    " + inputStream);

                    if(inputStream == "disconnect")
                    {
                        sw.WriteLine("disconnect");
                        sw.Flush();
                        sr.Close();
                        sw.Close();
                        if (ns != null)
                            ns.Close();

                        System.Environment.Exit(System.Environment.ExitCode); //close all 

                        break;
                    }
                }
                catch
                {
                    ns.Close();
                    System.Environment.Exit(System.Environment.ExitCode); //close all 
                }
            }
        }
        private void WriteToScreen(string message)
        {
            if (this.txtScreen.Dispatcher.CheckAccess())
            {
                if (txtScreen.Text.Length != 0)
                    txtScreen.Text += "\n";
                txtScreen.Text += message;
            }
            else
            {
                txtScreen.Dispatcher.BeginInvoke(new SetTextCallback(WriteToScreen), message);
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            TcpClient newConnection = new TcpClient();
            try
            {
                newConnection.Connect("127.0.0.1", 9801);

                ns = newConnection.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                bw1.RunWorkerAsync("Message to Worker");
            } 
            catch
            {
                WriteToScreen("Cannot connect to the Specified address; Connection cannot be made Sorry");
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            sw.WriteLine(txtSend.Text);

            WriteToScreen("You Said:    " + txtSend.Text);
            
            sw.Flush();
            if(txtSend.Text == "disconnect")
            {
                sw.Close();
                sr.Close();
                ns.Close();
                System.Environment.Exit(System.Environment.ExitCode);
            }
            txtSend.Text = "";
            
        }
    }
}
