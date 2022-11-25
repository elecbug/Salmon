using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal abstract class Unit : IControl
    {
        protected FieldData game_board;
        protected Point location;
        protected Type type;
        protected Team team;
        protected bool is_alived;
        protected int move_count;

        public FieldData GameBoard { get => this.game_board; }
        public Point Location { get => this.location; set => this.location = value; }
        public Type Type { get => this.type; }
        public Team Team { get => this.team; }
        public int MoveCount { get => this.move_count; }

        public Unit(FieldData game_board, Point location, Type type, Team team, bool is_alived = true)
        {
            this.game_board = game_board;
            this.location = location;
            this.type = type;
            this.team = team;
            this.is_alived = is_alived;
            this.move_count = 0;
        }
        
        public void Kill() => this.is_alived = false;
        public void IncreaseMove() => this.move_count++;

        public abstract List<Point> AbleToMove(); 
        public abstract List<Point> AbleToAttack();
    }
}
