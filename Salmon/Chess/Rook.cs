﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class Rook : Unit
    {
        public Rook(Field game_board, Point location, Team team, bool is_alive = true)
            : base(game_board, location, Type.Rook, team, is_alive) { }

        public override List<Point> AbleToMove()
        {
            List<Point> result = new List<Point>();

            Point left = new Point(this.location.X - 1, this.location.Y);
            Point right = new Point(this.location.X + 1, this.location.Y);
            Point up = new Point(this.location.X, this.location.Y - 1);
            Point down = new Point(this.location.X, this.location.Y + 1);

            for (int i = 0; i < Board.MAXIMUM; i++)
            {
                if (Board.IsInside(left) && this.game_board.Unit(left) == null)
                {
                    result.Add(left);
                    left = new Point(left.X - 1, left.Y);
                }
                if (Board.IsInside(right) && this.game_board.Unit(right) == null)
                {
                    result.Add(right);
                    right = new Point(right.X + 1, right.Y);
                }
                if (Board.IsInside(up) && this.game_board.Unit(up) == null)
                {
                    result.Add(up);
                    up = new Point(up.X, up.Y - 1);
                }
                if (Board.IsInside(down) && this.game_board.Unit(down) == null)
                {
                    result.Add(down);
                    down = new Point(down.X, down.Y + 1);
                }
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

            for (int i = 0; i < Board.MAXIMUM; i++)
            {
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
            }

            return result;
        }
    }
}