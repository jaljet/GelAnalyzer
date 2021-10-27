﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace GelAnalyzer
{
    public static class Analyzer
    {
        #region getting MolSizes
        // Находит координаты всех центров масс 
        public static double[] GetMolsSizes(int molNum, int molCount, List<double[]> file, double[] bSizes)
        {

            double xSize = 0.0, ySize = 0.0, zSize = 0.0;

            double xyRad = 0.0, zRad = 0.0;

            xSize = bSizes[0];
            ySize = bSizes[1];
            zSize = bSizes[2];

            var points = new List<double[]>();

            double molDiam = 0;
            int counter = 0;

            var bordermols = new List<int[]>();

            // get search radius
            for (int i = 0; i < molCount; i++)
            {
                var mol = file.Skip(i * molNum).Take(molNum).ToList();
                var diam = StructFormer.GetDiameter(mol);

                if (diam.Max() < 0.8 * Math.Max(xSize, ySize))
                {
                    molDiam += diam.Max();
                    xyRad += StructFormer.GetHydroRadius2D(mol);
                    zRad += Math.Sqrt(StructFormer.GetAxInertSquareRadius(mol, 2));

                    counter++;
                    var cm = StructFormer.GetCenterMass(mol);
                    points.Add(new double[] { cm[0], cm[1] });
                }
                else
                {
                    int sign = 0; // периодичность по оси X
                    if (diam[1] > 0.8 * ySize)
                    {
                        if (diam[0] > 0.8 * xSize)
                        {
                            sign = 2; // периодичность по осям XY
                        }
                        else
                        {
                            sign = 1; // периодичность по оси Y
                        }
                    }

                    bordermols.Add(new int[] { i, sign });
                }
            }

            molDiam /= counter;

            // Избавляемся от периодичности
            foreach (var c in bordermols)
            {
                var mol = file.Skip(c[0] * molNum).Take(molNum).ToList();
                var normMol = new List<double[]>();

                if (c[1] < 2)
                {
                    // Все частицы считаются от левого нижнего угла
                    var minCoord = mol.Min(x => x[c[1]]);
                    foreach (var d in mol)
                    {
                        if (Math.Abs(minCoord - d[c[1]]) < molDiam * 1.1)
                        {
                            normMol.Add(d);
                        }
                        else
                        {
                            if (c[1] == 0)
                                normMol.Add(new double[] { d[0] - xSize, d[1], d[2], d[3] });
                            else
                                normMol.Add(new double[] { d[0], d[1] - ySize, d[2], d[3] });
                        }
                    }
                }
                else
                {
                    var minCoord = mol.Min(x => x[0]);

                    var intermol = new List<double[]>();

                    // shift by x
                    foreach (var d in mol)
                    {
                        if (Math.Abs(minCoord - d[0]) < molDiam * 1.1)
                        {
                            intermol.Add(d);
                        }
                        else
                        {
                            intermol.Add(new double[] { d[0] - xSize, d[1], d[2], d[3] });
                        }
                    }

                    foreach (var d in intermol)
                    {
                        if (Math.Abs(minCoord - d[1]) < molDiam * 1.1)
                        {
                            normMol.Add(d);
                        }
                        else
                        {
                            normMol.Add(new double[] { d[0], d[1] - ySize, d[2], d[3] });
                        }
                    }
                }

                xyRad += StructFormer.GetHydroRadius2D(normMol);
                zRad += StructFormer.GetAxInertSquareRadius(normMol, 2);
            }


            xyRad /= molCount;
            zRad /= molCount;

            return new double[] { xyRad, zRad };
        }
        #endregion

        #region SurfCoverage
        public static double GetSurfCoverage(string filename)
        {

            var file = new List<double[]>();
            double xSize = 0, ySize = 0, zSize = 0;

            file = FileWorker.LoadConfLines(filename);

            xSize = Math.Abs(file[file.Count - 1][0] - file[file.Count - 8][0]);
            ySize = Math.Abs(file[file.Count - 1][1] - file[file.Count - 8][1]);
            zSize = Math.Abs(file[file.Count - 1][2]);

            // Finding interface coordinate

            int liqCount = file.Where(x => x[3].Equals(1.03) || x[3].Equals(1.02)).ToList().Count;
            int solvCount = file.Where(x => x[3].Equals(1.02)).ToList().Count;

            double zInterCoord = zSize * ((double)solvCount / (double)liqCount);

            // Lattice calc
            int surCounter = 0;

            for (int i = 1; i <= (int)xSize; i++)
            {
                for (int j = 1; j <= (int)ySize; j++)
                {
                    double coef1 = xSize / 2.0, coef2 = ySize / 2.0;

                    var cell = file.Where(x => Math.Abs(x[2] - zInterCoord) <= 1.0
                                              && x[0] >= (i - 1 - coef1) && x[0] <= (i - coef1)
                                              && x[1] >= (j - 1 - coef2) && x[1] <= (j - coef2)).ToList();

                    int unitPol = cell.Where(x => x[3].Equals(1.00) || x[3].Equals(1.01)
                                             || x[3].Equals(1.04) || x[3].Equals(1.05)).ToList().Count;

                    double cellFrac = (double)unitPol / (double)cell.Count();

                    if (cellFrac >= (double)(1.0 / 3.0))
                    {
                        surCounter++;
                    }
                }
            }

            double surfFrac = Math.Round((double)surCounter / (xSize * ySize), 3);

            return surfFrac;

        }
        #endregion

        #region AutoCenter
        public static void DoAutoCenter(bool withZCenter, int k, double[] sizes, double[] centerPoint,
            List<double[]> file) //withZCenter allows us to do 3D centering
        {
            if (k == 0)
            {
                return;
            }

            for (int i = 0; i < k; i++)
            {
                double[] centerCoord = StructFormer.CenterStructure(centerPoint, file);

                if (Math.Abs(centerCoord[0]) < 0.5 && Math.Abs(centerCoord[1]) < 0.5 && Math.Abs(centerCoord[2]) < 0.5)
                {
                    break;
                }

                int breakmark = 0;

                if (!withZCenter)
                {
                    centerCoord[2] = 0.0;
                }

                MolData.ShiftAllDouble(3, sizes, centerCoord, centerPoint, file);

                double[] diam = StructFormer.GetDiameter(file);

                for (int j = 0; j <= 2; j++)
                {
                    if (Math.Abs(diam[j] - sizes[j]) <= 2)
                    {
                        var shifts = new double[3];
                        shifts[j] = -StructFormer.CenterAxis_Type2(false, j, sizes[j], centerPoint[j], file);

                        if (j == 2 && !withZCenter)
                        {
                            shifts[j] = 0.0;
                        }
                        MolData.ShiftAllDouble(3, sizes, shifts, centerPoint, file);
                    }
                    else
                    {
                        if (centerCoord[j] < 0.5)
                        {
                            breakmark++;
                        }
                    }
                }

                if ((breakmark == 2 && !withZCenter) || (breakmark == 3 && withZCenter))
                {
                    break;
                }
            }
        }

        public static void DoAutoCenter(bool withZCenter, int k, double[] sizes, double[] centerPoint,
            List<MolData> file)
        {
            if (k == 0)
            {
                return;
            }

            for (int i = 0; i < k; i++)
            {
                double[] centerCoord = StructFormer.CenterStructure(centerPoint, file);

                if (Math.Abs(centerCoord[0]) < 0.5 && Math.Abs(centerCoord[1]) < 0.5 && Math.Abs(centerCoord[2]) < 0.5)
                {
                    break;
                }

                int breakmark = 0;

                if (!withZCenter)
                {
                    centerCoord[2] = 0.0;
                }

                MolData.ShiftAll(3, sizes, centerCoord, centerPoint, file);

                double[] diam = StructFormer.GetDiameter(file);

                for (int j = 0; j <= 2; j++)
                {
                    if (Math.Abs(diam[j] - sizes[j]) <= 2)
                    {
                        var shifts = new double[3];
                        shifts[j] = -StructFormer.CenterAxis_Type2(false, j, sizes[j], centerPoint[j], file);

                        if (j == 2 && !withZCenter)
                        {
                            shifts[j] = 0.0;
                        }
                        MolData.ShiftAll(3, sizes, shifts, centerPoint, file);
                    }
                    else
                    {
                        if (centerCoord[j] < 0.5)
                        {
                            breakmark++;
                        }
                    }
                }

                if ((breakmark == 2 && !withZCenter) || (breakmark == 3 && withZCenter))
                {
                    break;
                }
            }
        }



        #endregion


        public static double GetAngle(double x1, double x2, double y1, double y2, double z1, double z2) //angle between 2 beads
        {
            double CosAngle = (x1 * x2 + y1 * y2 + z1 * z2) / (Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1) * Math.Sqrt(x2 * x2 + y2 * y2 + z2 * z2));

            return Math.Acos(CosAngle);
        }
        public static double GetXYPlaneAngle(double x1, double x2, double y1, double y2) //plane (in XY) angle between 2 beads
        {
            double CosAngle = (x1 * x2 + y1 * y2) / (Math.Sqrt(x1 * x1 + y1 * y1) * Math.Sqrt(x2 * x2 + y2 * y2));

            return Math.Acos(CosAngle);
        }


        public static double GetAverageOfElementFromCollection(List<double[]> Collection, int CollectionSize, int NumberOfElementToAverageInCollection)
        {
            double sum=0;
            double meantvalue=0;
            
            for(int i=0; i < CollectionSize; i++)
            {
                sum += Collection[i][NumberOfElementToAverageInCollection];
            }
            meantvalue = Math.Round(sum / CollectionSize, 2);

            return meantvalue;
        }


        public static double GetDistance(double x1, double x2, double y1, double y2, double z1, double z2) //calculate distance between 2 beads
        {
            double distance = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2) + Math.Pow((z2 - z1), 2));

            return distance;
        }

        #region get cross-sections in microgel from box
        public static List<double[]> GetCrossSections(List<double[]> list)
        {
            List<double[]> crossSections = new List<double[]>();
            int a = 0; //counter for beads which are in concrete distance (0 < DISTANCE < 0.5) from bead in 1st foreach loop  
                       //0-value prevents from counting itself; distance value is based on previously calculated distance between 2 beads in 'real' model gel
            foreach (var c in list)
            {

                foreach (var d in list)
                {
                    if ((Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) < 0.31) & (Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) > 0))
                    { //0,3 is equal to distance from centre 
                      //of star to neighboring bead

                        a++;
                    }
                }
                if (a == 4)
                {
                    crossSections.Add(c);
                }
                a = 0;
            }

            return crossSections;
        }
        #endregion


        #region color in type-2 some of cross-sections to get star-diblock copolymer
        public static List<double[]> GetSortBCrossSections(List<double[]> allcrossections)
        {
            List<double[]> SortBCrossSections = new List<double[]>();

            int a = 0; //counter for beads from center to center for center in 1st foreach loop  
            var distance1 = 2.94;
            var distance2 = 2.95;

            int currcounter;
            int counter=0;
            

            var currCrosslinker = allcrossections[0];
            List<double[]> nearcenters = new List<double[]>(); //centers which are nearby to randomly coloured center
            SortBCrossSections.Add(currCrosslinker);
       
           do
            {
                currcounter = counter;

                foreach (var d in allcrossections)
                {

                    if ((Analyzer.GetDistance(currCrosslinker[0], d[0], currCrosslinker[1], d[1], currCrosslinker[2], d[2]) < distance2)
                        & (Analyzer.GetDistance(currCrosslinker[0], d[0], currCrosslinker[1], d[1], currCrosslinker[2], d[2]) > distance1))
                    {

                        if (!SortBCrossSections.Contains(d))
                        {
                            d[3] = 2;

                            SortBCrossSections.Add(d);
                            
                            counter++;
                        }
                    }
                }
                distance1 *= 1.9;
                distance2 *= 2;




            } while (currcounter < counter) ;


             /*foreach (var c in allcrossections)
            {
                
                foreach (var d in allcrossections)
                {
                    if ((Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) < 1.803) & ((c[3]==2) | d[3]==2))
                    {
                        break;
                        
                    }
                    if ((Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) < 1.805) & (Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) > 1.8))
                    {
                        c[3] = 2;
                        d[3] = 2;
                        a++;
                    }
                }
                if (a == 4)
                {
                    SortBCrossSections.Add(c);
                }
                a = 0;
            }*/

            return SortBCrossSections;
        }



        public static void RecolorALL(List<MolData> mGel, int subchainLength, int colorLength)
        {
            var crossLinkers = GetCrossLinkers(mGel);

            var cMass = StructFormer.GetCenterMass(mGel);

            var minDistance = crossLinkers.Min(x => StructFormer.GetDistance(x.XCoord, x.YCoord, x.ZCoord,
                                                                             cMass[0], cMass[1], cMass[2]));
            var basecLinker = crossLinkers.Where(x => StructFormer.GetDistance(x.XCoord, x.YCoord, x.ZCoord,
                                                                             cMass[0], cMass[1], cMass[2]) == minDistance).ToList()[0];
            basecLinker.AtomType = 1.01;
            basecLinker.MolIndex = 2;

            crossLinkerWalk(basecLinker, colorLength, subchainLength, 0, 0, false, mGel);
        }


        public static double Calculate_S_Parameter(List<MolData> mGel, int subchainLength, int colorLength)
        {
            
            //C:\Users\obi-v\Desktop\SCIENCE\Diblocks\models\test for parameter\16ksub6-diblocked.txt


            List<double[]> centermass = new List<double[]>();//центры масс гелей
            List<double> gelSx = new List<double>(); //параметры Sx всех гелей
            List<double> gelSy = new List<double>(); //параметры Sy всех гелей

            //переход к одиночной молекуле
            for (int j = 0; j < 10; j++) //10 - число МГ в монослое
            {
                double[] sizes = new double[] { 220.0, 60.0, 182.0 };


                var mol = mGel.Skip(j * 50025).Take(50025).ToList(); //50025 - число частиц в МГ

                var centerPoint = StructFormer.GetCenterPoint(sizes, mol); //центр ящика 

                centermass.Add(StructFormer.GetCenterMass(mol)); //центр масс геля в 3 координатах
                Analyzer.DoAutoCenter(false, 5, sizes, centerPoint, mol);



                var crossLinkers = GetCrossLinkers(mGel);

                crossLinkers = crossLinkers.Where(x => x.AtomType == 1.00).ToList(); //changing this will allow us to calc S-parameter for block A or B

                //S = 0.5 * < 3 * cos^2 (theta) - 1 > 
                //theta = angle between OX/OY-axis & subchain director


                var SmeanX = 0.0;
                var SmeanY = 0.0;
                var sigmaSx = 0.0;
                var sigmaSy = 0.0;
                List<double> generalSx = new List<double>(); //все Sx в рамках геля
                List<double> generalSy = new List<double>(); //все Sy в рамках геля

                var tempAngleX = 0.0;
                var tempAngleY = 0.0;

                double[] subchaindirector = { 0.0, 0.0 };
                foreach (var c in crossLinkers)
                {
                    for (int i = 0; i < c.Bonds.Count; i++)
                    {
                        var chainEnd = getChainEnd(c.Index, c.Bonds[i], c.Index, mGel);
                        subchaindirector[0] = chainEnd.XCoord - c.XCoord;
                        subchaindirector[1] = chainEnd.YCoord - c.YCoord;
                        //tempAngle = Analyzer.GetAngle(c.XCoord, chainEnd.XCoord, c.YCoord, chainEnd.YCoord, c.ZCoord, chainEnd.ZCoord);
                        tempAngleX = Analyzer.GetXYPlaneAngle(subchaindirector[0], subchaindirector[0], subchaindirector[1], 0);
                        generalSx.Add(0.5 * (3 * Math.Pow(Math.Cos(tempAngleX), 2) - 1));
                        tempAngleY = Analyzer.GetXYPlaneAngle(subchaindirector[0], 0, subchaindirector[1], subchaindirector[1]);
                        generalSy.Add(0.5 * (3 * Math.Pow(Math.Cos(tempAngleY), 2) - 1));

                        tempAngleX = 0.0;
                        tempAngleY = 0.0;
                    }
                }

                foreach (var c in generalSx)    //усредняем по гелю
                {
                    SmeanX += c;
                }
                SmeanX /= generalSx.Count;
                gelSx.Add(SmeanX);
                SmeanX = 0.0;

                foreach (var c in generalSy)
                {
                    SmeanY += c;
                }
                SmeanY /= generalSy.Count;
                gelSy.Add(SmeanY);
                SmeanY = 0.0;
            }

            var SgeneralX = 0.0;                 //усредняем по ансамблю
            foreach (var c in gelSx)
            {
                SgeneralX += c;
            }
            SgeneralX /= gelSx.Count;

            var SgeneralY = 0.0;                 
            foreach (var c in gelSy)
            {
                SgeneralY += c;
            }
            SgeneralY /= gelSy.Count;


            return Math.Round(SgeneralX, 3);               //must fix it later


        }

        private static MolData getChainEnd(int crosslinkerId, int currentBead, int previousBead, List<MolData> mGel)
        {
            var bead = mGel[currentBead - 1];
            var bonds = bead.Bonds;

            {
                foreach (var c in bead.Bonds)
                {
                    if (c != crosslinkerId && c!= previousBead)
                    {
                        if (!mGel[c - 1].AtomType.Equals(mGel[crosslinkerId - 1].AtomType))
                        {
                            return bead;
                        }
                        else
                        {
                            return getChainEnd(crosslinkerId, c, currentBead, mGel);
                        }

                    }
                    
                }
                return bead;
            }
            
        } 


        private static void crossLinkerWalk(MolData currBead, int colorLength, int subchainLength, 
                                          int colorCounter, int sChainCounter, bool chainRecolored,
                                          List<MolData> mGel)
        {
            // проверка является ли точка узлом (sChainCounter был введен для случая узлов на границе геля с 2 связями)
            if (currBead.Bonds.Count > 2 || sChainCounter == 0)
            {     

                foreach (var c in currBead.Bonds)
                {
                    mGel[c - 1].AtomType = currBead.AtomType;
                    mGel[c - 1].MolIndex = 2;

                if (currBead.AtomType == 1.01)
                {
                    colorCounter = 1;
                }

                    walkAlongSubchain(mGel[c - 1], colorLength, subchainLength, colorCounter, 1, chainRecolored, mGel);
                }     
            }
        }

        private static void walkAlongSubchain(MolData currBead, int colorLength, int subchainLength,
                                         int colorCounter, int sChainCounter, bool chainRecolored,
                                         List<MolData> mGel)
        {
            // обходы вдоль субцепей
            foreach (var c in currBead.Bonds)
            {
                // MolIndex = 2 means that this bead has been visited

                var nextBead = mGel[c - 1];
                if (nextBead.MolIndex != 2)
                {
                    nextBead.MolIndex = 2;
                    sChainCounter++;

                    if (currBead.AtomType == 1.01)
                    {
                        if (colorCounter < colorLength && !chainRecolored)
                        {
                            nextBead.AtomType = 1.01;
                            colorCounter++;
                        }
                        else
                        {
                            chainRecolored = true;
                            colorCounter = 0;

                            if (nextBead.Bonds.Count > 2 || sChainCounter > subchainLength)
                            {
                                sChainCounter = 0;
                                chainRecolored = false;
                                nextBead.AtomType = 1.01;
                            }

                        }
                    }
                    else
                    {
                        if (chainRecolored)
                        {
                            if (nextBead.Bonds.Count > 2 || sChainCounter > subchainLength)
                            {
                                sChainCounter = 0;
                                chainRecolored = false;
                            }
                        }
                        else
                        {
                            if (sChainCounter > subchainLength - colorLength)
                            {
                                colorCounter++;
                                nextBead.AtomType = 1.01;
                            }
                        }
                    }

                    if (nextBead.Bonds.Count > 2 || sChainCounter == 0)
                    {
                        crossLinkerWalk(nextBead, colorLength, subchainLength, colorCounter, 0, false, mGel);
                    }
                    else
                    {
                        walkAlongSubchain(nextBead, colorLength, subchainLength, colorCounter, sChainCounter, chainRecolored, mGel);

                    }
                }
                else
                {
                    continue;
                }
                }
        }

        public static List<MolData> GetCrossLinkers(List<MolData> mgel)
        {
            var cLinkers = new List<MolData>();

            foreach(var c in mgel)
            {
                if (c.Bonds.Count > 2)
                {
                    cLinkers.Add(c);
                }
            }

            return cLinkers;
        }
        #endregion

        #region color in type-2 some of neighbors of type-2 cross-sections
        public static List<double[]> GetSortBNeighbours(List<double[]> allbeads, List<double[]> sortBCrossSections)
        {
            List<double[]> SortBNeighbours = new List<double[]>(); //beads around type-2 crosssections which we want to make type-2
            
            foreach (var c in sortBCrossSections)
            {

                foreach (var d in allbeads)
                {

                    if ((Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) < 1.1) & (Analyzer.GetDistance(c[0], d[0], c[1], d[1], c[2], d[2]) > 0))
                    {
                        d[3] = 2;
                        SortBNeighbours.Add(d);
                    }
                    
                }                      
            }

            return SortBNeighbours;
        }
        #endregion
    }
}
