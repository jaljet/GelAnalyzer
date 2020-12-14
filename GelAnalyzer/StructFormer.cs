
namespace GelAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

    /// <summary>
    /// Класс для по формированию мол. структур + методы по 
    /// вычислению размеров 
    /// </summary>
    public class StructFormer
    {

        #region Статичные методы по вычислению размеров и прочей геометрии

        /// <summary>
        /// Проверка того, что количество слоев молекул влезет в ящик
        /// </summary>
        public static bool IsFull(int xSize, int ySize, int zSize, double[] diameters)
        {
            int zAmount = (int)((double)zSize / diameters[2]);

            int calcAmount = GetFlatAmount(xSize, ySize, diameters);

            int maxAmount = calcAmount * zAmount;

            if (calcAmount > maxAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Определение количества молекул в сечении XY
        /// </summary>
        public static int GetFlatAmount(int xSize, int ySize,
                                            double[] diameters)
        {
            int xAmount = (int)(xSize / diameters[0]);
            int yAmount = (int)(ySize / diameters[1]);

            return (int)(xAmount * yAmount);

        }

        public static int FuckWithCode(int xSize, int ySize,
                                    double[] diameters)
        {
            int xAmount = (int)(xSize / diameters[0]);
            int yAmount = (int)(ySize / diameters[1]);

            return (int)(xAmount * yAmount);

        }

        /// <summary>
        /// Вычисление гидродинамического радиуса
        /// </summary>
        public static double GetHydroDiameter(List<double[]> data)
        {

            double xRad = GetAxInertSquareRadius(data, 0);
            double yRad = GetAxInertSquareRadius(data, 1);
            double zRad = GetAxInertSquareRadius(data, 2);

            return Math.Round(Math.Sqrt(xRad + yRad + zRad), 3);
        }
        /// <summary>
        /// Вычисление гидродинамического радиуса в сечении XY
        /// </summary>
        public static double GetHydroRadius2D(List<double[]> data)
        {
            double xRad = GetAxInertSquareRadius(data, 0);
            double yRad = GetAxInertSquareRadius(data, 1);

            return Math.Round(Math.Sqrt(xRad + yRad), 2);
        }

        /// <summary>
        /// Вычисление квадрата радиуса инерции i-ой оси (0-X,1-Y,2-Z)
        /// </summary>
        public static double GetAxInertSquareRadius(List<double[]> data, int ax)
        {
            var polymer = data.Where(x => x[3] == 1.00 ||
                                          x[3] == 1.01 ||
                                          x[3] == 1.04).ToList();

            double[] centerMass = GetCenterMass(data);

            double inertRad = 0;

            foreach (var c in polymer)
            {
                inertRad += Math.Pow((c[ax] - centerMass[ax]), 2);
            }

            return inertRad / polymer.Count;
        }

        /// <summary>
        /// Вычисление диаметра молекулы
        /// </summary>
        public static double[] GetDiameter(List<double[]> data)
        {
            double[] diameters = new double[]
            {GetAxDiameter(data, 0),
            GetAxDiameter(data, 1),
            GetAxDiameter(data, 2)
            };

            return diameters;
        }
        /// <summary>
        /// Определение диаметра вдоль оси
        /// </summary>
        public static double GetAxDiameter(List<double[]> data, int axNum)
        {
            var polymer = data.Where(x => x[3] == 1.00 ||
                                          x[3] == 1.01).ToList();
            if (polymer.Count == 0)
            {
                polymer = data.Where(x => x[3] == 1.02 ||
                                          x[3] == 1.04).ToList();
            }

            double diam = polymer.Max(x => x[axNum]) - polymer.Min(x => x[axNum]);

            if (diam == 0) { diam = 0.5; }

            return diam;
        }

        /// <summary>
        /// 
        /// </summary>
        public static double GetMolSliceRadius(List<double[]> data, int axNumOne,
                                               int axNumTwo, double[] centerMass)
        {
            var slice = data.Where(x => x[3] == 1.00 || x[3] == 1.01).ToList();

            double rad = 0.0;

            if (slice.Count != 0)
            {
                rad = slice.Max(x => Math.Sqrt(Math.Pow(x[axNumOne] - centerMass[axNumOne], 2) +
                                               Math.Pow(x[axNumTwo] - centerMass[axNumTwo], 2)));
            }
            return rad;
        }

        #region CenterAxisType_2
        // Centering procedure based on the PBC 
        public static double CenterAxis_Type2(bool fullNorm, int axInd, double axSize, double axPoint, List<double[]> data)
        {
            var mol = data.Where(x => x[3] == 1.00 || x[3] == 1.01 || x[3] == 1.04).ToList();
            var normMol = new List<double[]>();

            // All beads will be rewtiten to the lower corner 
            var minCoord = mol.Min(x => x[axInd]);

            foreach (var d in mol)
            {

                var coef = axSize;

                if (axInd == 2)
                { coef = axSize; }

                if (Math.Abs(minCoord - d[axInd]) < coef)
                {
                    normMol.Add(d);
                }

                else
                {
                    if (axInd == 0)
                        normMol.Add(new double[] { d[0] - axSize, d[1], d[2], d[3] });
                    else if (axInd == 1)
                        normMol.Add(new double[] { d[0], d[1] - axSize, d[2], d[3] });
                    else
                        normMol.Add(new double[] { d[0], d[1], d[2] - axSize, d[3] });
                }
            }

            if (fullNorm)
            {
                double coef = (-axSize / 2.0 + 1.0);

                if (axInd == 2 || !data.Any(x => x[0] < 0.0))
                {
                    coef = 1.0;
                }

                if (minCoord > coef)
                {
                    foreach (var c in normMol)
                    {
                        c[axInd] -= axSize;
                    }
                }
            }

            var cm = StructFormer.GetCenterMass(normMol);

            if (!fullNorm)
            {
                cm[axInd] -= axPoint;
            }
            else
            {
                cm[axInd] -= axSize / 2.0;
            }

            return cm[axInd];
        }
        #endregion
        public static double[] GetCenterMass(List<double[]> data)
        {
            double[] centerMass = new double[]
            {GetAxCenterMass(data, 0),
            GetAxCenterMass(data, 1),
            GetAxCenterMass(data, 2)
            };

            return centerMass;
        }


        public static double[] GetCenterMass(List<MolData> data)
        {
            double[] centerMass = new double[]
            {GetAxCenterMass(data, 0),
            GetAxCenterMass(data, 1),
            GetAxCenterMass(data, 2)
            };

            return centerMass;
        }

        public static double GetAxCenterMass(List<double[]> data, int axNum)
        {
            double ax = 0;

            var polymer = data.Where(x => x[3] == 1.00 ||
                                          x[3] == 1.01 ||
                                          x[3] == 1.04).ToList();

            foreach (var c in polymer)
            {
                ax += c[axNum];
            }

            return ax / polymer.Count;
        }
        public static double GetAxCenterMass(List<MolData> data, int axNum)
        {
            double ax = 0;

            var polymer = data.Where(x => x.AtomType == 1.00 ||
                                          x.AtomType == 1.01 ||
                                          x.AtomType == 1.04).ToList();

            foreach (var c in polymer)
            {
                if (axNum == 0)
                {
                    ax += c.XCoord;
                }
                else if (axNum == 1)
                {
                    ax += c.YCoord;
                }
                else
                {
                    ax += c.ZCoord;
                }
            }

            return ax / polymer.Count;
        }

        public static double[] GetCenterPoint(double[] sizes, List<double[]> data)
        {
            double[] centerPoint = new double[] { sizes[0] / 2.0, sizes[1] / 2.0, sizes[2] / 2.0 };

            for (int i = 0; i <= 2; i++)
            {
                if (data.Any(x => x[i] < -1.0))
                {
                    centerPoint[i] = 0.0;
                }

            }
            return centerPoint;
        }

        public static double[] CenterStructure(double[] centerPoint, List<double[]> data)
        {
            var cMass = GetCenterMass(data);

            for (int i = 0; i <= 2; i++)
            {
                cMass[i] = Math.Round(centerPoint[i] - cMass[i], 2);
            }
            return cMass;
        }

        public static double[] CenterStructureInit(double[] sizes, double[] centerPoint, List<double[]> data)
        {
            var cMass = GetCenterMass(data);

            for (int i = 0; i < 2; i++)
            {
                cMass[i] = Math.Round(sizes[i] - centerPoint[i] - cMass[i], 2);
            }

            cMass[2] = 0.0;
            return cMass;
        }

        public static double[] CenterStructure(double xSize, double ySize, double zSize, List<double[]> data)
        {
            var cMass = CenterStructure(xSize, ySize, data);
            cMass[2] = zSize / 2.0 - Math.Round(GetAxCenterMass(data, 2));
            return cMass;
        }


        public static double[] CenterStructure(double xSize, double ySize, List<double[]> data)
        {
            var cMass = GetCenterMass(data);

            cMass[0] = Math.Round(-cMass[0], 2);
            cMass[1] = Math.Round(-cMass[1], 2);
            cMass[2] = 0.0;
            return cMass;
        }


        public static double GetDistance(double xOne, double yOne, double zOne,
                                          double xTwo, double yTwo, double zTwo)
        {
            return Math.Round(Math.Sqrt(Math.Pow((xOne - xTwo), 2) +
                             Math.Pow((yOne - yTwo), 2) +
                             Math.Pow((zOne - zTwo), 2)), 3);
        }


        public static double GetHeight(List<double[]> data)
        {
            var pols = data.Where(x => x[3] == 1.00 || x[3] == 1.01 || x[3] == 1.04).ToList();
            return pols.Max(x => x[2]);
        }

        #endregion

        
    }
}