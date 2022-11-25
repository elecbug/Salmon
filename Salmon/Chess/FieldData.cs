using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal class FieldData
    {
        private FieldUI parent;
        private Unit?[,] unit_matrix;
        private Unit? choose_unit;
        private List<Point> attack_points;
        private List<Point> move_points;

        public FieldUI FieldUI { get => this.parent; set => this.parent = value; }
        public Unit? Choosed => this.choose_unit;
        public List<Point> AttackPoints => this.attack_points;
        public List<Point> MovePoints => this.move_points;
        public Unit? GetUnit(int x, int y)
        {
            return this.unit_matrix[x, y];
        }

        public FieldData(FieldUI parent)
        {
            this.parent = parent;

            this.attack_points = new List<Point>();
            this.move_points = new List<Point>();

            this.unit_matrix = new Unit[Board.MAXIMUM, Board.MAXIMUM];
            this.choose_unit = null;

            TestCase();
        }

        public Unit? Unit(Point point)
        {
            return this.unit_matrix[point.X, point.Y];
        }
        public Unit? Unit(int x, int y)
        {
            return this.unit_matrix[x, y];
        }

        public void ClickUnit(int x, int y)
        {
            // 최초 선택
            if (this.unit_matrix[x, y] != null && this.choose_unit == null)
            {
                this.choose_unit = this.unit_matrix[x, y];

                this.attack_points = this.choose_unit!.AbleToAttack();
                this.move_points = this.choose_unit!.AbleToMove();
            }
            // 재선택(선택 해제)
            else if (this.choose_unit != null && this.choose_unit.Location == new Point(x, y))
            {
                this.choose_unit = null;

                this.move_points = new List<Point>();
                this.attack_points = new List<Point>();
            }
            else if (this.unit_matrix[x, y] != null && this.choose_unit != null)
            {
                // 공격 시도
                if (this.unit_matrix[x, y]!.Team != this.choose_unit!.Team)
                {
                    if (this.attack_points.Contains(new Point(x, y)))
                    {
                        this.unit_matrix[x, y]!.Kill();

                        // en passant!
                        if (this.choose_unit.GetType() == typeof(Pawn) && this.choose_unit.Location.Y == y
                            && ((this.choose_unit.Team == Team.First && this.unit_matrix[x, y - 1] == null)
                             || (this.choose_unit.Team == Team.First && this.unit_matrix[x, y + 1] == null)))
                        {
                            if (this.choose_unit.Team == Team.First)
                            {
                                this.unit_matrix[x, y - 1] = this.choose_unit!;
                                this.unit_matrix[x, y] = null;
                                this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;

                                this.choose_unit.Location = new Point(x, y - 1);
                            }
                            else if (this.choose_unit.Team == Team.Last)
                            {
                                this.unit_matrix[x, y + 1] = this.choose_unit!;
                                this.unit_matrix[x, y] = null;
                                this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;

                                this.choose_unit.Location = new Point(x, y + 1);
                            }
                        }
                        // 일반 공격
                        else
                        {
                            this.unit_matrix[x, y] = this.choose_unit!;
                            this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;

                            this.choose_unit.Location = new Point(x, y);
                        }

                        this.choose_unit!.IncreaseMove();
                        this.choose_unit = null;

                        this.move_points = new List<Point>();
                        this.attack_points = new List<Point>();
                    }
                    // 사정거리 내에 적이 없을 경우
                    else
                    {
                        this.choose_unit = null;

                        this.move_points = new List<Point>();
                        this.attack_points = new List<Point>();
                    }
                }
                // 선택 변경
                else if (this.unit_matrix[x, y]!.Team == this.choose_unit!.Team)
                {
                    this.choose_unit = this.unit_matrix[x, y];

                    this.attack_points = this.choose_unit!.AbleToAttack();
                    this.move_points = this.choose_unit!.AbleToMove();
                }
            }
            // 이동 시도
            else if (this.unit_matrix[x, y] == null && this.choose_unit != null
                && this.move_points.Contains(new Point(x, y)))
            {
                this.unit_matrix[x, y] = this.choose_unit;
                this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;
                this.choose_unit!.Location = new Point(x, y);

                this.choose_unit!.IncreaseMove();
                this.choose_unit = null;

                this.move_points = new List<Point>();
                this.attack_points = new List<Point>();
            }
            // 맨 땅(그 외)
            else
            {
                this.choose_unit = null;

                this.move_points = new List<Point>();
                this.attack_points = new List<Point>();
            }
        }

        private void MakeUnit(Point location, Type type, Team team)
        {
            switch (type)
            {
                case Type.Pawn:
                    this.unit_matrix[location.X, location.Y] = new Pawn(this, location, team); break;
                case Type.Rook:
                    this.unit_matrix[location.X, location.Y] = new Rook(this, location, team); break;
                case Type.Knight:
                    this.unit_matrix[location.X, location.Y] = new Knight(this, location, team); break;
                case Type.Bishop:
                    this.unit_matrix[location.X, location.Y] = new Bishop(this, location, team); break;
                case Type.Queen:
                    this.unit_matrix[location.X, location.Y] = new Queen(this, location, team); break;
                case Type.King: 
                    this.unit_matrix[location.X, location.Y] = new King(this, location, team); break;
            }
        }
        private void TestCase()
        {
            for (int x = 0; x < 8; x++)
            {
                MakeUnit(new Point(x, 1), Type.Pawn, Team.Last);
                MakeUnit(new Point(x, 6), Type.Pawn, Team.First);
            }

            this.unit_matrix[0, 0] = new Rook(this, new Point(0, 0), Team.Last);
            this.unit_matrix[7, 0] = new Rook(this, new Point(7, 0), Team.Last);
            this.unit_matrix[0, 7] = new Rook(this, new Point(0, 7), Team.First);
            this.unit_matrix[7, 7] = new Rook(this, new Point(7, 7), Team.First);

            this.unit_matrix[1, 0] = new Knight(this, new Point(1, 0), Team.Last);
            this.unit_matrix[6, 0] = new Knight(this, new Point(6, 0), Team.Last);
            this.unit_matrix[1, 7] = new Knight(this, new Point(1, 7), Team.First);
            this.unit_matrix[6, 7] = new Knight(this, new Point(6, 7), Team.First);

            this.unit_matrix[2, 0] = new Bishop(this, new Point(2, 0), Team.Last);
            this.unit_matrix[5, 0] = new Bishop(this, new Point(5, 0), Team.Last);
            this.unit_matrix[2, 7] = new Bishop(this, new Point(2, 7), Team.First);
            this.unit_matrix[5, 7] = new Bishop(this, new Point(5, 7), Team.First);

            this.unit_matrix[4, 0] = new Queen(this, new Point(4, 0), Team.Last);
            this.unit_matrix[4, 7] = new Queen(this, new Point(4, 7), Team.First);

            this.unit_matrix[3, 0] = new King(this, new Point(3, 0), Team.Last);
            this.unit_matrix[3, 7] = new King(this, new Point(3, 7), Team.First);
        }
    }
}
