using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GelAnalyzer
{
    /// <summary>
    /// Класс с полями для атомов, входящих в состав геля/мицеллы etc.
    /// </summary>
    public class MolData
    {
        public int Index;
        public int MolIndex;
        public double AtomType;
        public double Charge;
        public double XCoord;
        public double YCoord;
        public double ZCoord;
        public List<int> Bonds;


        public MolData(double atomType, int index,
                          double xCoord, double yCoord,
                          double zCoord)
        {
            AtomType = atomType;
            Index = index;
            XCoord = xCoord;
            YCoord = yCoord;
            ZCoord = zCoord;
        }

        public MolData(double atomType, int index, double xCoord, double yCoord, double zCoord, bool withBonds)
        {
            this.AtomType = atomType;
            this.Index = index;
            this.XCoord = xCoord;
            this.YCoord = yCoord;
            this.ZCoord = zCoord;
            if (!withBonds)
                return;
            this.Bonds = new List<int>();
        }

        public MolData(double atomType, int index, int molIndex, double xCoord, double yCoord, double zCoord)
        {
            this.AtomType = atomType;
            this.MolIndex = molIndex;
            this.Index = index;
            this.XCoord = xCoord;
            this.YCoord = yCoord;
            this.ZCoord = zCoord;
        }

        public MolData(double atomType, int index, int molIndex, double charge, double xCoord, double yCoord, double zCoord)
        {
            this.AtomType = atomType;
            this.MolIndex = molIndex;
            this.Index = index;
            this.Charge = charge;
            this.XCoord = xCoord;
            this.YCoord = yCoord;
            this.ZCoord = zCoord;
        }

        // Проверка на существование атома с индексом в списке
        public static bool Exists(List<int> list, int index)
        {
            foreach (int i in list)
            {
                if (i == index)
                    return true;
            }

            return false;

        }
        // Проверка на существование атома с индексом в списке с произвольным типом 
        public static bool Exists(List<MolData> list, int index)
        {
            foreach (var c in list)
            {
                if (c.Index == index)
                    return true;
            }

            return false;

        }

        // Сдвиг молекул в ящике
        public static List<MolData> ShiftAll(bool onlyPolymer, int density,
                                                int xSize, int ySize, int zSize,
                                                double xShift, double yShift, double zShift,
                                               List<double[]> data)
        {
            var atoms = new List<MolData>();
            // 0 Этап - смещение (если есть) всех частиц по координате

            bool xyNegative = data.Any(x => x[0] < 0.0);
            bool zNegative = data.Any(x => x[2] < 0.0);

            foreach (var c in data)
            {
                c[0] += xShift;
                c[1] += yShift;

                if (!xyNegative)
                {
                    if (c[0] <= 0.0) { c[0] += xSize; }
                    if (c[1] <= 0.0) { c[1] += ySize; }
                    if (c[0] >= xSize) { c[0] -= xSize; }
                    if (c[1] >= ySize) { c[1] -= ySize; }
                }
                else
                {
                    if (c[0] <= -xSize / 2.0) { c[0] += xSize; }
                    if (c[1] <= -ySize / 2.0) { c[1] += ySize; }
                    if (c[0] >= xSize / 2.0) { c[0] -= xSize; }
                    if (c[1] >= ySize / 2.0) { c[1] -= ySize; }
                }

                double border = zSize;

                if (!zNegative)
                {
                    c[2] += zShift;
                    if (c[2] <= 0) { c[2] += zSize; }
                    if (c[2] >= zSize) { c[2] -= zSize; }
                }
                else
                {
                    c[2] += (zShift - zSize / 2.0);
                    if (c[2] <= -zSize / 2.0) { c[2] += zSize; }
                    if (c[2] >= zSize / 2.0) { c[2] -= zSize; }
                }
            }

            double maxnum = xSize * ySize * zSize * density;

            if (data.Count < Math.Abs(maxnum)) { maxnum = data.Count; }

            for (int i = 0; i < maxnum; i++)
            {
                atoms.Add(new MolData(data[i][3], i + 1, data[i][0], data[i][1], data[i][2]));
            }

            if (onlyPolymer)
            {
                atoms = atoms.Where(x => x.AtomType.Equals(1.00) ||
                                         x.AtomType.Equals(1.01) ||
                                         x.AtomType.Equals(1.04)).ToList();
            }

            return atoms;
        }
        // Сдвиг молекул в ящике
        public static void ShiftAll(bool onlyPolymer, int density,
                                                int xSize, int ySize, int zSize,
                                                double xShift, double yShift, double zShift,
                                               List<MolData> data)
        {
            // 0 Этап - смещение (если есть) всех частиц по координате

            bool xyNegative = data.Any(x => x.XCoord < 0.0);
            bool zNegative = data.Any(x => x.ZCoord < 0.0);

            double coef = 0;

            foreach (var c in data)
            {
                c.XCoord += xShift;
                c.YCoord += yShift;

                if (!xyNegative)
                {
                    if (c.XCoord <= 0.0) { c.XCoord += xSize; }
                    if (c.YCoord <= 0.0) { c.YCoord += ySize; }
                    if (c.XCoord >= xSize) { c.XCoord -= xSize; }
                    if (c.YCoord >= ySize) { c.YCoord -= ySize; }
                }
                else
                {
                    if (c.XCoord <= -xSize / 2.0) { c.XCoord += xSize; }
                    if (c.YCoord <= -ySize / 2.0) { c.YCoord += ySize; }
                    if (c.XCoord >= xSize / 2.0) { c.XCoord -= xSize; }
                    if (c.YCoord >= ySize / 2.0) { c.YCoord -= ySize; }
                }

                double border = zSize - 2 * coef;

                if (!zNegative)
                {
                    c.ZCoord += zShift;
                    if (c.ZCoord <= coef) { c.ZCoord += border; }
                    if (c.ZCoord >= zSize - coef) { c.ZCoord -= border; }
                }
                else
                {
                    c.ZCoord += (zShift - zSize / 2.0);
                    if (c.ZCoord <= -zSize / 2.0 + coef) { c.ZCoord += border; }
                    if (c.ZCoord >= zSize / 2.0 - coef) { c.ZCoord -= border; }
                }
            }

        }

        #region ShiftAllDouble old
        public static void ShiftAllDouble(int density, int xSize, int ySize, int zSize,
                                          double xShift, double yShift, double zShift,
                                          List<double[]> data)
        {

            // 0 Этап - смещение (если есть) всех частиц по координате

            bool xyNegative = data.Any(x => x[0] < 0.0);
            bool zNegative = data.Any(x => x[2] < 0.0);

            int maxnum = xSize * ySize * zSize * density;

            for (int i = 0; i < Math.Min(data.Count, maxnum); i++)
            {
                data[i][0] += xShift;
                data[i][1] += yShift;
                data[i][2] += zShift;

                if (!xyNegative)
                {
                    if (data[i][0] <= 0.0) { data[i][0] += xSize; }
                    if (data[i][1] <= 0.0) { data[i][1] += ySize; }
                    if (data[i][0] >= xSize) { data[i][0] -= xSize; }
                    if (data[i][1] >= ySize) { data[i][1] -= ySize; }
                }
                else
                {
                    if (data[i][0] <= -xSize / 2.0) { data[i][0] += xSize; }
                    if (data[i][1] <= -ySize / 2.0) { data[i][1] += ySize; }
                    if (data[i][0] >= xSize / 2.0) { data[i][0] -= xSize; }
                    if (data[i][1] >= ySize / 2.0) { data[i][1] -= ySize; }
                }

                if (!zNegative)
                {
                    if (data[i][2] <= -0.0) { data[i][2] += zSize; }
                    if (data[i][2] >= zSize) { data[i][2] -= zSize; }
                }
                else
                {
                    if (data[i][2] <= -zSize / 2.0) { data[i][2] += zSize; }
                    if (data[i][2] >= zSize / 2.0) { data[i][2] -= zSize; }
                }
            }

        }
        #endregion



        #region ShiftAllDouble new
        public static void ShiftAllDouble(int density, double[] sizes, double[] shifts, double[] centerPoint, List<double[]> data)
        {
            // 0 Этап - смещение (если есть) всех частиц по координате 

            int maxnum = (int)(sizes[0] * sizes[1] * sizes[2] * density);

            for (int i = 0; i < Math.Min(data.Count, maxnum); i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    data[i][j] += shifts[j];

                    if (data[i][j] <= (centerPoint[j] - sizes[j] / 2.0)) { data[i][j] += sizes[j]; }
                    if (data[i][j] >= (centerPoint[j] + sizes[j] / 2.0)) { data[i][j] -= sizes[j]; }
                }
            }

        }


        public static void ShiftAll(int density, double[] sizes, double[] shifts, double[] centerPoint, List<MolData> data)
        {
            // 0 Этап - смещение (если есть) всех частиц по координате 

            int maxnum = (int)(sizes[0] * sizes[1] * sizes[2] * density);

            for (int i = 0; i < Math.Min(data.Count, maxnum); i++)
            {

                data[i].XCoord += shifts[0];
                data[i].YCoord += shifts[1];
                data[i].ZCoord += shifts[2];

                if (data[i].XCoord <= (centerPoint[0] - sizes[0] / 2.0)) { data[i].XCoord += sizes[0]; }
                if (data[i].XCoord >= (centerPoint[0] + sizes[0] / 2.0)) { data[i].XCoord -= sizes[0]; }

                if (data[i].YCoord <= (centerPoint[1] - sizes[1] / 2.0)) { data[i].YCoord += sizes[1]; }
                if (data[i].YCoord >= (centerPoint[1] + sizes[1] / 2.0)) { data[i].YCoord -= sizes[1]; }

                if (data[i].ZCoord <= (centerPoint[2] - sizes[2] / 2.0)) { data[i].ZCoord += sizes[2]; }
                if (data[i].ZCoord >= (centerPoint[2] + sizes[2] / 2.0)) { data[i].ZCoord -= sizes[2]; }
            }
            

        }
        #endregion

        public static List<double[]> ConvertToListDouble(List<MolData> data)
        {
            var doubleData = new List<double[]>();

            foreach (var c in data)
            {
                var line = new double[4];

                line[0] = c.XCoord;
                line[1] = c.YCoord;
                line[2] = c.ZCoord;
                line[3] = c.AtomType;

                doubleData.Add(line);
            }

            return doubleData;
        }

        public static List<MolData> ConvertToMolData(List<double[]> data, bool withBonds, List<int[]> bonds)
        {
            var system = new List<MolData>();

            for (int i = 0; i < data.Count; i++)
            {
                var newAtom = new MolData(data[i][3], i + 1, Convert.ToInt32(data[i][4]), data[i][0], data[i][1], data[i][2]);
                system.Add(newAtom);
            }

            if (withBonds)
            {
                foreach (var c in system)
                {   
                        var beadBonds = new List<int>();
                    foreach (var p in bonds)
                    {
                        if (p[0] == c.Index)
                        {
                            beadBonds.Add(p[1]);
                        }
                        if (p[1] == c.Index)
                        {
                            beadBonds.Add(p[0]);
                        }
                    }

                    c.Bonds = beadBonds;
                    }
            }

            return system;
        }

    }
}