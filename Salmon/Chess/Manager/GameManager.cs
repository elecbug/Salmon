﻿using System;
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
        private FieldData back;
        private Team turn;

        public Team Turn { get => this.turn; }
        public FieldData Data => this.back;

        public GameManager(Form parent)
        {
            this.back = new FieldData(this);
            this.front = new FieldUI(this, parent.ClientSize)
            {
                Parent = parent,
                Visible = true,
                Dock = DockStyle.Fill,
            };
            
            this.parent = parent;
            this.turn = Team.First;
        }

        public void ChangeTurn()
        {
            this.turn = (this.turn == Team.First ? Team.Last : Team.First);
            
            switch (this.back.IsMated())
            {
                case GameState.FirstWin: MessageBox.Show("First Win!"); break;
                case GameState.LastWin: MessageBox.Show("Last Win!"); break;
                case GameState.Draw: MessageBox.Show("Draw..."); break;
            }
        }
    }
}
