namespace CutlassC.Game
{
    internal class Manager
    {
        private CutlassShare.Card.Object[] cards;
        private Socket.Client? admin;

        public Manager(bool defaults = true)
        {
            this.cards = new CutlassShare.Card.Object[66];
         
            if (defaults)
            {
                int i = 0;

                for (int n = 1; n <= 13; n++)
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.Red, n);
                    i++;
                }
                for (int n = 1; n <= 13; n++)
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.Yellow, n);
                    i++;
                }
                for (int n = 1; n <= 13; n++)
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.Blue, n);
                    i++;
                }
                for (int n = 1; n <= 13; n++)
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.Black, n);
                    i++;
                }
                for (int n = 0; n < 5; n++)
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.Pirate, 0);
                    i++;
                }
                for (int n = 0; n < 2; n++)
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.Mermaid, 0);
                    i++;
                }
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.ScaryMary, 0);
                    i++;
                }
                {
                    this.cards[i] = CutlassShare.Card.Object.Card(i, CutlassShare.Card.Type.SkullKing, 0);
                    i++;
                }
            }
        }

        public void Run()
        {
            this.admin = Socket.Server.Instance().AdminClient;

            for (int i = 1; i <= 10; i++)
            {
                PlayGame(i);
            }
        }

        private void PlayGame(int level)
        {
            this.admin!.Send(CutlassShare.Protocall.Token.GameStart + CutlassShare.Protocall.Token.Splitter +
                             CutlassShare.Protocall.Token.Level + level + CutlassShare.Protocall.Token.Splitter);

            int starter = new Random(DateTime.Now.Millisecond).Next(1, Socket.Server.Instance().Clients.Count);
            
            for (int i = 0; i < level; i++)
            {
                starter = PlayTurn(starter);
            }
        }

        private int PlayTurn(int starter)
        {
            this.admin!.Send(CutlassShare.Protocall.Token.TurnStart + CutlassShare.Protocall.Token.Splitter +
                             CutlassShare.Protocall.Token.Player + starter + CutlassShare.Protocall.Token.Splitter);

            return 1;

        }
    }
}
