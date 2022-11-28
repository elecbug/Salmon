using Salmon.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Salmon.Chess
{
    internal class FieldData
    {
        public const int MINIMUM = 0, MAXIMUM = 8;
        public static bool IsInside(Point point)
            => point.X >= FieldData.MINIMUM && point.Y >= FieldData.MINIMUM
            && point.X < FieldData.MAXIMUM && point.Y < FieldData.MAXIMUM;

        private GameManager manager;
        private Unit?[,] unit_matrix;
        private Unit? choose_unit;
        private List<Point> attack_points;
        private List<Point> move_points;
        private King? first_king, last_king;

        public Unit? Choosed => this.choose_unit;
        public List<Point> AttackPoints => this.attack_points;
        public List<Point> MovePoints => this.move_points;

        public FieldData(GameManager manager)
        {
            this.manager = manager;

            this.attack_points = new List<Point>();
            this.move_points = new List<Point>();

            this.unit_matrix = new Unit[FieldData.MAXIMUM, FieldData.MAXIMUM];
            this.choose_unit = null;

            DefaultCase();
            //TestCase();
        }
        public FieldData(FieldData target)
        {
            this.manager = target.manager;
            this.unit_matrix = new Unit?[FieldData.MAXIMUM, FieldData.MAXIMUM];
            for (int x = FieldData.MINIMUM; x < FieldData.MAXIMUM; x++)
            {
                for (int y = FieldData.MINIMUM; y < FieldData.MAXIMUM; y++)
                {
                    this.unit_matrix[x, y] = target.unit_matrix[x, y]?.Clone();
                }
            }
            if (target.choose_unit != null)
            {
                this.choose_unit 
                    = target.unit_matrix[target.choose_unit!.Location.X, target.choose_unit!.Location.Y];
            }
            this.attack_points = new List<Point>(target.attack_points);
            this.move_points = new List<Point>(target.move_points);
            this.first_king = this.unit_matrix[target.first_king!.Location.X, target.first_king!.Location.Y] as King;
            this.last_king = this.unit_matrix[target.last_king!.Location.X, target.last_king!.Location.Y] as King;
        }

        public Unit? Unit(Point point)
        {
            return this.unit_matrix[point.X, point.Y];
        }
        public Unit? Unit(int x, int y)
        {
            return this.unit_matrix[x, y];
        }

        public void ControlUnit(int x, int y, bool in_check = false)
        {
            bool used_turn = false;
            ref Unit? ref_select = ref this.unit_matrix[x, y];

            // 재선택(선택 해제)
            if (this.choose_unit != null && this.choose_unit!.Location == new Point(x, y))
            {
                this.choose_unit = null;

                this.move_points = new List<Point>();
                this.attack_points = new List<Point>();
            }
            // 내 턴일 때만 가능
            else if (this.choose_unit != null && this.manager.Turn == this.choose_unit!.Team)
            {
                // 이미 말 집고 다른 말 선택
                if (ref_select != null && this.choose_unit != null)
                {
                    // castling
                    if (this.choose_unit!.GetType() == typeof(King)
                        && this.move_points.Contains(ref_select.Location))
                    {
                        foreach (Unit? unit in this.unit_matrix)
                        {
                            if (unit != null && this.manager.Turn == IsChecked(this, unit!, this.choose_unit.Location))
                            {
                                return;
                            }
                        }
                        if (ref_select.Location.X < this.choose_unit.Location.X)
                        {
                            this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;
                            this.unit_matrix[FieldData.MINIMUM + 1, this.choose_unit.Location.Y] = this.choose_unit;
                            this.choose_unit!.IncreaseMove();
                            this.choose_unit!.Location = new Point(FieldData.MINIMUM + 1, this.choose_unit.Location.Y);

                            Unit select = ref_select!;

                            this.unit_matrix[ref_select.Location.X, ref_select.Location.Y] = null;
                            this.unit_matrix[FieldData.MINIMUM + 2, select.Location.Y] = select;
                            select.IncreaseMove();
                            select.Location = new Point(FieldData.MINIMUM + 2, this.choose_unit.Location.Y);
                        }
                        else if (ref_select.Location.X > this.choose_unit.Location.X)
                        {
                            this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;
                            this.unit_matrix[FieldData.MAXIMUM - 4, this.choose_unit.Location.Y] = this.choose_unit;
                            this.choose_unit!.IncreaseMove();
                            this.choose_unit!.Location = new Point(FieldData.MAXIMUM - 4, this.choose_unit.Location.Y);

                            Unit select = ref_select!;

                            this.unit_matrix[ref_select.Location.X, ref_select.Location.Y] = null;
                            this.unit_matrix[FieldData.MAXIMUM - 3, select.Location.Y] = select;
                            select.IncreaseMove();
                            select.Location = new Point(FieldData.MAXIMUM - 3, select.Location.Y);
                        }

                        this.move_points = new List<Point>();
                        this.attack_points = new List<Point>();

                        used_turn = true;
                    }
                    // 공격 시도
                    else if (ref_select!.Team != this.choose_unit!.Team)
                    {
                        // 사정거리 내에 적이 있을 경우
                        if (this.attack_points.Contains(new Point(x, y)))
                        {
                            ref_select!.Kill();

                            // en passant!
                            if (this.choose_unit.GetType() == typeof(Pawn) && this.choose_unit.Location.Y == y
                                && ((this.choose_unit.Team == Team.First && this.unit_matrix[x, y - 1] == null)
                                 || (this.choose_unit.Team == Team.Last && this.unit_matrix[x, y + 1] == null)))
                            {
                                ref_select = null;

                                if (this.choose_unit.Team == Team.First)
                                {
                                    this.unit_matrix[x, y - 1] = this.choose_unit!;
                                    this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;

                                    this.choose_unit.Location = new Point(x, y - 1);
                                }
                                else if (this.choose_unit.Team == Team.Last)
                                {
                                    this.unit_matrix[x, y + 1] = this.choose_unit!;
                                    this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;

                                    this.choose_unit.Location = new Point(x, y + 1);
                                }
                            }
                            // 일반 공격
                            else
                            {
                                ref_select = this.choose_unit!;
                                this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;

                                this.choose_unit.Location = new Point(x, y);
                            }
                            if (!in_check && this.choose_unit!.GetType() == typeof(Pawn)
                                && (this.choose_unit!.Location.Y == FieldData.MINIMUM || this.choose_unit!.Location.Y == FieldData.MAXIMUM - 1))
                            {
                                Promotion(this.choose_unit);
                            }

                            this.choose_unit!.IncreaseMove();
                            this.choose_unit = null;

                            this.move_points = new List<Point>();
                            this.attack_points = new List<Point>();

                            used_turn = true;
                        }
                        // 사정거리 내에 적이 없을 경우
                        else
                        {
                            this.choose_unit = ref_select;

                            this.move_points = new List<Point>();
                            this.attack_points = new List<Point>();
                        }
                    }
                    // 선택 변경
                    else if (ref_select!.Team == this.choose_unit!.Team)
                    {
                        this.choose_unit = ref_select;

                        if (ref_select!.Team == this.manager.Turn)
                        {
                            this.attack_points = this.choose_unit!.AbleToAttack(this.unit_matrix);
                            this.move_points = this.choose_unit!.AbleToMove(this.unit_matrix);

                            for (int i = 0; i < this.move_points.Count; i++)
                            {
                                if (!in_check && IsChecked(this, this.choose_unit, this.move_points[i]) == this.manager.Turn)
                                {
                                    this.move_points.RemoveAt(i);
                                    i--;
                                }
                            }
                            for (int i = 0; i < this.attack_points.Count; i++)
                            {
                                if (!in_check && IsChecked(this, this.choose_unit, this.attack_points[i]) == this.manager.Turn)
                                {
                                    this.attack_points.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
                // 이동 시도
                else if (ref_select == null && this.choose_unit != null
                    && this.move_points.Contains(new Point(x, y)))
                {
                    ref_select = this.choose_unit;

                    this.unit_matrix[this.choose_unit.Location.X, this.choose_unit.Location.Y] = null;
                    this.choose_unit!.Location = new Point(x, y);

                    if (!in_check && this.choose_unit!.GetType() == typeof(Pawn)
                        && (this.choose_unit!.Location.Y == FieldData.MINIMUM || this.choose_unit!.Location.Y == FieldData.MAXIMUM - 1))
                    {
                        Promotion(this.choose_unit);
                    }

                    this.choose_unit!.IncreaseMove();
                    this.choose_unit = null;

                    this.move_points = new List<Point>();
                    this.attack_points = new List<Point>();

                    used_turn = true;
                }
            }
            // 연관 없는 선택 혹은 최초 선택
            else if (ref_select != null)
            {
                this.choose_unit = ref_select;

                if (ref_select!.Team == this.manager.Turn)
                {
                    this.attack_points = this.choose_unit!.AbleToAttack(this.unit_matrix);
                    this.move_points = this.choose_unit!.AbleToMove(this.unit_matrix);

                    for (int i = 0; i < this.move_points.Count; i++)
                    {
                        if (!in_check && IsChecked(this, this.choose_unit, this.move_points[i]) == this.manager.Turn)
                        {
                            this.move_points.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int i = 0; i < this.attack_points.Count; i++)
                    {
                        if (!in_check && IsChecked(this, this.choose_unit, this.attack_points[i]) == this.manager.Turn)
                        {
                            this.attack_points.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            // 맨 땅(그 외)
            else
            {
                this.choose_unit = null;

                this.move_points = new List<Point>();
                this.attack_points = new List<Point>();
            }

            if (used_turn)
            {
                this.choose_unit = null;
                this.move_points = new List<Point>();
                this.attack_points = new List<Point>();

                if (!in_check)
                {
                    this.manager.ChangeTurn();
                }
            }
        }

        private void Promotion(Unit choose_unit)
        {
            Type what = Chess.Promotion.DialogBox("Promotion", "What do you want?");
            
            switch (what)
            {
                case Type.Queen:
                    this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y]
                        = new Queen(new Point(choose_unit.Location.X, choose_unit.Location.Y), choose_unit.Team);
                    this.choose_unit = this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y];
                    this.choose_unit!.IncreaseMove();
                    break;
                case Type.Knight:
                    this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y]
                        = new Knight(new Point(choose_unit.Location.X, choose_unit.Location.Y), choose_unit.Team);
                    this.choose_unit = this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y];
                    this.choose_unit!.IncreaseMove();
                    break;
                case Type.Rook:
                    this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y]
                        = new Rook(new Point(choose_unit.Location.X, choose_unit.Location.Y), choose_unit.Team);
                    this.choose_unit = this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y];
                    this.choose_unit!.IncreaseMove();
                    break;
                case Type.Bishop:
                    this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y]
                        = new Bishop(new Point(choose_unit.Location.X, choose_unit.Location.Y), choose_unit.Team);
                    this.choose_unit = this.unit_matrix[choose_unit.Location.X, choose_unit.Location.Y];
                    this.choose_unit!.IncreaseMove();
                    break;
            }

        }

        private void MakeUnit(Point location, Type type, Team team)
        {
            switch (type)
            {
                case Type.Pawn:
                    this.unit_matrix[location.X, location.Y] = new Pawn(location, team); break;
                case Type.Rook:
                    this.unit_matrix[location.X, location.Y] = new Rook(location, team); break;
                case Type.Knight:
                    this.unit_matrix[location.X, location.Y] = new Knight(location, team); break;
                case Type.Bishop:
                    this.unit_matrix[location.X, location.Y] = new Bishop(location, team); break;
                case Type.Queen:
                    this.unit_matrix[location.X, location.Y] = new Queen(location, team); break;
                case Type.King:
                    this.unit_matrix[location.X, location.Y] = new King(location, team);
                    if (team == Team.First) { this.first_king = this.unit_matrix[location.X, location.Y] as King; }
                    else if (team == Team.Last) { this.last_king = this.unit_matrix[location.X, location.Y] as King; }
                    break;
            }
        }
        private void DefaultCase()
        {
            for (int x = 0; x < 8; x++)
            {
                MakeUnit(new Point(x, 1), Type.Pawn, Team.Last);
                MakeUnit(new Point(x, 6), Type.Pawn, Team.First);
            }

            this.unit_matrix[0, 0] = new Rook(new Point(0, 0), Team.Last);
            this.unit_matrix[7, 0] = new Rook(new Point(7, 0), Team.Last);
            this.unit_matrix[0, 7] = new Rook(new Point(0, 7), Team.First);
            this.unit_matrix[7, 7] = new Rook(new Point(7, 7), Team.First);

            this.unit_matrix[1, 0] = new Knight(new Point(1, 0), Team.Last);
            this.unit_matrix[6, 0] = new Knight(new Point(6, 0), Team.Last);
            this.unit_matrix[1, 7] = new Knight(new Point(1, 7), Team.First);
            this.unit_matrix[6, 7] = new Knight(new Point(6, 7), Team.First);

            this.unit_matrix[2, 0] = new Bishop(new Point(2, 0), Team.Last);
            this.unit_matrix[5, 0] = new Bishop(new Point(5, 0), Team.Last);
            this.unit_matrix[2, 7] = new Bishop(new Point(2, 7), Team.First);
            this.unit_matrix[5, 7] = new Bishop(new Point(5, 7), Team.First);

            this.unit_matrix[4, 0] = new Queen(new Point(4, 0), Team.Last);
            this.unit_matrix[4, 7] = new Queen(new Point(4, 7), Team.First);

            this.unit_matrix[3, 0] = this.last_king = new King(new Point(3, 0), Team.Last);
            this.unit_matrix[3, 7] = this.first_king = new King(new Point(3, 7), Team.First);
        }
        private void TestCase()
        {
            MakeUnit(new Point(0, 1), Type.King, Team.Last);
            MakeUnit(new Point(7, 7), Type.King, Team.First);
            MakeUnit(new Point(1, 7), Type.Rook, Team.First);
            MakeUnit(new Point(2, 7), Type.Rook, Team.First);
            MakeUnit(new Point(7, 1), Type.Pawn, Team.First);
        }

        private Team IsChecked(FieldData target, Unit unit, Point point)
        {
            FieldData futures = new FieldData(target);
            futures.choose_unit = futures.unit_matrix[unit.Location.X, unit.Location.Y];
            futures.move_points = futures.choose_unit!.AbleToMove(futures.unit_matrix);
            futures.attack_points = futures.choose_unit!.AbleToAttack(futures.unit_matrix);

            futures.ControlUnit(point.X, point.Y, true);

            for (int x = FieldData.MINIMUM; x < FieldData.MAXIMUM; x++)
            {
                for (int y = FieldData.MINIMUM; y < FieldData.MAXIMUM; y++)
                {
                    if (futures.unit_matrix[x, y] != null && futures.unit_matrix[x, y]!.Team != this.manager.Turn)
                    {
                        List<Point> points = futures.unit_matrix[x, y]!.AbleToAttack(futures.unit_matrix);

                        if (points.Contains(futures.last_king!.Location))
                        {
                            return Team.Last;
                        }
                        else if (points.Contains(futures.first_king!.Location))
                        {
                            return Team.First;
                        }
                    }
                }
            }

            return Team.NA;
        }

        public GameState IsMated()
        {
            Unit? unit = null;
            King? king = null;
            for (int x = FieldData.MINIMUM; x < FieldData.MAXIMUM; x++)
            {
                for (int y = FieldData.MINIMUM; y < FieldData.MAXIMUM; y++)
                {
                    if (this.unit_matrix[x, y] != null && this.unit_matrix[x, y]!.GetType() == typeof(King)
                        && this.unit_matrix[x, y]!.Team == this.manager.Turn)
                    {
                        king = (King)this.unit_matrix[x, y]!;
                    }

                    if (this.unit_matrix[x, y] != null && this.unit_matrix[x, y]!.Team == this.manager.Turn)
                    {
                        unit = this.unit_matrix[x, y];

                        List<Point> attack_points = this.unit_matrix[x, y]!.AbleToAttack(this.unit_matrix);
                        List<Point> move_points = this.unit_matrix[x, y]!.AbleToMove(this.unit_matrix);

                        for (int i = 0; i < move_points.Count; i++)
                        {
                            if (IsChecked(this, unit!, move_points[i]) == this.manager.Turn)
                            {
                                move_points.RemoveAt(i);
                                i--;
                            }
                        }
                        for (int i = 0; i < attack_points.Count; i++)
                        {
                            if (IsChecked(this, unit!, attack_points[i]) == this.manager.Turn)
                            {
                                attack_points.RemoveAt(i);
                                i--;
                            }
                        }

                        if (attack_points.Count + move_points.Count != 0)
                        {
                            unit = null;
                            return GameState.NA;
                        }
                    }
                }
            }

            for (int x = FieldData.MINIMUM; x < FieldData.MAXIMUM; x++)
            {
                for (int y = FieldData.MINIMUM; y < FieldData.MAXIMUM; y++)
                {
                    if (this.unit_matrix[x, y] != null && this.unit_matrix[x, y]!.Team != this.manager.Turn)
                    {
                        List<Point> points = this.unit_matrix[x, y]!.AbleToAttack(this.unit_matrix);

                        if (points.Contains(king!.Location))
                        {
                            unit = null;
                            switch (this.manager.Turn)
                            {
                                case Team.First: return GameState.LastWin;
                                case Team.Last: return GameState.FirstWin;
                            }
                        }
                    }
                }
            }

            unit = null;
            return GameState.Draw;
        }
    }
}