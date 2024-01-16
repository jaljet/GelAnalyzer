using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelAnalyzer
{
    class Polygon
    {
        private readonly List<Point> _vertices;
        private readonly Node _root;

        public Polygon(List<Point> vertices)
        {
            _vertices = vertices;
            _root = BuildTree(vertices);
        }

        private Node BuildTree(List<Point> vertices)
        {
            if (vertices.Count == 0)
            {
                return null;
            }
            int middleIndex = vertices.Count / 2;
            Point middlePoint = vertices[middleIndex];
            List<Point> leftVertices = new List<Point>();
            List<Point> rightVertices = new List<Point>();
            for (int i = 0; i < vertices.Count; i++)
            {
                if (i == middleIndex)
                {
                    continue;
                }
                Point point = vertices[i];
                if (point.X < middlePoint.X)
                {
                    leftVertices.Add(point);
                }
                else
                {
                    rightVertices.Add(point);
                }
            }
            return new Node(middlePoint, BuildTree(leftVertices), BuildTree(rightVertices));
        }

        private bool IntersectsRay(Point p, Point q, Point vertex)
        {
            if ((vertex.Y > p.Y && vertex.Y <= q.Y) || (vertex.Y > q.Y && vertex.Y <= p.Y))
            {
                double intersectionX = (vertex.Y - p.Y) * (q.X - p.X) / (q.Y - p.Y) + p.X;
                if (intersectionX > vertex.X)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(Point point)
        {
            int intersections = 0;
            Node currentNode = _root;
            while (currentNode != null)
            {
                if (point.Y < currentNode.Point.Y)
                {
                    currentNode = currentNode.Left;
                }
                else
                {
                    if (point.Y == currentNode.Point.Y && point.X <= currentNode.Point.X)
                    {
                        return true;
                    }
                    if (currentNode.Left != null)
                    {
                        for (int i = currentNode.Left.Vertices.Count - 1; i >= 0; i--)
                        {
                            Point vertex = currentNode.Left.Vertices[i];
                            if (IntersectsRay(point, new Point(double.MaxValue, point.Y, point.Z), vertex))
                            {
                                intersections++;
                            }
                        }
                    }
                    if (currentNode.Right == null)
                    {
                        break;
                    }
                    for (int i = 0; i < currentNode.Right.Vertices.Count; i++)
                    {
                        Point vertex = currentNode.Right.Vertices[i];
                        if (IntersectsRay(point, new Point(double.MaxValue, point.Y, point.Z), vertex))
                        {
                            intersections++;
                        }
                    }
                    currentNode = currentNode.Right;
                }
            }
            return (intersections % 2) == 1;
        }
    }
}
