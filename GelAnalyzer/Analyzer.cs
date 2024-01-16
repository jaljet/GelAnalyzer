using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.IO;

namespace GelAnalyzer
{
    public static class Analyzer
    {

        static int beadInSubchainCounter = 1;

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


        public static List<double> Calculate_S_Parameter(List<MolData> mGel, int subchainLength, int colorLength)
        {

        //F:\SCIENCE\Diblocks\lammelae\snapshots sub6 lam monolayer\10 50k\200\gel\for S parameter\gel - 1000000 - bonds.txt
               
            List<double[]> centermass = new List<double[]>();//центры масс гелей
            List<double> SCosX = new List<double>(); //параметры Sx всех гелей
            List<double> SCosY = new List<double>(); //параметры Sy всех гелей
            List<double> devSCosX = new List<double>(); //погрешности Sx всех гелей
            List<double> devSCosY = new List<double>(); //погрешности Sy всех гелей

            //переход к одиночной молекуле
            for (int j = 0; j < 1; j++) //10 - число МГ в монослое
            {
                double[] sizes = new double[] { 220, 60, 182 }; //размеры ящика

                var mol = mGel.Skip(j * 50025).Take(50025).ToList(); //50025 - число частиц в МГ

                mol = mol.Where(x => x.AtomType == 1.00).ToList(); //changing this will allow us to calc S-parameter for block A or B

                var centerPoint = StructFormer.GetCenterPoint(sizes, mol); //центр ящика 

                centermass.Add(StructFormer.GetCenterMass(mol)); //центр масс геля в 3 координатах

                var crossLinkers = GetCrossLinkers(mol);
                //List<double[]> boundaries = new List<double[]> { new double[] { 0, sizes[0] }, new double[] { 0, sizes[1] }, new double[] { 0, sizes[2] } };

                //slice
                
                mol = mol.Where(x => (x.ZCoord >= 89) && (x.ZCoord <= 91)).ToList(); //make a slice
                List<double[]> boundaries = new List<double[]> { new double[] {0, sizes[0] }, new double[] {0 , sizes[1] }, new double[] {89, 91 } };
                
                

                //test room
                
                List<double[]> testmol = MolData.ConvertToListDouble(mol);
                 List<double[]> cylinders = new List<double[]>();
                 cylinders.AddRange(testmol.ToList());
                 var cylmol = new List<MolData>();
                 cylmol = MolData.ShiftAll(false, 3, (int)sizes[0], (int)sizes[1], (int)sizes[2]
                                 , 0, 0, 0, cylinders);

                 FileWorker.SaveLammpstrj(false, @"F:\SCIENCE\Diblocks\lammelae\snapshots sub6 lam monolayer\10 50k\60\chains test\mgel" + (j + 1).ToString() + ".lammpstrj",
                                                  1, sizes, 3, cylmol);
                
                //test room closed



                crossLinkers = crossLinkers.Where(x => x.AtomType == 1.00).ToList(); //changing this will allow us to calc S-parameter for block A or B

                //S = 0.5 * < 3 * cos^2 (theta) - 1 > 
                //theta = angle between OX/OY-axis & subchain director

                var ACosXmean = 0.0;
                var ACosYmean = 0.0;

                List<double> gelACosSx = new List<double>(); //все углы для Sx в рамках геля
                List<double> gelACosSy = new List<double>(); //все углы для Sy в рамках геля
                double[] subchaindirector = { 0.0, 0.0 };

                int counter = 0;
                
                foreach (var c in crossLinkers)
                {
                    for (int i = 0; i < c.Bonds.Count; i++)
                    {
                        //var chainEnd = getChainEnd(c.Index, c.Bonds[i], c.Index, mol);

                        beadInSubchainCounter = 1;
                        var chainEnd = getChainEndWithBoundaries(c.Index, c.Bonds[i], c.Index, mol, boundaries);

                        if(beadInSubchainCounter != colorLength || chainEnd.AtomType != c.AtomType)
                        {
                            beadInSubchainCounter = 1;
                            continue;
                        }

                        subchaindirector[0] = chainEnd.XCoord - c.XCoord;
                        subchaindirector[1] = chainEnd.YCoord - c.YCoord;

                        //test room
                        double[] subChainStart = { c.XCoord, c.YCoord, c.ZCoord, c.AtomType };
                        double[] subChainEnd = {chainEnd.XCoord, chainEnd.YCoord, chainEnd.ZCoord, chainEnd.AtomType };
                        List<double[]> testSubChain = new List<double[]>();
                        testSubChain.Add(subChainStart);
                        testSubChain.Add(subChainEnd);
                        var outChain = new List<MolData>();
                        outChain = MolData.ShiftAll(false, 3, (int)sizes[0], (int)sizes[1], (int)sizes[2]
                                        , 0, 0, 0, testSubChain);

                        FileWorker.SaveLammpstrj(false, @"F:\SCIENCE\Diblocks\lammelae\snapshots sub6 lam monolayer\10 50k\60\chains test\chain" + (counter + 1).ToString() + ".lammpstrj",
                                                         1, sizes, 3, outChain);
                        counter++;
                        //test room end

                        double CosXAngle = subchaindirector[0] / 
                            Math.Sqrt(subchaindirector[0]*subchaindirector[0] + subchaindirector[1]*subchaindirector[1]);
                        gelACosSx.Add(Math.Acos(CosXAngle));

                        double CosYAngle = subchaindirector[1] /
                            Math.Sqrt(subchaindirector[0] * subchaindirector[0] + subchaindirector[1] * subchaindirector[1]);
                        gelACosSy.Add(Math.Acos(CosYAngle));

                    }
                }

                double sum = 0.0;


                foreach (var c in gelACosSx)    //усредняем по гелю углы
                {
                    ACosXmean += c;
                }

                //test X
                StreamWriter strX = new StreamWriter(@"F:\SCIENCE\Diblocks\lammelae\snapshots sub6 lam monolayer\10 50k\60\chains test\gelACosSx.txt");
                for (int i = 0; i < gelACosSx.Count; i++)
                {
                    strX.WriteLine(gelACosSx[i]);
                }
                strX.Close();
                //test X ended

                ACosXmean = ACosXmean/gelACosSx.Count;
                SCosX.Add(ACosXmean);

                foreach (var c in gelACosSy)
                {
                    ACosYmean += c;
                }

                //test Y
                StreamWriter strY = new StreamWriter(@"F:\SCIENCE\Diblocks\lammelae\snapshots sub6 lam monolayer\10 50k\60\chains test\gelACosSy.txt");
                for (int i = 0; i < gelACosSy.Count; i++)
                {
                    strY.WriteLine(gelACosSy[i]);
                }
                strY.Close();
                //test Y ended

                ACosYmean = ACosYmean / gelACosSy.Count;
                SCosY.Add(ACosYmean);

                for (int k = 0; k < gelACosSx.Count; k++) //sqX - rad
                {
                    sum += Math.Pow((gelACosSx[k] - ACosXmean), 2);
                }
                sum = Math.Sqrt(sum / (gelACosSx.Count - 1));
                devSCosX.Add(sum); 
                sum = 0;
                

                for (int k = 0; k < gelACosSy.Count; k++) //sqX - rad
                {
                    sum += Math.Pow((gelACosSy[k] - ACosYmean), 2);
                }
                sum = Math.Sqrt(sum / (gelACosSy.Count - 1));
                devSCosY.Add(sum); 
                sum = 0;

                gelACosSx.Clear();
                gelACosSy.Clear();
                ACosXmean = 0.0;
                ACosYmean = 0.0;
            //    devSCosX.Clear();
            //    devSCosY.Clear();

            }

            
            var SgeneralX = 0.0;                 //усредняем по ансамблю сначала углы, а потом переводим в S
            foreach (var c in SCosX)
            {
                SgeneralX += c;
            }
            SgeneralX = SgeneralX / SCosX.Count;
            SgeneralX = 0.5 * (3 * Math.Pow(Math.Cos(SgeneralX), 2) - 1);

            var SgeneralY = 0.0;                 
            foreach (var c in SCosY)
            {
                SgeneralY += c;
            }
            SgeneralY = SgeneralY / SCosY.Count;
            SgeneralY = 0.5 * (3 * Math.Pow(Math.Cos(SgeneralY), 2) - 1);

            var devSgeneralX = 0.0;
            foreach (var c in devSCosX)
            {
                devSgeneralX += c;
            }
            devSgeneralX = devSgeneralX / devSCosX.Count;
            devSgeneralX = 0.5 * (3 * Math.Pow(Math.Cos(devSgeneralX), 2) - 1);

            var devSgeneralY = 0.0;
            foreach (var c in devSCosY)
            {
                devSgeneralY += c;
            }
            devSgeneralY = devSgeneralY / devSCosY.Count;
            devSgeneralY = 0.5 * (3 * Math.Pow(Math.Cos(devSgeneralY), 2) - 1);

            List<double> SxSy = new List<double>();
            SxSy.Add(Math.Round(SgeneralX, 3));
            SxSy.Add(Math.Round(SgeneralY, 3));
            SxSy.Add(Math.Round(devSgeneralX, 3));
            SxSy.Add(Math.Round(devSgeneralY, 3));


            return SxSy;              


        }

