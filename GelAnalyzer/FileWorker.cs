using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace GelAnalyzer
{
    public class FileWorker
    {

        /// <summary>
        /// перечислимый тип видов записи в строку
        /// </summary>
        private enum Condition : byte
        {
            Coord,
            Velocity,
            Connection,
            Angle
        }

        public static Dictionary<int,double> AtomTypes =
            new Dictionary<int, double>()
        {
          {1, 1.00}, 
          {2, 1.01},
          {3, 1.03},
          {4, 1.02},
          {5, 1.04},
          {6, 1.05},
          {7, 1.06},
          {8, 1.07},
          {9, 1.09}
        };

      
        private static string CreateOneLine_Lammptrj(int number,double x, double y, double z, int type)
        {
            return String.Format(
                "{0,6}{1,3}{2,9}{3,9}{4,9}",
                number,
                type,
                x.ToString("0.0000", CultureInfo.InvariantCulture),
                y.ToString("0.0000", CultureInfo.InvariantCulture),
                z.ToString("0.0000", CultureInfo.InvariantCulture)               
            );
        }

        public static List<double[]> LoadConfLines(string fileName)
         {
            var lines = File.ReadAllLines(fileName);

            var data = new List<double[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                var sList = readLine(lines[i]);
                var row = new double[sList.Count];

                for (int j = 0; j < sList.Count; j++)
                {
                    row[j] = replaceValue(sList.ElementAt(j));
                }

                data.Add(row);
            }

            //MolData.RecolorInitially(data); 

            return data;

        }
        #region related to ReadLammpstrjFile
        /*public static List<double[]> LoadLammpstrjLines(string fileName, int snapNum, out double[] sizes)
        {
            var lines = ReadLammpstrjFile(fileName);

            sizes = new double[3];

            var data = new List<double[]>();

            int molcount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                // Define array's length 
                if (lines[i] == "ITEM: NUMBER OF ATOMS")
                {
                    molcount = Convert.ToInt32(lines[i + 1]);
                }

                // Define box sizes 
                if (lines[i] == "ITEM: BOX BOUNDS pp pp pp" || lines[i] == "ITEM: BOX BOUNDS ff pp pp" ||
                lines[i] == "ITEM: BOX BOUNDS pp ff pp" || lines[i] == "ITEM: BOX BOUNDS pp pp ff")
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var sList = ReadLine(lines[i + j + 1]);

                        if (sList[0].Length == 22)
                        {
                            var maxSize = ReplaceValue(sList[1].Substring(0, 3)) * Math.Pow(10, Convert.ToInt32(sList[1].Substring(20)));
                            var minSize = ReplaceValue(sList[0].Substring(0, 3)) * Math.Pow(10, Convert.ToInt32(sList[0].Substring(20)));
                            sizes[j] = (int)(maxSize - minSize);
                        }
                        else
                        {
                            sizes[j] = Convert.ToInt32(sList[1]) - Convert.ToInt32(sList[0]);
                        }
                    }
                }

                if (lines[i] == "ITEM: ATOMS id type xs ys zs")
                {
                    var atomList = new List<double[]>();

                    for (int j = 0; j < molcount; j++)
                    {
                        var sList = ReadLine(lines[i + j + 1]);

                        var row = new double[sList.Count];

                        for (int k = 0; k < sList.Count; k++)
                        {
                            if (k == 0)
                            {
                                row[k] = Convert.ToInt32(sList[0]);
                            }
                            else if (k == 1)
                            {
                                row[k] = AtomTypes[Convert.ToInt32(sList[1])];
                            }
                            else
                            {
                                row[k] = Math.Round(ReplaceValue(sList[k]) * sizes[k - 2], 3);
                                if (row[k] < 0)
                                {
                                    row[k] = 0.0;
                                }
                            }
                        }

                        atomList.Add(row);
                    }

                    atomList = atomList.OrderBy(x => x[0]).ToList();

                    foreach (var c in atomList)
                    {
                        data.Add(new double[] { c[2], c[3], c[4], c[1], c[0] - 1 });
                    }
                }
            }

            return data;
        }
        */

        public static void LoadConfLines(out double xSize, out double ySize, out double zSize, string fileName, 
            List<double[]> data, List<int[]> bonds, List<int[]> angles)
        {
            xSize = 0;
            ySize = 0;
            zSize = 0;
            var lines = File.ReadAllLines(fileName);
            var sizes = new List<double[]>();
            bonds.Clear();
            angles.Clear();
            int molcount = 0;
            int bondscount = 0;
            int anglescount = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var sList = readLine(lines[i]);
                if (sList.Count == 2)
                {
                    if (sList[1].Equals("atoms"))
                        molcount = Convert.ToInt32(sList[0]);
                    if (sList[1].Equals(nameof(bonds)))
                        bondscount = Convert.ToInt32(sList[0]);
                    if (sList[1].Equals(nameof(angles)))
                        anglescount = Convert.ToInt32(sList[0]);
                }
                if (sList.Count == 4)
                {
                    if (sList[2] == "xlo")
                    {
                        sizes.Add(new double[2] { replaceValue(sList[0]), replaceValue(sList[1]) });
                        xSize = sizes[0][1] - sizes[0][0];
                    }
                    if (sList[2] == "ylo")
                    {
                        sizes.Add(new double[2] { replaceValue(sList[0]), replaceValue(sList[1]) });
                        ySize = sizes[1][1] - sizes[1][0];
                    }
                    if (sList[2] == "zlo")
                    {
                        sizes.Add(new double[2] { replaceValue(sList[0]), replaceValue(sList[1]) });
                        zSize = sizes[2][1] - sizes[2][0];
                    }
                }
                if (sList.Count == 1)
                {
                    int counter = 0;

                    // Read atoms
                    if (sList[0].Equals("Atoms"))
                    {
                        do
                        {
                            int ind = Math.Min(i + 1 + counter, lines.Length - 1);
                            sList = readLine(lines[ind]);
                            counter++;

                            if (sList.Count >= 6)
                            {
                                var row = new double[5];
                                int startLine = 3;

                                if (sList[2].Length == 7)
                                {
                                    startLine = 2;
                                    row[3] = AtomTypes[Convert.ToInt32(sList[1])];

                                    if (sList.Count == 7)
                                    {
                                        row[4] = replaceValue(sList[6]);
                                    }
                                    else
                                    {
                                        row[4] = replaceValue(sList[5]);
                                    }
                                }
                                else
                                {
                                    row[3] = AtomTypes[Convert.ToInt32(sList[2])];
                                    row[4] = replaceValue(sList[1]);
                                    if (sList.Count == 7) { startLine = 4; }
                                }

                                for (int j = 0; j < 3; j++)
                                {
                                    row[j] = replaceValue(sList[startLine + j]) - sizes[j][0];
                                }
                                data.Add(row);
                            }
                        }
                        while (data.Count < molcount);
                    }

                    // Read bonds and angles
                    if (bondscount != 0 || (uint)anglescount > 0U)
                    {
                        if (sList[0].Equals("Bonds"))
                        {
                            counter = 0;
                            do
                            {
                                int ind = Math.Min(i + 1 + counter, lines.Length - 1);
                                sList = readLine(lines[ind]);
                                counter++;
                                if (data.Count >= 100000)
                                {
                                    if (sList.Count == 2)
                                        bonds.Add(new int[3]
                                        {
                                        Convert.ToInt32(sList[1].Substring(1, 6)),
                                        Convert.ToInt32(sList[1].Substring(7, 6)),
                                        Convert.ToInt32(sList[1].Substring(0, 1))
                                        });
                                    if (sList.Count == 3)
                                    {
                                        if (sList[1].Length == 1)
                                            bonds.Add(new int[3]
                                            {
                                             Convert.ToInt32(sList[2].Substring(0, 5)),
                                             Convert.ToInt32(sList[2].Substring(5, 6)),
                                             Convert.ToInt32(sList[1])
                                            });
                                        else
                                            bonds.Add(new int[3]
                                            {
                                             Convert.ToInt32(sList[1].Substring(1, 6)),
                                             Convert.ToInt32(sList[2]),
                                             Convert.ToInt32(sList[1].Substring(0, 1))
                                            });
                                    }
                                    if (sList.Count == 4)
                                        bonds.Add(new int[3]
                                        {
                                         Convert.ToInt32(sList[2]),
                                         Convert.ToInt32(sList[3]),
                                         Convert.ToInt32(sList[1])
                                        });
                                }
                                else if (sList.Count == 4)
                                    bonds.Add(new int[3]
                                    {
                                     Convert.ToInt32(sList[2]),
                                     Convert.ToInt32(sList[3]),
                                     Convert.ToInt32(sList[1])
                                    });
                            }
                            while (bonds.Count < bondscount);
                        }
                        if (sList[0].Equals("Angles"))
                        {
                            counter = 0;
                            do
                            {
                                var ind = Math.Min(i + 1 + counter, lines.Length - 1);
                                sList = readLine(lines[ind]);
                                counter++;
                                if (sList.Count == 5 && angles != null)
                                    angles.Add(new int[4]
                                    {
                                     Convert.ToInt32(sList[2]),
                                     Convert.ToInt32(sList[3]),
                                     Convert.ToInt32(sList[4]),
                                     Convert.ToInt32(sList[1])
                                    });
                                if (ind == lines.Length - 1)
                                {
                                    break;
                                }
                            }
                            while (angles.Count < anglescount);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        private static string[] ReadLammpstrjFile(string fileName)
         {
             using (StreamReader file = new StreamReader(fileName))
             {
                 var readLines = new List<string>();

                 int snapshotcounts = 0;

                 do
                 {
                     var tempLine = file.ReadLine();
                     if (tempLine == "ITEM: TIMESTEP")
                     { snapshotcounts++; }

                     if (tempLine == null) break;

                     readLines.Add(tempLine);

                 } while (snapshotcounts < 2);

                 // проверка на пустые строки
                 if (readLines[readLines.Count - 1] == " ")
                 {
                     readLines.RemoveAt(readLines.Count - 1);
                 }

                 var lines = new string[readLines.Count];

                 for (int i = 0; i < lines.Length; i++)
                 {
                     lines[i] = readLines[i];
                 }

                 return lines;
             }
         }

        public static List<double[]> LoadLammpstrjLines(string fileName, out int snapNum, out double[] sizes)
        {
            var lines = ReadLammpstrjFile(fileName);
            sizes = new double[3];
            var data = new List<double[]>();
            int molcount = 0;

            snapNum = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "ITEM: TIMESTEP")
                {
                    snapNum = Convert.ToInt32(lines[i + 1]);
                }
                if (lines[i] == "ITEM: NUMBER OF ATOMS")
                    molcount = Convert.ToInt32(lines[i + 1]);
                if (lines[i] == "ITEM: BOX BOUNDS pp pp pp" || lines[i] == "ITEM: BOX BOUNDS ff pp pp" || lines[i] == "ITEM: BOX BOUNDS pp ff pp" || lines[i] == "ITEM: BOX BOUNDS pp pp ff")
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var sList = readLine(lines[i + j + 1]);
                        if (sList[0].Length == 22 || sList[0].Length == 23)
                        {
                            double maxSize = replaceValue(sList[1].Substring(0, 4)) * Math.Pow(10.0, (double)Convert.ToInt32(sList[1].Substring(20)));
                            double minSize = replaceValue(sList[0].Substring(0, 4)) * Math.Pow(10.0, (double)Convert.ToInt32(sList[0].Substring(20)));
                            sizes[j] = maxSize - minSize;
                        }
                        else
                            sizes[j] = replaceValue(sList[1]) - replaceValue(sList[0]);
                    }
                }
                if (lines[i] == "ITEM: ATOMS id type xs ys zs")
                {
                    List<double[]> atomList = new List<double[]>();
                    for (int j = 0; j < molcount; j++)
                    {
                        var sList = readLine(lines[i + j + 1]);
                        double[] row = new double[sList.Count];
                        for (int k = 0; k < sList.Count; k++)
                        {
                            switch (k)
                            {
                                case 0:
                                    row[k] = (double)Convert.ToInt32(sList[0]);
                                    break;
                                case 1:
                                    row[k] = AtomTypes[Convert.ToInt32(sList[1])];
                                    break;
                                default:
                                    row[k] = Math.Round(replaceValue(sList[k]) * sizes[k - 2], 3);
                                    if (row[k] < 0.0)
                                        row[k] = 0.0;
                                    break;
                            }
                        }
                        atomList.Add(row);
                    }
                    atomList = atomList.OrderBy(x => x[0]).ToList();
                    foreach (var c in atomList)
                    {
                        try
                        {
                            data.Add(new double[] { c[2], c[3], c[4], c[1], c[0] - 1 });
                        }
                        catch
                        {
                            var error = c[0];

                            throw new Exception("Ð¾ÑˆÐ¸Ð±ÐºÐ° Ð² ÑÐ»ÐµÐ¼ÐµÐ½Ñ‚Ðµ " + error.ToString());
                        }
                    }
                }
            }
            return data;
        }

        private static List<string> readLine(string line)
         {
             string[] strs = line.Split(new char[] { ' ' });

             var sList = new List<string>();

             foreach (var ss in strs)
             {
                 if (ss.Trim() != "")
                     sList.Add(ss);
             }

             return sList;
         }

        public static void SaveLammpstrj(bool append, string FileName, int ItemNum, double[] sizes, double density, 
            List<MolData> strct)
        {
            bool xyPositive = true;
            bool onlyZpositive = true;

            foreach (MolData molData in strct)
            {
                if (molData.ZCoord < 0.0)
                {
                    onlyZpositive = false;
                    break;
                }
            }
            foreach (MolData molData in strct)
            {
                if (molData.XCoord < 0.0)
                {
                    xyPositive = false;
                    break;
                }
            }
            int atomCount = (int)(density * sizes[0] * sizes[1] * sizes[2]);
            if (strct.Count < atomCount || density == 0)
                atomCount = strct.Count;
            string directoryName = Path.GetDirectoryName(FileName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var filemode = FileMode.Append;
            if (!append)
            {
                filemode = FileMode.Create;
            }

            using (FileStream fileStream = new FileStream(FileName, filemode, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine("ITEM: TIMESTEP");
                    streamWriter.WriteLine(ItemNum);
                    streamWriter.WriteLine("ITEM: NUMBER OF ATOMS");
                    streamWriter.WriteLine(atomCount);
                    streamWriter.WriteLine("ITEM: BOX BOUNDS pp pp pp");
                    streamWriter.WriteLine(string.Format("{0,1}{1,6}", 0, Math.Round(sizes[0], 2)));
                    streamWriter.WriteLine(string.Format("{0,1}{1,6}", 0, Math.Round(sizes[1], 2)));
                    streamWriter.WriteLine(string.Format("{0,1}{1,6}", 0, Math.Round(sizes[2], 2)));
                    streamWriter.WriteLine("ITEM: ATOMS id type xs ys zs");

                    for (int i = 0; i < atomCount; i++)
                    {
                        double xCoord = Math.Round((strct[i].XCoord + sizes[0] / 2.0) / sizes[0], 4);
                        double yCoord = Math.Round((strct[i].YCoord + sizes[1] / 2.0) / sizes[1], 4);
                        double zCoord = Math.Round((strct[i].ZCoord + sizes[2] / 2.0) / sizes[2], 4);
                        if (onlyZpositive)
                            zCoord = Math.Round(strct[i].ZCoord / sizes[2], 4);
                        if (xyPositive)
                        {
                            xCoord = Math.Round(strct[i].XCoord / sizes[0], 4);
                            yCoord = Math.Round(strct[i].YCoord / sizes[1], 4);
                        }
                        int type = AtomTypes.FirstOrDefault((x => x.Value == strct[i].AtomType)).Key;
                        if (strct[i].AtomType > 2.0)
                        {
                            type = (int)strct[i].AtomType;
                        }
                        string oneLineLammptrj = CreateOneLine_Lammptrj(i + 1, xCoord, yCoord, zCoord, type);
                        streamWriter.WriteLine(oneLineLammptrj);
                    }
                    streamWriter.Flush();
                }
            }
        }


        #endregion
        private static List<string> MolLine(string line)
         {
             string[] strs = line.Split(new char[] { ' ' });

             var sList = new List<string>();

             foreach (var ss in strs)
             {
                 if (ss.Trim() != "")
                     sList.Add(ss);
             }

             return sList;
         }

          private static double replaceValue(string str)
        {
            string symbol = "";
            double retValue = 0;
            symbol = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            if (symbol == ".")
            {
                str = str.Replace(",", ".");
            }
            else
            {
                str = str.Replace(".", ",");
            }

            if (str == "")
            {
                throw new ApplicationException("Имеются незаполненные поля! Убедитесь,что заданы все параметры расчета!");
            }

            retValue = Convert.ToDouble(str);
            return retValue;
        }

    }
}
