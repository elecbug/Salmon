using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class GameManager
    {
        private Form parent;
        private FieldUI front;
        private Team turn;

        public Team Turn { get => this.turn; }

        public GameManager(Form parent)
        {
            this.front = new FieldUI(this, parent.ClientSize)
            {
                Parent = parent,
                Visible = true,
                Dock = DockStyle.Fill,
            };
            
            this.parent = parent;
            this.turn = Team.First;
        }

        
    }
}
