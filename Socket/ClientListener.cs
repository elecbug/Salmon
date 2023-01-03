using System.Diagnostics;
using System.Net.Sockets;

namespace Socket
{
    public class ClientListener
    {
        private StreamReader reader;
        private Thread thread;
        private bool run;

        public System.Net.Sockets.Socket Client { get; private set; }

        public ClientListener(System.Net.Sockets.Socket client)
        {
            this.Client = client;
            this.run = true;
            this.reader = new StreamReader(new NetworkStream(client));

            this.thread = new Thread(new ThreadStart(Run));
            this.thread.Start();
        }

        private async void Run()
        {
            while (this.run)
            {
                string? line = "";

                while (line == "")
                {
                    line = await this.reader.ReadLineAsync();

                    if (line == null)
                    {
                        Server.Instance().Clients.Remove(this);
                        Debug.WriteLine("client is log off");

                        return;
                    }
                }

                if (line.EndsWith("access"))
                {
                    Server.Instance().Names.Add(line.Split('$')[0]);
                }
                else
                {
                    for (int i = 0; i < Server.Instance().Clients.Count; i++)
                    {
                        ClientListener client = Server.Instance().Clients[i];
                        NetworkStream network_stream = new NetworkStream(client.Client);
                        StreamWriter writer = new StreamWriter(network_stream);

                        writer.WriteLine(line);
                        writer.Flush();
                    }
                }
            }
        }

        public void Stop()
        {
            Debug.WriteLine("stop listener");
            lock (this)
            {
                this.run = false;
            }
        }
    }
}
