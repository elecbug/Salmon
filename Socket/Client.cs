using System.Diagnostics;
using System.Net.Sockets;

namespace CutlassS.Socket
{
    public class Client
    {
        private StreamReader? reader;
        private StreamWriter? writer;
        private Thread? thread;
        private bool run;

        public string Name { get; private set; }

        public Client(string name, string net_adress, int port)
        {
            this.Name = name;
            this.run = true;

            try
            {
                TcpClient server = new TcpClient(net_adress, port);
                Debug.WriteLine("Get server");


                NetworkStream network_stream = server.GetStream();
                this.reader = new StreamReader(network_stream);
                this.writer = new StreamWriter(network_stream);

                this.thread = new Thread(RunRead);
                this.thread.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
        }

        private async void RunRead()
        {
            try
            {
                while (this.run)
                {
                    string? line = "";

                    while (line.Equals(""))
                    {
                        line = await this.reader!.ReadLineAsync();

                        if (line == null)
                        {
                            Debug.WriteLine("Cut accessed to server");
                            return;
                        }
                    }

                    Debug.WriteLine(this.Name + " is get " + line);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Send(string message)
        {
            try
            {
                this.writer!.WriteLine(this.Name + ">" + message);
                this.writer!.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            lock (this)
            {
                Debug.WriteLine("stop client");
                this.run = false;
            }
        }
    }
}
