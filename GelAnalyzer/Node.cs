using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelAnalyzer
{
    class Node
    {
        public Point Point { get; }
        public List<Point> Vertices { get; }
        public Node Left { get; }
        public Node Right { get; }

        public Node(Point point, Node left, Node right)
        {
            Point = point;
            Vertices = new List<Point>();
            if (left != null)
            {
                Vertices.AddRange(left.Vertices);
            }
            if (right != null)
            {
                Vertices.AddRange(right.Vertices);
            }
            Vertices.Add(point);
            Left = left;
            Right = right;
        }
    }
}
