namespace CutlassS.Command
{
    public class Manager
    {
        private static Manager result = new Manager();
        private Socket.Server? server;

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
                        this.server = Socket.Server.Instance();
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
                        Socket.Client client1 = new Socket.Client("Lee", "127.0.0.1", 9764);
                        Socket.Client client2 = new Socket.Client("Park", "127.0.0.1", 9764);
                        Socket.Client client3 = new Socket.Client("Choi", "127.0.0.1", 9764);

                        client1.Send("hi");
                        client2.Send("Hello");

                        return "Tested now server.";
                    }
                    else if (token[1] == Token.Names)
                    {
                        string result = "";

                        foreach(string str in this.server!.Names)
                        {
                            result += "\r\n        >> " + str;
                        }

                        return result;
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