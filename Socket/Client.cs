using System.Diagnostics;
using System.Net.Sockets;

namespace Socket
{
    public class Client
    {
        private StreamReader? reader;
        private StreamWriter? writer;
        private Thread? thread;
        private bool run;
        public bool AdminMode { get; private set; }

        public string Name { get; private set; }

        public Client(string name, string net_adress, int port, bool admin_mode = false)
        {
            this.Name = name + "#" + new Random(DateTime.Now.Millisecond).Next(0, 1000000);
            this.run = true;
            this.AdminMode = admin_mode;

            try
            {
                TcpClient server = new TcpClient(net_adress, port);
                Debug.WriteLine("Get server");

                NetworkStream network_stream = server.GetStream();

                this.reader = new StreamReader(network_stream);
                this.writer = new StreamWriter(network_stream);

                this.thread = new Thread(RunRead);
                this.thread.Start();

                Send("access");
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

                    Debug.WriteLine(this.Name + "is get:" + line);
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
                this.writer!.WriteLine(this.Name + "$$" + message);
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
