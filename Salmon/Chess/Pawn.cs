using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class Pawn : Unit
    {
        public Pawn(Field game_board, Point location, Team team, bool is_alive = true, bool is_moved = false)
            : base(game_board, location, Type.Pawn, team, is_alive) { }

        public override List<Point> AbleToMove()
        {
            List<Point> result = new List<Point>();

            if (this.team == Team.First)
            {
                Point p1 = new Point(this.location.X, this.location.Y - 1);
                Point p2 = new Point(this.location.X, this.location.Y - 2);

                if (Board.IsInside(p1) && this.game_board.Unit(p1) == null)
                {
                    result.Add(p1);

                    if (Board.IsInside(p2) && this.game_board.Unit(p2) == null && this.move_count == 0)
                    {
                        result.Add(p2);
                    }
                }
            }
            else if (this.team == Team.Last)
            {
                Point p1 = new Point(this.location.X, this.location.Y + 1);
                Point p2 = new Point(this.location.X, this.location.Y + 2);

                if (Board.IsInside(p1) && this.game_board.Unit(p1) == null)
                {
                    result.Add(p1);

                    if (Board.IsInside(p2) && this.game_board.Unit(p2) == null && this.move_count == 0)
                    {
                        result.Add(p2);
                    }
                }
            }

            return result;
        }
        public override List<Point> AbleToAttack()
        {
            List<Point> result = new List<Point>();

            if (this.team == Team.First)
            {
                Point p1 = new Point(this.location.X + 1, this.location.Y - 1);
                Point p2 = new Point(this.location.X - 1, this.location.Y - 1);

                if (Board.IsInside(p1) && this.game_board.Unit(p1) != null 
                    && this.game_board.Unit(p1)!.Team != this.team)
                {
                    result.Add(p1);
                }
                if (Board.IsInside(p2) && this.game_board.Unit(p2) != null
                    && this.game_board.Unit(p2)!.Team != this.team)
                {
                    result.Add(p2);
                }
            }
            else if (this.team == Team.Last)
            {
                Point p1 = new Point(this.location.X + 1, this.location.Y + 1);
                Point p2 = new Point(this.location.X - 1, this.location.Y + 1);

                if (Board.IsInside(p1) && this.game_board.Unit(p1) != null
                    && this.game_board.Unit(p1)!.Team != this.team)
                {
                    result.Add(p1);
                }
                if (Board.IsInside(p2) && this.game_board.Unit(p2) != null
                    && this.game_board.Unit(p2)!.Team != this.team)
                {
                    result.Add(p2);
                }
            }

            // En Passant
            if (this.location.Y == Board.MINIMUM + 3 || this.location.Y == Board.MAXIMUM - 4)
            {
                Point p1 = new Point(this.location.X + 1, this.location.Y);
                Point p2 = new Point(this.location.X - 1, this.location.Y);

                if (Board.IsInside(p1) && this.game_board.Unit(p1) != null
                    && this.game_board.Unit(p1)!.Team != this.team && this.game_board.Unit(p1)!.Type == Type.Pawn
                    && this.game_board.Unit(p1)!.MoveCount == 1)
                {
                    result.Add(p1);
                }
                if (Board.IsInside(p2) && this.game_board.Unit(p2) != null
                    && this.game_board.Unit(p2)!.Team != this.team && this.game_board.Unit(p2)!.Type == Type.Pawn
                    && this.game_board.Unit(p2)!.MoveCount == 1)
                {
                    result.Add(p2);
                }
            }

            return result;
        }
    }
}
