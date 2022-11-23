using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    public class Paint
    {
        private Color color;
        public Color Color { get => this.color; set { SetPaint(); this.color = value; } }

        private Bitmap[] units_image;

        public Paint(Color color)
        {
            this.color = color;
            this.units_image = new Bitmap[Enum.GetValues<Type>().Length];

            this.units_image[(int)Type.Pawn] = Properties.Unit.Pawn;
            this.units_image[(int)Type.Rook] = Properties.Unit.Rook;
            this.units_image[(int)Type.Knight] = Properties.Unit.Knight;
            this.units_image[(int)Type.Bishop] = Properties.Unit.Bishop;
            this.units_image[(int)Type.King] = Properties.Unit.King;
            this.units_image[(int)Type.Queen] = Properties.Unit.Queen;

            SetPaint();
        }

        public Bitmap Painting(Type unit) => this.units_image[(int)unit];

        private void SetPaint()
        {
            foreach (Bitmap unit in this.units_image)
            {
                for (int x = 0; x < unit.Width; x++)
                {
                    for (int y = 0; y < unit.Height; y++)
                    {
                        if (unit.GetPixel(x, y).A != 0)
                        {
                            unit.SetPixel(x, y, this.color);
                        }
                    }
                }
            }
        }
    }
}
