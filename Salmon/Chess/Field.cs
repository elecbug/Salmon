using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Salmon.Chess
{
    internal class Field : Panel
    {
        private PictureBox[,] unit_image;
        private Unit?[,] unit_matrix;
        private Unit? choose_unit;
        private List<Point> attack_points;
        private List<Point> move_points;
        private Paint color1, color2;

        public Color Color1 { get => this.color1.Color; set => this.color1.Color = value; }
        public Color Color2 { get => this.color2.Color; set => this.color2.Color = value; }

        public Field(Size size) : base()
        {
            this.Size = size;

            this.color1 = new Paint(Color.Black);
            this.color2 = new Paint(Color.Ivory);

            this.attack_points = new List<Point>();
            this.move_points = new List<Point>();

            this.unit_image = new PictureBox[Board.MAXIMUM, Board.MAXIMUM];
            this.unit_matrix = new Unit[Board.MAXIMUM, Board.MAXIMUM];
            this.choose_unit = null;

            for (int x = 0; x < Board.MAXIMUM; x++)
            {
                for (int y = 0; y < Board.MAXIMUM; y++)
                {
                    this.unit_image[x, y] = new PictureBox()
                    {
                        Parent = this,
                        Visible = true,
                        Size = new Size(this.Width / Board.MAXIMUM, this.Height / Board.MAXIMUM),
                        Location = new Point(x * this.Width / Board.MAXIMUM, y * this.Height / Board.MAXIMUM),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };

                    this.unit_image[x, y].Click += ClickCell;
                }
            }

            this.Resize += ResizeField;

            TestCase();
            ResizeField(this, new EventArgs());
            Repainting();
        }

        private void ResizeField(object? sender, EventArgs e)
        {
            for (int x = 0; x < Board.MAXIMUM; x++)
            {
                for (int y = 0; y < Board.MAXIMUM; y++)
                {
                    this.unit_image[x, y].Size
                        = new Size(this.Width / Board.MAXIMUM, this.Height / Board.MAXIMUM);
                    this.unit_image[x, y].Location
                        = new Point(x * this.Width / Board.MAXIMUM, y * this.Height / Board.MAXIMUM);
                }
            }
        }

        public void Repainting()
        {
            for (int x = 0; x < Board.MAXIMUM; x++)
            {
                for (int y = 0; y < Board.MAXIMUM; y++)
                {
                    if (this.unit_matrix[x, y] != null)
                    {
                        this.unit_image[x, y].BackgroundImage = (this.unit_matrix[x, y]!.Team == Team.First
                            ? this.color1.Painting(this.unit_matrix[x, y]!.Type)
                            : this.color2.Painting(this.unit_matrix[x, y]!.Type));
                    }
                    else
                    {
                        this.unit_image[x, y].BackgroundImage = null;
                    }
                    if (this.attack_points.Contains(new Point(x, y)))
                    {
                        this.unit_image[x, y].BackColor = (x + y) % 2 == 0 ? Color.Pink : Color.DarkRed; 
                    }
                    else if (this.move_points.Contains(new Point(x, y)))
                    {
                        this.unit_image[x, y].BackColor = (x + y) % 2 == 0 ? Color.LightGreen: Color.DarkGreen;    
                    }
                    else
                    {
                        this.unit_image[x, y].BackColor = (x + y) % 2 == 0 ? Color.LightBlue : Color.DarkBlue;
                    } 
                }
            }
        }

        public Unit? Unit(Point point)
        {
            return this.unit_matrix[point.X, point.Y];
        }
        public Unit? Unit(int x, int y)
        {
            return this.unit_matrix[x, y];
        }

        public void ClickCell(object? sender, EventArgs e)
        {
            int x = 0, y = 0;

            for (x = 0; x < Board.MAXIMUM; x++)
            {
                for (y = 0; y < Board.MAXIMUM; y++)
                {
                    if (sender == this.unit_image[x, y])
                    {
                        goto outside;
                    }
                }
            }

        outside:
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

            Repainting();
        }

        private void TestCase()
        {
            for (int x = 0; x < 8; x++)
            {
                this.unit_matrix[x, 1] = new Pawn(this, new Point(x, 1), Team.Last);
                this.unit_matrix[x, 6] = new Pawn(this, new Point(x, 6), Team.First);
            }
            this.unit_matrix[0, 0] = new Rook(this, new Point(0, 0), Team.Last);
            this.unit_matrix[7, 0] = new Rook(this, new Point(7, 0), Team.Last);
            this.unit_matrix[0, 7] = new Rook(this, new Point(0, 7), Team.First);
            this.unit_matrix[7, 7] = new Rook(this, new Point(7, 7), Team.First);
        }
    }
}

// 캐슬링(체크 탈출용은 안된다는 듯?), 프로모션, 체크, 체크메이트, 스테일메이트
// 남은 유닛들 만들고 체크 & 스테일 검사 시스템 구축