        private static MolData getChainEnd(int crosslinkerId, int currentBead, int previousBead, List<MolData> mGel)
        {
            var bead = mGel[currentBead - 1];
            var bonds = bead.Bonds;

            {
                foreach (var c in bead.Bonds)
                {
                    if (c != crosslinkerId && c != previousBead)
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

        private static MolData getChainEndWithBoundaries(int crosslinkerId, int currentBead, int previousBead, 
            List<MolData> mGel, List<double[]> boundaries)
        {
            
            var beads = mGel.Where(x => x.Index == currentBead).ToList();
            if (beads.Count != 0)
            {
                var bead = beads[0];
                var bonds = bead.Bonds;


            
            {
                foreach (var c in bead.Bonds)
                {
                    if (c != crosslinkerId && c != previousBead)
                    {
                        var newbead = mGel.Where(x => x.Index == c).ToList();
                            if (newbead.Count == 0)
                            {
                                continue;
                            }
                            else try
                                {
                                    if (!newbead[0].AtomType.Equals(mGel[crosslinkerId - 1].AtomType))

                                    {
                                        return bead;
                                    }
                                    else
                                    //if (!mGel[c - 1].AtomType.Equals(mGel[crosslinkerId - 1].AtomType))
                                    //{
                                    //    return bead;
                                    //}
                                    //else
                                    {
                                        if (bead.ZCoord >= boundaries[2][0] && bead.ZCoord <= boundaries[2][1])
                                        {
                                            beadInSubchainCounter++;
                                            return getChainEndWithBoundaries(crosslinkerId, c, currentBead, mGel, boundaries);
                                        }
                                        else
                                        {
                                            return bead;
                                        }
                                        // return getChainEnd(crosslinkerId, c, currentBead, mGel);
                                    }

                                }
                                catch (Exception e)
                                {
                                    continue;
                                }

                                }

                }
                return bead;
            }
            }
            else
            {
                return new MolData(100, 0, 0.0, 0.0, 0.0);
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

        private static double[] FindInsidePoint(List<double[]> points)
        {
            // Находим центр масс множества
            double centerX = 0;
            double centerY = 0;
            double centerZ = 0;
            foreach (double[] point in points)
            {
                centerX += point[0];
                centerY += point[1];
                centerZ += point[2];
            }
            centerX /= points.Count;
            centerY /= points.Count;
            centerZ /= points.Count;
            // Создаем луч, исходящий из центра масс в любом направлении
            double x1 = centerX;
            double y1 = centerY;
            double z1 = centerZ;
            double x2 = centerX + 100000;
            double y2 = centerY + 100000;
            double z2 = centerZ + 100000;
            // Находим первую точку пересечения луча с границей множества
            foreach (double[] point in points)
            {
                double x3 = point[0];
                double y3 = point[1];
                double z3 = point[2];
                double[] nextPoint = GetNextBoundaryPoint(points, point);
                double x4 = nextPoint[0];
                double y4 = nextPoint[1];
                double z4 = nextPoint[2];
                double[] intersection = GetIntersection(x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4);
                if (intersection != null)
                {
                    return intersection;
                }
            }
            return null;
        }

        private static double[] FindNearestBoundaryPoint(List<double[]> points, double[] insidePoint)
        {
            // Находим ближайшую точку на границе множества
            double minDistance = double.MaxValue;
            double[] nearestPoint = null;
            foreach (double[] point in points)
            {
                double distance = GetDistance(point, insidePoint);
                if (distance < minDistance)
                {
                    double[] nextPoint = GetNextBoundaryPoint(points, point);
                    if (IsIntersecting(point, nextPoint, insidePoint[0], insidePoint[1], insidePoint[2]))
                    {
                        minDistance = distance;
                        nearestPoint = point;
                    }
                }
            }
            return nearestPoint;
        }

        private static double[] GetIntersection(double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3, double x4, double y4, double z4)
        {
            // Находим точку пересечения луча, исходящего из точки (x1, y1, z1), соединяющего точки (x1, y1, z1) и (x2, y2, z2),
            // с отрезком, соединяющим точки (x3, y3, z3) и (x4, y4, z4)
            double[] intersection = null;
            double x = double.NaN;
            double y = double.NaN;
            double z = double.NaN;
            double t = double.NaN;
            double a1 = y2 - y1;
            double b1 = x1 - x2;
            double c1 = -z2 + z1;
            double d1 = -y1 * x2 + y2 * x1;
            double a2 = y4 - y3;
            double b2 = x3 - x4;
            double c2 = -z4 + z3;
            double d2 = -y3 * x4 + y4 * x3;
            double det = a1 * b2 - a2 * b1;
            if (det != 0)
            {
                x = (b1 * d2 - b2 * d1) / det;
                y = (a2 * d1 - a1 * d2) / det;
                z = (c1 * b2 - c2 * b1) / det;
                t = Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2) + Math.Pow(z - z1, 2));
                if (t < 100000)
                {
                    intersection = new double[] { x, y, z };
                }
            }
            return intersection;
        }

        private static double GetDistance(double[] p1, double[] p2)
        {
            // Находим расстояние между двумя точками
            double dx = p1[0] - p2[0];
            double dy = p1[1] - p2[1];
            double dz = p1[2] - p2[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        private static bool IsIntersecting(double[] p1, double[] p2, double x, double y, double z)
        {
            // Проверяем, пересекает ли луч, исходящий из точки (x, y, z), отрезок, соединяющий точки p1 и p2
            double x1 = p1[0];
            double y1 = p1[1];
            double z1 = p1[2];
            double x2 = p2[0];
            double y2 = p2[1];
            double z2 = p2[2];
            if (((y1 > y) != (y2 > y)) && (x < (x2 - x1) * (y - y1) / (y2 - y1) + x1) && (z < (z2 - z1) * (y - y1) / (y2 - y1) + z1))
            {
                return true;
            }
            return false;
        }

        private static double[] GetNextBoundaryPoint(List<double[]> points, double[] currentPoint)
        {
            // Находим следующую точку на границе множества
            int currentIndex = points.IndexOf(currentPoint);
            int nextIndex = (currentIndex + 1) % points.Count;
            return points[nextIndex];
        }

        

        public static Boolean isBeadInside(List<double[]> points, double x, double y, double z)
        {
            // Находим точку внутри множества
            double[] insidePoint = FindInsidePoint(points);
            if (insidePoint == null)
            {
                return false;
            }
            // Находим ближайшую точку на границе множества
            double[] boundaryPoint = FindNearestBoundaryPoint(points, insidePoint);
            if (boundaryPoint == null)
            {
                return false;
            }
            // Обходим границу множества и проверяем, находится ли наша точка внутри многоугольника
            bool inside = false;
            double[] currentPoint = boundaryPoint;
            do
            {
                double[] nextPoint = GetNextBoundaryPoint(points, currentPoint);
                if (IsIntersecting(currentPoint, nextPoint, x, y, z))
                {
                    inside = !inside;
                }
                currentPoint = nextPoint;
            } while (!currentPoint.SequenceEqual(boundaryPoint));
            return inside;
        }

        public static bool checkDistance(double[] liquidParticle, SynchronizedCollection<double[]> microgelParticles, double maxDistance, double[] gelCenterPoint)
        {
            bool result = false;
            double distanceToNearestPolymerBead = Analyzer.GetDistance(liquidParticle, gelCenterPoint);

            if(distanceToNearestPolymerBead > maxDistance)
            {
                return false;
            } else
            {
                for (int index = 0; index < microgelParticles.Count; index++)
                {
                    distanceToNearestPolymerBead = Analyzer.GetDistance(microgelParticles[index], liquidParticle);
                    if (distanceToNearestPolymerBead <= 2)
                    {
                        return true;
                    }
                }
            }

            return result;
        }



        public static Boolean checkAtLeastOneIsHigher(double[] liquidParticle, List<double[]> microParticles)
        {
            double[] testParticle = microParticles.Find(x => x[0] > liquidParticle[0] && 
                                                        x[1] > liquidParticle[1] && 
                                                        x[2] > liquidParticle[2]);

            if(testParticle == null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public static Boolean checkAtLeastOneIsLower(double[] liquidParticle, List<double[]> microParticles)
        {
            double[] testParticle = microParticles.Find(x => x[0] < liquidParticle[0] &&
                                                       x[1] < liquidParticle[1] &&
                                                       x[2] < liquidParticle[2]);

            if (testParticle == null)
            {
                return false;
            }
            else
            {
                return true;
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
