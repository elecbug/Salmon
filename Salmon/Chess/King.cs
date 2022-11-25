using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class King : Unit
    {
        public King(FieldData game_board, Point location, Team team, bool is_alive = true)
            : base(game_board, location, Type.King, team, is_alive) { }

        public override List<Point> AbleToMove()
        {
            List<Point> result = new List<Point>();

            Point left = new Point(this.location.X - 1, this.location.Y);
            Point right = new Point(this.location.X + 1, this.location.Y);
            Point up = new Point(this.location.X, this.location.Y - 1);
            Point down = new Point(this.location.X, this.location.Y + 1);
            Point left_up = new Point(this.location.X - 1, this.location.Y - 1);
            Point right_up = new Point(this.location.X + 1, this.location.Y - 1);
            Point left_down = new Point(this.location.X - 1, this.location.Y + 1);
            Point right_down = new Point(this.location.X + 1, this.location.Y + 1);

            if (Board.IsInside(left) && this.game_board.Unit(left) == null)
            {
                result.Add(left);
            }
            if (Board.IsInside(right) && this.game_board.Unit(right) == null)
            {
                result.Add(right);
            }
            if (Board.IsInside(up) && this.game_board.Unit(up) == null)
            {
                result.Add(up);
            }   
            if (Board.IsInside(down) && this.game_board.Unit(down) == null)
            {
                result.Add(down);
            }
            if (Board.IsInside(left_up) && this.game_board.Unit(left_up) == null)
            {
                result.Add(left_up);
            }
            if (Board.IsInside(right_up) && this.game_board.Unit(right_up) == null)
            {
                result.Add(right_up);
            }
            if (Board.IsInside(left_down) && this.game_board.Unit(left_down) == null)
            {
                result.Add(left_down);
            }
            if (Board.IsInside(right_down) && this.game_board.Unit(right_down) == null)
            {
                result.Add(right_down);
            }

            return result;
        }
        public override List<Point> AbleToAttack()
        {
            List<Point> result = new List<Point>();

            Point left = new Point(this.location.X - 1, this.location.Y);
            Point right = new Point(this.location.X + 1, this.location.Y);
            Point up = new Point(this.location.X, this.location.Y - 1);
            Point down = new Point(this.location.X, this.location.Y + 1);
            Point left_up = new Point(this.location.X - 1, this.location.Y - 1);
            Point right_up = new Point(this.location.X + 1, this.location.Y - 1);
            Point left_down = new Point(this.location.X - 1, this.location.Y + 1);
            Point right_down = new Point(this.location.X + 1, this.location.Y + 1);

            if (Board.IsInside(left) && this.game_board.Unit(left) == null)
            {
                left = new Point(left.X - 1, left.Y);
            }
            else if (Board.IsInside(left) && this.game_board.Unit(left) != null
                && this.game_board.Unit(left)!.Team != this.team)
            {
                result.Add(left);
            }
            if (Board.IsInside(right) && this.game_board.Unit(right) == null)
            {
                right = new Point(right.X + 1, right.Y);
            }
            else if (Board.IsInside(right) && this.game_board.Unit(right) != null
                && this.game_board.Unit(right)!.Team != this.team)
            {
                result.Add(right);
            }
            if (Board.IsInside(up) && this.game_board.Unit(up) == null)
            {
                up = new Point(up.X, up.Y - 1);
            }
            else if (Board.IsInside(up) && this.game_board.Unit(up) != null
                && this.game_board.Unit(up)!.Team != this.team)
            {
                result.Add(up);
            }
            if (Board.IsInside(down) && this.game_board.Unit(down) == null)
            {
                down = new Point(down.X, down.Y + 1);
            }
            else if (Board.IsInside(down) && this.game_board.Unit(down) != null
                && this.game_board.Unit(down)!.Team != this.team)
            {
                result.Add(down);
            }
            if (Board.IsInside(left_up) && this.game_board.Unit(left_up) == null)
            {
                left_up = new Point(left_up.X - 1, left_up.Y - 1);
            }
            else if (Board.IsInside(left_up) && this.game_board.Unit(left_up) != null
                && this.game_board.Unit(left_up)!.Team != this.team)
            {
                result.Add(left_up);
            }
            if (Board.IsInside(right_up) && this.game_board.Unit(right_up) == null)
            {
                right_up = new Point(right_up.X + 1, right_up.Y - 1);
            }
            else if (Board.IsInside(right_up) && this.game_board.Unit(right_up) != null
                && this.game_board.Unit(right_up)!.Team != this.team)
            {
                result.Add(right_up);
            }
            if (Board.IsInside(left_down) && this.game_board.Unit(left_down) == null)
            {
                left_down = new Point(left_down.X - 1, left_down.Y + 1);
            }
            else if (Board.IsInside(left_down) && this.game_board.Unit(left_down) != null
                && this.game_board.Unit(left_down)!.Team != this.team)
            {
                result.Add(left_down);
            }
            if (Board.IsInside(right_down) && this.game_board.Unit(right_down) == null)
            {
                right_down = new Point(right_down.X + 1, right_down.Y + 1);
            }
            else if (Board.IsInside(right_down) && this.game_board.Unit(right_down) != null
                && this.game_board.Unit(right_down)!.Team != this.team)
            {
                result.Add(right_down);
            }

            return result;
        }
    }
}
