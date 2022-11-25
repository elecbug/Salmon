using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class Bishop : Unit
    {
        public Bishop(FieldData game_board, Point location, Team team, bool is_alive = true)
            : base(game_board, location, Type.Bishop, team, is_alive) { }

        public override List<Point> AbleToMove()
        {
            List<Point> result = new List<Point>();

            Point left_up = new Point(this.location.X - 1, this.location.Y - 1);
            Point right_up = new Point(this.location.X + 1, this.location.Y - 1);
            Point left_down = new Point(this.location.X - 1, this.location.Y + 1);
            Point right_down = new Point(this.location.X + 1, this.location.Y + 1);

            for (int i = 0; i < FieldData.MAXIMUM; i++)
            {
                if (FieldData.IsInside(left_up) && this.game_board.Unit(left_up) == null)
                {
                    result.Add(left_up);
                    left_up = new Point(left_up.X - 1, left_up.Y - 1);
                }
                if (FieldData.IsInside(right_up) && this.game_board.Unit(right_up) == null)
                {
                    result.Add(right_up);
                    right_up = new Point(right_up.X + 1, right_up.Y - 1);
                }
                if (FieldData.IsInside(left_down) && this.game_board.Unit(left_down) == null)
                {
                    result.Add(left_down);
                    left_down = new Point(left_down.X - 1, left_down.Y + 1);
                }
                if (FieldData.IsInside(right_down) && this.game_board.Unit(right_down) == null)
                {
                    result.Add(right_down);
                    right_down = new Point(right_down.X + 1, right_down.Y + 1);
                }
            }

            return result;
        }
        public override List<Point> AbleToAttack()
        {
            List<Point> result = new List<Point>();

            Point left_up = new Point(this.location.X - 1, this.location.Y - 1);
            Point right_up = new Point(this.location.X + 1, this.location.Y - 1);
            Point left_down = new Point(this.location.X - 1, this.location.Y + 1);
            Point right_down = new Point(this.location.X + 1, this.location.Y + 1);

            for (int i = 0; i < FieldData.MAXIMUM; i++)
            {
                if (FieldData.IsInside(left_up) && this.game_board.Unit(left_up) == null)
                {
                    left_up = new Point(left_up.X - 1, left_up.Y - 1);
                }
                else if (FieldData.IsInside(left_up) && this.game_board.Unit(left_up) != null
                    && this.game_board.Unit(left_up)!.Team != this.team)
                {
                    result.Add(left_up);
                }
                if (FieldData.IsInside(right_up) && this.game_board.Unit(right_up) == null)
                {
                    right_up = new Point(right_up.X + 1, right_up.Y - 1);
                }
                else if (FieldData.IsInside(right_up) && this.game_board.Unit(right_up) != null
                    && this.game_board.Unit(right_up)!.Team != this.team)
                {
                    result.Add(right_up);
                }
                if (FieldData.IsInside(left_down) && this.game_board.Unit(left_down) == null)
                {
                    left_down = new Point(left_down.X - 1, left_down.Y + 1);
                }
                else if (FieldData.IsInside(left_down) && this.game_board.Unit(left_down) != null
                    && this.game_board.Unit(left_down)!.Team != this.team)
                {
                    result.Add(left_down);
                }
                if (FieldData.IsInside(right_down) && this.game_board.Unit(right_down) == null)
                {
                    right_down = new Point(right_down.X + 1, right_down.Y + 1);
                }
                else if (FieldData.IsInside(right_down) && this.game_board.Unit(right_down) != null
                    && this.game_board.Unit(right_down)!.Team != this.team)
                {
                    result.Add(right_down);
                }
            }

            return result;
        }
    }
}
