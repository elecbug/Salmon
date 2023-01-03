using CutlassS.Socket;

namespace CutlassS.Command
{
    public class Manager
    {
        private static Manager result = new Manager();
        private Server? server;

        public string Command { get; set; }

        private Manager()
        {
            this.Command = "";
        }

        public static Manager Instance()
        {
            return Manager.result;
        }

        public string ExcuteCommand()
        {
            string[] token = this.Command.Trim('\r', '\n').Split(' ');

            try
            {
                if (token[0] == Token.Server)
                {
                    if (token[1] == Token.Start)
                    {
                        this.server = Server.Instance();
                        this.server.Run();

                        return "Server is started now.";
                    }
                    else if (token[1] == Token.Stop)
                    {
                        this.server!.Stop();

                        return "Server is stoped now.";
                    }
                    else if (token[1] == Token.Test)
                    {
                        Client client1 = new Client("Lee", "127.0.0.1", 9764);
                        Client client2 = new Client("Park", "127.0.0.1", 9764);
                        Client client3 = new Client("Choi", "127.0.0.1", 9764);

                        client1.Send("hi");
                        client2.Send("Hello");

                        return "Tested now server.";
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}