using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class Knight : Unit
    {
        public Knight(Field game_board, Point location, Team team, bool is_alive = true)
            : base(game_board, location, Type.Knight, team, is_alive) { }

        public override List<Point> AbleToMove()
        {
            List<Point> result = new List<Point>();

            Point[] p =
            {
                new Point(this.location.X - 1, this.location.Y - 2),
                new Point(this.location.X - 2, this.location.Y - 1),
                new Point(this.location.X - 1, this.location.Y + 2),
                new Point(this.location.X - 2, this.location.Y + 1),
                new Point(this.location.X + 1, this.location.Y - 2),
                new Point(this.location.X + 2, this.location.Y - 1),
                new Point(this.location.X + 1, this.location.Y + 2),
                new Point(this.location.X + 2, this.location.Y + 1),
            };
            for (int i = 0; i < p.Length; i++)
            {
                if (Board.IsInside(p[i]) && this.game_board.Unit(p[i]) == null)
                {
                    result.Add(p[i]);
                }
            }

            return result;
        }
        public override List<Point> AbleToAttack()
        {

            List<Point> result = new List<Point>();

            Point[] p =
            {
                new Point(this.location.X - 1, this.location.Y - 2),
                new Point(this.location.X - 2, this.location.Y - 1),
                new Point(this.location.X - 1, this.location.Y + 2),
                new Point(this.location.X - 2, this.location.Y + 1),
                new Point(this.location.X + 1, this.location.Y - 2),
                new Point(this.location.X + 2, this.location.Y - 1),
                new Point(this.location.X + 1, this.location.Y + 2),
                new Point(this.location.X + 2, this.location.Y + 1),
            };
            for (int i = 0; i < p.Length; i++)
            {
                if (Board.IsInside(p[i]) && this.game_board.Unit(p[i]) != null
                    && this.game_board.Unit(p[i])!.Team != this.team)
                {
                    result.Add(p[i]);
                }
            }

            return result;

        }
    }
}
