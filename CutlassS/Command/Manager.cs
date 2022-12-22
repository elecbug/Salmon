using CutlassS.Socket;

namespace CutlassS.Command
{
    public class Manager
    {
        private static Manager result = new Manager();

        public string Command { get; set; }

        private Manager()
        {
            Command = "";
        }

        public static Manager Instance()
        {
            return result;
        }

        public string ExcuteCommand()
        {
            string[] token = Command.Trim('\r', '\n').Split(' ');

            try
            {
                if (token[0] == Token.Server)
                {
                    if (token[1] == Token.Start)
                    {
                        Server server = Server.Instance();
                        server.Run();

                        Client client1 =new Client("Lee", "127.0.0.1", 9764);
                        Client client2 = new Client("Park", "127.0.0.1", 9764);
                        Client client3 = new Client("Choi", "127.0.0.1", 9764);

                        client1.Send("hi");
                        server.Stop();
                        client2.Send("Hello");

                        return "Server is started now.";
                    }
                    else if (token[1] == Token.Stop)
                    {
                        return "Server is stoped now.";
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