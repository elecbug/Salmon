using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace CutlassS.Socket
{
    public class Server
    {
        public List<ClientListener> Clients { get; private set; }

        private Thread thread;
        private TcpListener tcp_listener;
        private static Server result = new Server();
        private bool run;

        public static Server Instance()
        {
            return Server.result;
        }

        private Server(string address = "127.0.0.1", int port = 9764)
        {
            this.Clients = new List<ClientListener>();
            this.thread = new Thread(new ThreadStart(delegate { Run(address, port); }));
            this.run = true;
            this.tcp_listener = new TcpListener(IPAddress.Parse(address), port);
        }

        public void Run()
        {
            this.thread.Start();
        }

        private void Run(string address, int port)
        {
            this.tcp_listener.Start();

            while (this.run)
            {
                if (this.tcp_listener.Pending())
                {
                    System.Net.Sockets.Socket client = this.tcp_listener.AcceptSocket();
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint!;

                    Debug.WriteLine("Access by adress {0}", ip);

                    ClientListener client_thread = new ClientListener(client);
                    this.Clients.Add(client_thread);
                }
            }
        }

        public void Stop()
        {
            this.run = false;

            for (int i = 0; i < this.Clients.Count; i++)
            {
                this.Clients[i].Stop();
            }

            Debug.WriteLine("stop server");

            this.tcp_listener.Stop();
        }
    }
}
