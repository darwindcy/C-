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
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void SetTextCallback(String text);
        delegate void SetIntCallbCk(int theadnum);

        TcpListener listener = new TcpListener(IPAddress.Any, 9801);

        public MainWindow()
        {
            InitializeComponent();
        }
        Socket currentClient;
       
        NetworkStream[] ns = new NetworkStream[100];
        StreamReader[] sr = new StreamReader[100];
        StreamWriter[] sw = new StreamWriter[100];
        List<int> AvailableClients = new List<int>(100);
        List<int> UsedClientNumbers = new List<int>(100);
        int clientcount = 0;
        BackgroundWorker bwMain = new BackgroundWorker();
        BackgroundWorker[] bw1 = new BackgroundWorker[100]; 
        

        private void WriteToScreen(string message)
        {
            if (this.txtScreen.Dispatcher.CheckAccess())
            {
                if (txtScreen.Text.Length != 0)
                    txtScreen.Text += "\n";
                txtScreen.Text += message;
            } else
            {
                txtScreen.Dispatcher.BeginInvoke(new SetTextCallback(WriteToScreen), message);
            }
        }

        private void bwMain_IDontWantToWorkk(object sender, DoWorkEventArgs e)
        {
            string toPrint;
            TcpListener newSocket = new TcpListener(IPAddress.Any, 9801); // Create a TCP listencer
            
            newSocket.Start();
            
            for(int i = 0; i < 100; i++)
            {
                AvailableClients.Add(i);
            }

            while(AvailableClients.Count > 0)
            {
                if (UsedClientNumbers.Count <= 5)
                {
                    WriteToScreen("Waiting For Client");  // waiting for connection
                    toPrint = "Available Clients = " + AvailableClients.Count;
                    WriteToScreen(toPrint);             // waiting for connection

                    currentClient = newSocket.AcceptSocket();          //Accept connection

                    clientcount = AvailableClients.First();
                    AvailableClients.Remove(clientcount);
                    ns[clientcount] = new NetworkStream(currentClient);
                    sr[clientcount] = new StreamReader(ns[clientcount]);
                    sw[clientcount] = new StreamWriter(ns[clientcount]);

                    string welcome = "Welcome";
                    WriteToScreen("Client Connected");

                    addActiveConnections("Client Number " + clientcount.ToString());

                    sw[clientcount].WriteLine(welcome);
                    sw[clientcount].Flush();

                    bw1[clientcount] = new BackgroundWorker();
                    bw1[clientcount].DoWork += new DoWorkEventHandler(client_DoesntWantToWork);

                    bw1[clientcount].RunWorkerAsync(clientcount);
                    UsedClientNumbers.Add(clientcount);
                } 
                else
                {
                    WriteToScreen("Maximum number of connections Reached");
                }
            }
        }

        private void client_DoesntWantToWork(object sender, DoWorkEventArgs e)
        {
            
            int clientNum = (int)e.Argument;
            //MessageBox.Show("Client Connected to and Doing work " + clientNum);
            bw1[clientNum].WorkerSupportsCancellation = true;

            while (true)
            {
                string inputStream;
                try
                {
                    inputStream = sr[clientNum].ReadLine();
                    WriteToScreen(inputStream);
                    if(inputStream == "disconnect"){
                        sr[clientNum].Close();
                        sw[clientNum].Close();
                        ns[clientNum].Close();
                        WriteToScreen("Client " + clientNum + " has disconnected");
                        KillMe(clientNum);
                        break;
                    }
                } 
                catch
                {
                    sr[clientNum].Close();
                    sw[clientNum].Close();
                    ns[clientNum].Close();
                    WriteToScreen("Client " + clientNum + " has disconnected");
                    KillMe(clientNum);
                }
            }
        }

        private void addActiveConnections(string clientName)
        {
            if (this.txtScreen.Dispatcher.CheckAccess())
            {
                activeConnections.Items.Insert(0, clientName);
            }
            else
            {
                txtScreen.Dispatcher.BeginInvoke(new SetTextCallback(addActiveConnections), clientName);
            }
            
        }
        private void KillMe(int threadnum)
        {
            UsedClientNumbers.Remove(threadnum);
            AvailableClients.Add(threadnum);
            if (bw1[threadnum] != null && bw1[threadnum].IsBusy)
            {
                bw1[threadnum].CancelAsync();
                bw1[threadnum].Dispose();
            }
            bw1[threadnum] = null;
            GC.Collect();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            foreach (int t in UsedClientNumbers)
            {
                sw[t].WriteLine(txtData.Text);

                WriteToScreen("You Said:    " + txtData.Text);

                sw[t].Flush();
            }
            txtData.Text = "";
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            bwMain.DoWork += new DoWorkEventHandler(bwMain_IDontWantToWorkk);
            if (!bwMain.IsBusy)
                bwMain.RunWorkerAsync("Hello Worker");
            else
                WriteToScreen("The Dude's already Started, How much do you want it to Work !!!");
        }
    }
}
