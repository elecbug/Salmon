using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutlassC.Card
{
    internal class Object
    {
        public Type Type { get; private set; }
        public int Num { get; private set; }

        private Object() { }

        public static Object Card(Type type, int num = 0)
        {
            Object result = new Object()
            {
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
            if (this.Type == Type.Red || this.Type == Type.Yellow || this.Type == Type.Blue || this.Type == Type.Black)
            {
                return this.Num == 0;
            }
            else
            {
                return this.Num != 0;
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

            }
        }
    }
}
