using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal interface IControl
    {
        public abstract List<Point> AbleToMove(Unit?[,] unit_matrix);
        public abstract List<Point> AbleToAttack(Unit?[,] unit_matrix);
    }
}
