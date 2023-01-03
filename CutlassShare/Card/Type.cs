using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutlassShare.Card
{
    public enum Type
    {
        None = 0,
        Escape = 1,

        Red = 2,
        Yellow = 4,
        Blue = 8,
        NormalColor = 14,

        Black = 16,

        Mermaid = 32,
        Pirate = 64,
        SkullKing = 128,
        Unique = 224,

        ScaryMary = 65,
    }
}
