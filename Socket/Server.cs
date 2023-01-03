using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Socket
{
    public class Server
    {
        private Thread? thread;
        private TcpListener? tcp_listener;
        private static Server result = new Server();
        private bool run;

        public List<ClientListener> Clients { get; private set; }
        public Client? AdminClient { get; private set; }
        public List<string> Names { get; private set; }

        public static Server Instance()
        {
            return Server.result;
        }

        private Server()
        {
            this.Names = new List<string>();
            this.Clients = new List<ClientListener>();
        }

        public void Run(string address = "127.0.0.1", int port = 9764)
        {
            this.thread = new Thread(new ThreadStart(delegate { InRun(address, port); }));
            this.run = true;
            this.tcp_listener = new TcpListener(IPAddress.Parse(address), port);

            this.thread.Start();

            this.AdminClient = new Client("@admin", address, port, true);
        }

        private void InRun(string address, int port)
        {
            try
            {
                this.tcp_listener!.Start();

                while (this.run)
                {
                    if (this.tcp_listener.Pending())
                    {
                        System.Net.Sockets.Socket client = this.tcp_listener.AcceptSocket();
                        IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint!;

                        Debug.WriteLine("Access by address {0}", ip);

                        ClientListener client_thread = new ClientListener(client);
                        this.Clients.Add(client_thread);
                    }
                }
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
                try
                {
                    this.run = false;

                    for (int i = 0; i < this.Clients.Count; i++)
                    {
                        this.Clients[i].Stop();
                    }

                    Debug.WriteLine("stop server");

                    this.tcp_listener!.Stop();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
