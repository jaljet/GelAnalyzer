using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelAnalyzer
{
    class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
