using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salmon.Chess
{
    internal interface IControl
    {
        public abstract List<Point> AbleToMove();
        public abstract List<Point> AbleToAttack();
    }
}
