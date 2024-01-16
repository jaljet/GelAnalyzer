using System;
using System.Collections.Generic;

namespace GelAnalyzer
{
    class GelStruct
    {
        public SynchronizedCollection<Point> GelPoints;
        public List<double[]> pointsData;

        public double maxDistanceToCenter;
        public double[] centerMassPoint;

        public GelStruct(List<double[]> pointsData)
        {
            this.pointsData = pointsData;

            SynchronizedCollection<Point> dataToStore = new SynchronizedCollection<Point>();
            foreach (double[] point in pointsData)
            {
                dataToStore.Add(new Point(point[0], point[1], point[2]));
            }
            this.GelPoints = dataToStore;
            this.centerMassPoint = StructFormer.GetCenterMass(pointsData);
            this.maxDistanceToCenter = GetMaxDistanceToCenter(dataToStore);
        }

        public double GetMaxDistanceToCenter(SynchronizedCollection<Point> GelPoints)
        {
            Point center = new Point(this.centerMassPoint[0], this.centerMassPoint[1], this.centerMassPoint[2]);  
            double maxDistance = 0;

            foreach (Point point in GelPoints)
            {
                double distance = Math.Sqrt(Math.Pow(point.x - center.X, 2) + Math.Pow(point.y - center.Y, 2) + Math.Pow(point.z - center.Z, 2));
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }

            return maxDistance;
        }

        public bool IsPointInside(Point point)
        {
            Point center = new Point(this.centerMassPoint[0], this.centerMassPoint[1], this.centerMassPoint[2]);
            double maxDistance = this.maxDistanceToCenter;
            double distance = Math.Sqrt(Math.Pow(point.x - center.x, 2) + Math.Pow(point.y - center.y, 2) + Math.Pow(point.z - center.z, 2));

            if (distance <= maxDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
