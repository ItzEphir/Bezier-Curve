using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bieziezie
{
    public class Stick
    {
        public Point A, B;
        public float length;

        public Stick(Point a, Point b)
        {
            this.A = a;
            this.B = b;
            this.length = this.A.coord.Distance(this.B.coord);
        }
    }
}
