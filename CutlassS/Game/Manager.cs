using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutlassC.Game
{
    internal class Manager
    {
        private CutlassShare.Card.Object[] cards;

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
    }
}
