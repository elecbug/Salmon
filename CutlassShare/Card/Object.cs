using System.Diagnostics;

namespace CutlassShare.Card
{
    public class Object
    {
        public int Id { get; set; }
        public Type Type { get; private set; }
        public int Num { get; private set; }

        private Object() { }

        public static Object Card(int id, Type type, int num = 0)
        {
            Object result = new Object()
            {
                Id = id,
                Type = type,
                Num = num,
            };
            if (result.ExceptionTest())
            {
                throw new Exception("Card.Object's instance is nonstable.");
            }

            return result;
        }

        private bool ExceptionTest()
        {
            if (Type == Type.Red || Type == Type.Yellow || Type == Type.Blue || Type == Type.Black)
            {
                return Num == 0;
            }
            else
            {
                return Num != 0;
            }
        }

        public static Object Compare(Object? before_win, Object now)
        {
            if (before_win == null)
            {
                return now;
            }
            else
            {
                // if escape with not first turn
                if (now.Type == Type.Escape)
                {
                    return before_win;
                }
                // if color on the color(normal)
                else if ((before_win.Type & Type.NormalColor) != Type.None && (now.Type & Type.NormalColor) != Type.None)
                {
                    if (before_win.Type == now.Type)
                    {
                        return before_win.Num > now.Num ? before_win : now;
                    }
                    else
                    {
                        return before_win;
                    }
                }
                // if black on the color(normal)
                else if ((before_win.Type & Type.NormalColor) != Type.None && now.Type == Type.Black)
                {
                    return now;
                }
                // if black on the black
                else if (before_win.Type == Type.Black && now.Type == Type.Black)
                {
                    return before_win.Num > now.Num ? before_win : now;
                }
                // if unique on the color(all)
                else if ((before_win.Type & (Type.NormalColor | Type.Black)) != Type.None && (now.Type & Type.Unique) != Type.None)
                {
                    return now;
                }
                // if unique one the unique
                else if ((before_win.Type & Type.Unique) != Type.None && (now.Type & Type.Unique) != Type.None)
                {
                    switch (before_win.Type)
                    {
                        case Type.Mermaid:
                            switch (now.Type)
                            {
                                case Type.Mermaid:
                                    return before_win;
                                case Type.Pirate:
                                    return now;
                                case Type.SkullKing:
                                    return before_win;
                            }
                            break;
                        case Type.Pirate:
                            switch (now.Type)
                            {
                                case Type.Mermaid:
                                    return before_win;
                                case Type.Pirate:
                                    return before_win;
                                case Type.SkullKing:
                                    return now;
                            }
                            break;
                        case Type.SkullKing:
                            switch (now.Type)
                            {
                                case Type.Mermaid:
                                    return now;
                                case Type.Pirate:
                                    return before_win;
                                case Type.SkullKing:
                                    return before_win;
                            }
                            break;
                        default:
                            Debug.WriteLine("???");
                            break;
                    }
                }

                return new Object() { Id = -1, Num = 0, Type = Type.None };
            }
        }
    }
}
