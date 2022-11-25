using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Salmon.Chess
{
    internal class FieldUI : Panel
    {
        private PictureBox[,] unit_image;
        private Paint color1, color2;
        private FieldData field_data;

        public Color Color1 { get => this.color1.Color; set => this.color1.Color = value; }
        public Color Color2 { get => this.color2.Color; set => this.color2.Color = value; }

        public FieldUI(Size size) : base()
        {
            this.Size = size;
            this.field_data = new FieldData(this);

            this.color1 = new Paint(Color.Black);
            this.color2 = new Paint(Color.Ivory);

            this.unit_image = new PictureBox[Board.MAXIMUM, Board.MAXIMUM];

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
                        BorderStyle = BorderStyle.FixedSingle,
                        BackgroundImageLayout = ImageLayout.Zoom,
                    };

                    this.unit_image[x, y].Click += ClickCell;
                }
            }

            this.Resize += ResizeField;

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
                    if (this.field_data.GetUnit(x, y) != null)
                    {
                        this.unit_image[x, y].BackgroundImage = (this.field_data.GetUnit(x, y)!.Team == Team.First
                            ? this.color1.Painting(this.field_data.GetUnit(x, y)!.Type)
                            : this.color2.Painting(this.field_data.GetUnit(x, y)!.Type));
                    }
                    else
                    {
                        this.unit_image[x, y].BackgroundImage = null;
                    }
                    if (this.field_data.AttackPoints.Contains(new Point(x, y)))
                    {
                        this.unit_image[x, y].BackColor = Color.Pink;
                    }
                    else if (this.field_data.MovePoints.Contains(new Point(x, y)))
                    {
                        this.unit_image[x, y].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.unit_image[x, y].BackColor = (x + y) % 2 == 0 ? Color.LightGray : Color.DarkGray;
                    }
                }
            }
            if (this.field_data.Choosed != null)
            {
                this.unit_image[this.field_data.Choosed.Location.X, this.field_data.Choosed.Location.Y]!.BackColor
                    = Color.LightBlue;
            }
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
            this.field_data.ClickUnit(x, y);

            Repainting();
        }
    }
}

// 캐슬링(체크 탈출용은 안된다는 듯?), 프로모션, 체크, 체크메이트, 스테일메이트
// 남은 유닛들 만들고 체크 & 스테일 검사 시스템 구축