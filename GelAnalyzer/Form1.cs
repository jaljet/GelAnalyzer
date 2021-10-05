using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Accord.Math.Decompositions;


namespace GelAnalyzer
{

    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
                int i = 0;
                //dgvData.Rows.Add();
                dgvData.Rows.Add(i+1, tbA.Text, tbXYradius.Text, tbZradius.Text, tbSurfArea.Text, 
                    tbsqXYradius.Text, tbsqZradius.Text);
                i++;
                /*
                dgvData.Rows[i].Cells[0].Value = (i+1).ToString();
                dgvData.Rows[i].Cells[1].Value = MolCount.ToString();
                dgvData.Rows[i].Cells[2].Value = tbXYradius.ToString();
                dgvData.Rows[i].Cells[3].Value = tbZradius.ToString();
                dgvData.Rows[i].Cells[4].Value = tbSurfArea.ToString();*/
        }
   

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                lblProgBar.Visible = true;
                pBar.Visible = true;

                var numberN = Convert.ToInt32(tbN.Text);
                var files = Directory.GetFiles(tbPath.Text, "*.xyzr").OrderBy(f => f).ToArray();

                bgWorker.RunWorkerAsync(new object[] { numberN, files });
               // bgSurfWorker.RunWorkerAsync(new object[] { files });
            }
            catch
            {
                lblProgBar.Visible = false;
                pBar.Value = 0;
                pBar.Visible = false;

                throw new Exception("Число частиц не является числом!");
            }
        }
        #region OldWork
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (object[])e.Argument;

            var numberN = (int)args[0];
            var files = (string[])args[1];

            var counter = new double[2]; // содержит данные об xy радиусе, z радиусе и проценте покрытия
            int molCount = 0;
            int snapCount = 0;

            for (int i = 0; i < files.Length; i++)
            {
              
                try
                {
                    var file = FileWorker.LoadConfLines(files[i]);

                    var sizes = new double[3];

                    sizes[0] = Math.Abs(file[file.Count - 1][0] - file[file.Count - 8][0]);
                    sizes[1] = Math.Abs(file[file.Count - 1][1] - file[file.Count - 8][1]);
                    sizes[2] = Math.Abs(file[file.Count - 1][2]);

                   

                    int localMol = file.Where(x => x[3] == 1.00 || x[3] == 1.01).ToList().Count/ numberN;

                    if (localMol > molCount)
                    {
                        molCount = localMol;
                    }

                    var coords  = Analyzer.GetMolsSizes(numberN, molCount, file, sizes);

                    counter[0] += coords [0];
                    counter[1] += coords[1];
                    snapCount++;

                    //progress
                    int barStep = (int)(files.Length / 100.0);
                    if (barStep == 0)
                    {
                        barStep++;
                    }

                    if ((i+1) % barStep == 0)
                    {
                        ((BackgroundWorker)sender).ReportProgress((int)(100.0 * ((double)(i+1)) / ((double)files.Length)));
                    }
                }
                catch
                {
                    MessageBox.Show("ошибка в файле № " + i + 1 + "!");
                }
            }

            counter[0] = Math.Round(counter[0]/snapCount, 3);
            counter[1] = Math.Round(counter[1]/snapCount, 3);


            e.Result = new object[] { molCount, counter };
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = (object[])e.Result;
            tbMolNum.Text = ((int)args[0]).ToString();
            tbXYradius.Text = (((double[])args[1])[0]).ToString();
            tbZradius.Text = (((double[])args[1])[1]).ToString();
            pBar.Value = 0;
        }
        #endregion

        #region SurfFinding
        private void bgSurfWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (object[])e.Argument;

            var filenames = (string[])args[0];

            var obtainedData = new double();

            for (int k = 0; k < filenames.Length; k++)
            {
                if (!bgWorker.CancellationPending)
                {

                    var surf = Analyzer.GetSurfCoverage(filenames[k]);

                    obtainedData += surf;

                    int barStep = (int)(filenames.Length / 100.0);
                    if (barStep == 0)
                    {
                        barStep++;
                    }

                    if (k % barStep == 0)
                    {
                        ((BackgroundWorker)sender).ReportProgress((int)(100.0 * ((double)k) / ((double)filenames.Length)));
                    }
                }
                else
                {
                    e.Result = new object[] { };
                }
            }

            obtainedData /= filenames.Length;

            e.Result = new object[] { obtainedData};

        }
        #endregion
        private void bgSurfWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void bgSurfWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = (object[])e.Result;
            tbSurfArea.Text = ((double)args[0]).ToString();

            pBar.Visible = false;
            lblProgBar.Visible = false;
        }

        private void btnClearTable_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
           // try
            //{
                lblProgBar.Visible = true;
                pBar.Visible = true;

                var numberN = Convert.ToInt32(tbN.Text);
                var files = Directory.GetFiles(tbPath.Text, "*.lammpstrj").OrderBy(f => f).ToArray();
                var molAmount = Convert.ToInt32(tbA.Text);
                var eps = Convert.ToDouble(tbStep.Text);
                var centre = Convert.ToDouble(tbDensCentre.Text);
                bool makecentercut = Convert.ToBoolean(checkCenterCut.Checked);
                bool makeinterfacecut = Convert.ToBoolean(checkInterfaceCut.Checked);
                bool getOrientationalOrderParameter = Convert.ToBoolean(checkAxOrientationalParameter.Checked);
                bool makehollowuppercut = Convert.ToBoolean(checkUpperBottomCut.Checked);
                bgWorkerFindGroup.RunWorkerAsync(new object[] { numberN, files, molAmount,eps, centre, makecentercut, getOrientationalOrderParameter, makeinterfacecut, makehollowuppercut });
                
            //}
            /*catch{
                lblProgBar.Visible = false;
                pBar.Value = 0;
                pBar.Visible = false;               

                throw new Exception("Число частиц не является числом!");
            }*/

        }
        private void bgWorkerFindGroup_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (object[])e.Argument;

            var numberN = (int)args[0]; //число частиц в каждой молекуле
            var files = (string[])args[1]; //берём файлы
            var molAmount = (int)args[2]; //число молекул
            var epsilon = (double)args[3]; //половина толщины слоя
            var centre = (double)args[4]; //расположение межфазной границы (для вычисления в одном слое)
            bool docentercut = (bool)args[5]; //вырезаем центральные в монослое гели в отдельный снэпшот или нет
            bool calcOrientationalOrder = (bool)args[6]; //считаем или не считаем ориентационный параметр порядка
            bool dointerfacecut = (bool)args[7]; //считаем плотности в приграничном слое или нет
            bool dohollowuppercut = (bool)args[8]; //делаем вырезку гелей, у которых полость ушла вверх/вниз по мере сжатия

            var masscenters = new double[molAmount];
            var res = new double[15]; // содержит данные об x/y/xy радиусе, z радиусе и их среднеквадратичных отклонениях, плотностях в выделенном слое
                                      //и параметре порядка для типа А и В в рамках одного снэпшота

            List<double[]> GeneralRes = new List<double[]>();

            var density = new double[3]; //результат подсчёта плотности полимера, общей плотности и об. доли полимера в слое системы
            int molCount = 0; 
            int snapCount = 1;

           // double[] XYfull = new double[molAmount]; //массив для всех Rxy
           // double[] Xfull = new double[molAmount]; //массив для всех Rx
           // double[] Yfull = new double[molAmount]; //массив для всех Ry
           // double[] Zfull = new double[molAmount]; //массив для всех Rz


            //для щёток
            List<double> BackBone = new List<double>();//храним данные по всем щёткам - длина бэкбона (?)
            List<double> SideChain = new List<double>(); //задаётся числом боковых цепей - расстояния от бэкбона до конца боковой цепи по одной щётке
            List<double> BrushSideChain = new List<double>();//храним данные по всем щёткам - расстояния от бэкбона до конца боковой цепи по всем

            double[] X = new double[molAmount]; //размеры в рамках одного снэпшота у гелей
            double[] Y = new double[molAmount];
            double[] XY = new double[molAmount];
            double[] Z = new double[molAmount];

            double[] XOrientationalOrderTypeA = new double[molAmount];//массив ориентационных параметров порядков у гелей, из него получим общий параметр
            double[] XOrientationalOrderTypeB = new double[molAmount];
            double[] YOrientationalOrderTypeA = new double[molAmount];
            double[] YOrientationalOrderTypeB = new double[molAmount];


            var gelXAnglesTypeA = new List<double>();//храним в рамках одного геля значения 3*cos^2 -1 у отдельных бидов относительно Х или Y оси
            var gelXAnglesTypeB = new List<double>();
            var gelYAnglesTypeA = new List<double>();
            var gelYAnglesTypeB = new List<double>();


            double[] poldens = new double[files.Length]; //плотность полимера, полная плотность, объёмная плотность полимера
            double[] fulldens = new double[files.Length];
            double[] volpoldens = new double[files.Length];
            var cylmol = new List<MolData>();
            var colormol = new List<MolData>();

            var UpperHollowmol = new List<MolData>(); //полые гели с полостью над межфазной границей 
            var DownHollowmol = new List<MolData>(); //полые гели с полостью над межфазной границей 

            var BrushWithColoredEnds = new List<MolData>(); //перекрашиваемая щётка

            var sizes = new double[3]; //размеры ящика моделирования

            for (int i = 0; i < files.Length; i++) //идём по снэпшотам
            {
                res = new double[15];
                try
                {

                    var file = FileWorker.LoadLammpstrjLines(files[i], out snapCount, out sizes);

                    List<double[]> centermass = new List<double[]>();//центры масс гелей
                    List<double[]> centers = new List<double[]>(); //центры масс гелей не у стенки
                    List<double> cXY = new List<double>(); //Rxy гелей не у стенки
                    List<double> cY = new List<double>(); //Ry гелей не у стенки
                    List<double> cX = new List<double>(); //Ry гелей не у стенки
                    List<double[]> cylinders = new List<double[]>(); //частицы гелей не у стенки

                    List<double[]> hollowUpCentersmass = new List<double[]>(); //центры масс гелей, полость которых ориентирована вверх 
                    List<double[]> hollowDownCentersmass = new List<double[]>(); //центры масс гелей, полость которых ориентирована вниз
                    List<double> hollowUpRxy = new List<double>(); //Rxy гелей с полостью вверх
                    List<double> hollowDownRxy = new List<double>(); //Rxy гелей с полостью вниз
                    List<double[]> Uphollows = new List<double[]>(); //частицы всех гелей с полостью вверх
                    List<double[]> Downhollows = new List<double[]>();//частицы всех гелей с полостью вниз

                    List<double[]> BrushColoredEnds = new List<double[]>(); // частицы перекрашенной щётки


                    //очищаем коллекции, в которые вырезаем частицы, чтобы избежать перезаписи в снэпшотах и избыточного использования памяти 
                    cylinders.Clear();
                    cylmol.Clear();
                    Uphollows.Clear();
                    Downhollows.Clear();
                    UpperHollowmol.Clear();
                    DownHollowmol.Clear();


                    int backbonecounter1 = 0;
                    int backbonecounter2 = 238;
                    int sideEnd1 = 253;

                    //переход к одиночной молекуле
                    for (int j = 0; j < molAmount; j++)
                    {
                        var mol = file.Skip(j * numberN).Take(numberN).ToList(); //биды геля

                        var centerPoint = StructFormer.GetCenterPoint(sizes, mol); //центр ящика 

                        centermass.Add(StructFormer.GetCenterMass(mol)); //центр масс геля в 3 координатах
                        Analyzer.DoAutoCenter(false, 5, sizes, centerPoint, mol);

                        double[] Masscenter = StructFormer.GetCenterMass(mol);
                        double diagX = 0.0;
                        double diagY = 0.0;
                        double diagZ = 0.0;
                        double elemXY = 0.0;
                        double elemXZ = 0.0;
                        double elemYZ = 0.0;

                        foreach (var c in mol)
                        {
                            diagX += Math.Pow(c[0] - Masscenter[0], 2.0);
                            diagY += Math.Pow(c[1] - Masscenter[1], 2.0);
                            diagZ += Math.Pow(c[2] - Masscenter[2], 2.0);
                            elemXY += (c[0] - Masscenter[0]) * (c[1] - Masscenter[1]);
                            elemXZ += (c[0] - Masscenter[0]) * (c[2] - Masscenter[2]);
                            elemYZ += (c[1] - Masscenter[1]) * (c[2] - Masscenter[2]);
                        }

                        var gyrRad = diagX + diagY + diagZ;

                        var gyrTensor = new double[3, 3]{{diagX,elemXY, elemXZ}, {elemXY,diagY, elemYZ}, {elemXZ,elemYZ, diagZ}};


                        var solver = new EigenvalueDecomposition(gyrTensor, true, true);
 
                        var eigens = solver.RealEigenvalues;
                        //некоторые оси могут быть развёрнуты, поэтому нужно смотреть, какие векторы и куда действительно смотрят
                        var eigenvectors = solver.Eigenvectors;
                        int indexX = 0;
                        int indexY = 0;
                        int indexZ = 0;
                        for (int k = 0; k < 3; k++)
                        { 
                                if (Math.Abs(eigenvectors[0, k]) >= 0.7)
                                {
                                    indexX = k;
                                    break;
                                }
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            if (Math.Abs(eigenvectors[1, k]) >= 0.7)
                            {
                                indexY = k;
                                break;
                            }
                        }
                        for (int k = 0; k < 3; k++)
                        {
                            if (Math.Abs(eigenvectors[2, k]) >= 0.7)
                            {
                                indexZ = k;
                                break;
                            }
                        }

                        

                        //ищем размеры щётки: по backbone и по соседним концам боковых цепей
                        #region всякие размеры щётки
                        
                        //int backbonecounter1 = 0;
                        //int backbonecounter2 = 238;
                        //int sideEnd1 = 253; 
                        
                            
                        if((StructFormer.GetAxDiameter(mol,0)>= 88) || (StructFormer.GetAxDiameter(mol, 1) >= 88) || (StructFormer.GetAxDiameter(mol, 2)>= 88))
                                {
                            //backbonecounter1 += 3824;
                           //backbonecounter2 += 3824;
                            //sideEnd1 += 3824;
                            
                            continue;
                                }

                        BackBone.Add(Analyzer.GetDistance(mol[backbonecounter1][0], mol[backbonecounter2][0], mol[backbonecounter1][1],
                            mol[backbonecounter2][1], mol[backbonecounter1][2], mol[backbonecounter2][2]));
                        //backbonecounter1 += 3824;
                        //backbonecounter2 += 3824;



                        //for (sideEnd1 = 253 + j*numberN; sideEnd1 < numberN + j*numberN - 15; sideEnd1 += 15)
                        //{
                        //        SideChainXY.Add(Analyzer.GetDistance(mol[sideEnd1][0], mol[sideEnd1 + 15][0], mol[sideEnd1][1],
                        //    mol[sideEnd1 + 15][1], mol[sideEnd1][2], mol[sideEnd1 + 15][2]));
                        //}

                        for (int b = sideEnd1; b < numberN - 15; b += 15)
                        {
                            SideChain.Add(Analyzer.GetDistance(mol[b][0], mol[b + 15][0], mol[b][1],
                        mol[b + 15][1], mol[b][2], mol[b + 15][2]));
                        }

                        //sideEnd1 += 3824;
                        BrushSideChain.Add(SideChain.Average());
                        
                        #endregion

                        // old approach to calc sizes - without tensor diagonalization
                        /*
                        XY[j] = StructFormer.GetHydroRadius2D(mol);
                        X[j] = Math.Sqrt(StructFormer.GetAxInertSquareRadius(mol, 0));
                        Y[j] = Math.Sqrt(StructFormer.GetAxInertSquareRadius(mol, 1));
                        Z[j] = Math.Sqrt(StructFormer.GetAxInertSquareRadius(mol, 2));
                        */

                        //new approach
                        X[j] = Math.Sqrt(eigens[indexX]/numberN);
                        Y[j] = Math.Sqrt(eigens[indexY]/numberN);
                        Z[j] = Math.Sqrt(eigens[indexZ]/numberN);
                        XY[j] = Math.Sqrt(X[j]*X[j]  + Y[j]*Y[j]);
                        
                        if (docentercut)
                        {
                            #region work with one gel для проверки расположения не у стенки

                            //work with one gel для проверки расположения не у стенки
                            var Yrad = Math.Sqrt(StructFormer.GetAxInertSquareRadius(mol, 1));
                            var Xrad = Math.Sqrt(StructFormer.GetAxInertSquareRadius(mol, 0));

                            var distX = Math.Abs(mol.Max(x => x[0]) - mol.Min(x => x[0]));
                            var distY = Math.Abs(mol.Max(x => x[1]) - mol.Min(x => x[1]));

                            if (distX < sizes[0] * 0.95 && distY < sizes[1] * 0.95)
                            {
                                centers.Add(StructFormer.GetCenterMass(mol));
                                cXY.Add(XY[j]);
                                cX.Add(X[j]);
                                cY.Add(Y[j]);
                            }

                            #endregion
                        }

                        if (calcOrientationalOrder)
                        {
                            #region вычисление ориентационного параметра порядка для одного геля

                            List<double[]> inputMg = new List<double[]>();
                            List<int[]> mgBonds = new List<int[]>();
                            List<int[]> mgAngles = new List<int[]>();
                            FileWorker.LoadConfLines(out sizes[0], out sizes[1], out sizes[2], openFileDialog.FileName, inputMg, mgBonds, mgAngles);
                            var bonds = (List<int[]>)args[1];
                            var mGel = MolData.ConvertToMolData(mol, true, bonds);
                            
                            double XCosAngle = 0;
                            double YCosAngle = 0;

                            var Molindex = 2;
                            // MolIndex = 2 means that this bead has been visited
                            var crossLinkers = Analyzer.GetCrossLinkers(mGel);

                            for (int m = 0; m < numberN; m++)
                            {
                                if (mol[m][3] == 1) //type A
                                {

                                    XCosAngle = Math.Cos(Analyzer.GetXYPlaneAngle(mol[m][0], mol[m][0], mol[m][1], 0));
                                    YCosAngle = Math.Cos(Analyzer.GetXYPlaneAngle(mol[m][0], 0, mol[m][1], mol[m][1]));

                                    gelXAnglesTypeA.Add(3 * XCosAngle * XCosAngle - 1);
                                    gelYAnglesTypeA.Add(3 * YCosAngle * YCosAngle - 1);
                                    XCosAngle = 0;
                                    YCosAngle = 0;

                                }

                                if (mol[m][3] == 1.01) //type B
                                {

                                    XCosAngle = Math.Cos(Analyzer.GetXYPlaneAngle(mol[m][0], mol[m][0], mol[m][1], 0));
                                    YCosAngle = Math.Cos(Analyzer.GetXYPlaneAngle(mol[m][0], 0, mol[m][1], mol[m][1]));

                                    gelXAnglesTypeB.Add(3 * XCosAngle * XCosAngle - 1);
                                    gelYAnglesTypeB.Add(3 * YCosAngle * YCosAngle - 1);
                                    XCosAngle = 0;
                                    YCosAngle = 0;
                                }
                            }

                            double sumAngle = 0;
                            double meantAngle = 0;


                            for (int p = 0; p < gelXAnglesTypeA.Count; p++)
                            {
                                sumAngle += gelXAnglesTypeA[p];
                            }
                            meantAngle = sumAngle / gelXAnglesTypeA.Count;
                            XOrientationalOrderTypeA[j] = 0.5 * meantAngle;
                            sumAngle = 0; meantAngle = 0;

                            for (int p = 0; p < gelYAnglesTypeA.Count; p++)
                            {
                                sumAngle += gelYAnglesTypeA[p];
                            }
                            meantAngle = sumAngle / gelXAnglesTypeA.Count;
                            YOrientationalOrderTypeA[j] = 0.5 * meantAngle;
                            sumAngle = 0; meantAngle = 0;

                            for (int p = 0; p < gelXAnglesTypeB.Count; p++)
                            {
                                sumAngle += gelXAnglesTypeB[p];
                            }
                            meantAngle = sumAngle / gelXAnglesTypeB.Count;
                            XOrientationalOrderTypeB[j] = 0.5 * meantAngle;
                            sumAngle = 0; meantAngle = 0;


                            for (int p = 0; p < gelYAnglesTypeB.Count; p++)
                            {
                                sumAngle += gelYAnglesTypeB[p];
                            }
                            meantAngle = sumAngle / gelYAnglesTypeB.Count;
                            YOrientationalOrderTypeB[j] = 0.5 * meantAngle;
                            sumAngle = 0; meantAngle = 0;
                            #endregion
                        }

                        if (dohollowuppercut)
                        {
                            #region отбираем центры масс полых гелей, оказавшихся выше межфазной границы, затем у тех, кто ниже

                            //отбираем центры масс полых гелей, оказавшихся выше межфазной границы, затем у тех, кто ниже
                            //centre положение межфазки по Z-axis
                            double counter = 0;
                            for (int k = 0; k < numberN; k++)
                            {
                                if (mol[k][2] <= centre)
                                {
                                    counter++;
                                }
                            }
                            if (counter / numberN <= 0.45)
                            {
                                hollowUpCentersmass.Add(centermass[j]);
                                hollowUpRxy.Add(XY[j]);
                            }
                            else
                            {
                                hollowDownCentersmass.Add(centermass[j]);
                                hollowDownRxy.Add(XY[j]);
                            }

                            #endregion
                        }
                        
                        
                        
                        /*
                        #region красим концы щётки в тип 1
                        int end = 253;

                        for (int p = 0; p < numberN; p++)
                        {
                            if (mol[p][4] == end)
                            {
                                mol[p][3] = 1;
                                end += 15;
                            }
                        }
                        BrushColoredEnds.AddRange(mol);

                        BrushWithColoredEnds = MolData.ShiftAll(false, 3, (int)sizes[0], (int)sizes[1], (int)sizes[2]
                                    , 0, 0, 0, BrushColoredEnds);


                        #endregion
                        */

                    }

                    if (docentercut)
                    {
                        #region вырезание центральных в монослое гелей в отдельный снэпшот
                        // отбираем все частицы в радиусе 1/3 Rxy от центров масс отобранных раннее центральных гелей

                        for (int k = 0; k < centers.Count; k++)
                        {

                            cylinders.AddRange(file.Where(x => Math.Sqrt(Math.Pow(Math.Abs(x[0] - centers[k][0]), 2) + Math.Pow(Math.Abs(x[1] - centers[k][1]), 2))
                                                                <= 0.33 * Math.Min(cY[k],cX[k])).ToList());
                        }


                        cylmol = MolData.ShiftAll(false, 3, (int)sizes[0], (int)sizes[1], (int)sizes[2]
                                , 0, 0, 0, cylinders);

                        FileWorker.SaveLammpstrj(false, tbPath.Text + "//res" + (i + 1).ToString() + ".lammpstrj",
                                                 1, sizes, 3, cylmol);
                        #endregion
                    }

                    /*
                    //сохраняем щётку
                    FileWorker.SaveLammpstrj(false, tbPath.Text + "//brush" + (i + 1).ToString() + ".lammpstrj",
                                                                 1, sizes, 3, BrushWithColoredEnds);
                    */



                    if (dohollowuppercut)
                    {
                        #region вырезаем в отдельный снэпшот полые гели с полостью вверх и полые гели с полостью вниз

                        //отбираем частицы растворителей в радиусе 0.6 Rxy от центров масс отобранных раннее полых гелей из тех, что выше/ниже 
                        for (int p = 0; p < hollowUpCentersmass.Count; p++)
                        {

                            Uphollows.AddRange(file.Where(x => Math.Sqrt(Math.Pow(Math.Abs(x[0] - hollowUpCentersmass[p][0]), 2) + Math.Pow(Math.Abs(x[1] - hollowUpCentersmass[p][1]), 2))
                                                                <= 0.6 * hollowUpRxy[p]).ToList());
                            Uphollows.RemoveAll(x => x[3] == 1);
                            Uphollows.RemoveAll(x => x[3] == 1.01);
                        }



                        for (int p = 0; p < hollowDownCentersmass.Count; p++)
                        {
                            Downhollows.AddRange(file.Where(x => Math.Sqrt(Math.Pow(Math.Abs(x[0] - hollowDownCentersmass[p][0]), 2) + Math.Pow(Math.Abs(x[1] - hollowDownCentersmass[p][1]), 2))
                                                                <= 0.6 * hollowDownRxy[p]).ToList());
                            Downhollows.RemoveAll(x => x[3] == 1);
                            Downhollows.RemoveAll(x => x[3] == 1.01);
                        }


                        UpperHollowmol = MolData.ShiftAll(false, 3, (int)sizes[0], (int)sizes[1], (int)sizes[2]
                                , 0, 0, 0, Uphollows);

                        DownHollowmol = MolData.ShiftAll(false, 3, (int)sizes[0], (int)sizes[1], (int)sizes[2]
                                , 0, 0, 0, Downhollows);

                        FileWorker.SaveLammpstrj(false, tbPath.Text + "//hollowup" + (i + 1).ToString() + ".lammpstrj",
                                                         1, sizes, 3, UpperHollowmol);

                        FileWorker.SaveLammpstrj(false, tbPath.Text + "//hollowdown" + (i + 1).ToString() + ".lammpstrj",
                                                 1, sizes, 3, DownHollowmol);

                        #endregion
                    }

                    /*  #region вычисление радиусов гелей по ансамблю и среднекв. отклонений в плоскости XY и в проекции на ось Z
                          double radXY, radZ;
                          double summ = 0;
                          for (int k = 0; k < XY.Length; k++)
                          {
                              summ += XY[k];
                          }
                          radXY = summ / molAmount; summ = 0;
                          XYfull[i] = Math.Round(radXY, 2);

                          for (int k = 0; k < Z.Length; k++)
                          {
                              summ += Z[k];
                          }
                          radZ = summ / molAmount; summ = 0;
                          Zfull[i] = Math.Round(radZ, 2);
                          #endregion
                      */

                    //вычисление плотностей в пограничном с межфазной границей слое 

                    if (dointerfacecut)
                    {

                        #region density

                        int polOneCount = 0;
                        int polTwoCount = 0;
                        int polThreeCount = 0;
                        int solvAcount = 0;
                        int solvBcount = 0;
                        int watercount = 0;
                        int totalcount = 0;

                        //изначальный вариант
                        var layer = file.Where(x => x[2] > (centre - epsilon) &&
                        x[2] <= (centre + epsilon)).ToList();

                        //для асимметричных случаев
                        var layer1 = file.Where(x => x[2] <= centre && x[2] >= (centre - 2 * epsilon)).ToList();

                        //для симметричных случаев
                        var layer2 = file.Where(x => x[2] >= centre && x[2] <= (centre + 2 * epsilon)).ToList();


                        double h = 2.0 * epsilon;

                        totalcount += layer1.Count;
                        if (totalcount == 0)
                        {
                            totalcount++;
                        }

                        foreach (var c in layer1)
                        {
                            if (c[3] == 1.000)
                            {
                                polOneCount++;
                            }
                            if (c[3] == 1.010)
                            {
                                polTwoCount++;
                            }
                            if (c[3] == 1.040)
                            {
                                polThreeCount++;
                            }
                            if (c[3] == 1.020)
                            {
                                solvAcount++;
                            }
                            if (c[3] == 1.070)
                            {
                                solvBcount++;
                            }
                            if (c[3] == 1.030)
                            {
                                watercount++;
                            }
                        }
                        density[0] = Math.Round((polOneCount / (sizes[0] * sizes[1] * h)), 2); //плотность полимера
                        density[1] = Math.Round(((double)polOneCount / (double)totalcount), 2); //объёмная доля полимера
                        density[2] = Math.Round((totalcount / (sizes[0] * sizes[1] * h)), 2); //общая плотность
                        poldens[i] = density[0];
                        volpoldens[i] = density[1];
                        fulldens[i] = density[2];

                        file.Clear();
                        snapCount = 1;
                        sizes = new double[3];

                        #endregion

                    }
                    //progress
                    int barStep = (int)(files.Length / 100.0);
                    if (barStep == 0)
                    {
                        barStep++;
                    }

                    if ((i + 1) % barStep == 0)
                    {
                        ((BackgroundWorker)sender).ReportProgress((int)(100.0 * ((double)(i + 1)) / ((double)files.Length)));
                    }

                    #region средние радиусы с погрешностями и плотности в слое по ансамблю
                    //для обработки размеров щёток
                    #region обработки размеров щёток
                    /*
                     double sum = 0;
                     double meantBackbone, meantSideChain, sqBackBone, sqSideChain;
                     double brushsum = 0;
                     int check = 0;
                     for(int k=0; k<BackBone.Count; k++)   
                     {
                         if (BackBone[k] > 90)
                         {
                             continue;
                         }
                         brushsum += BackBone[k];
                         check++;
                     }
                     meantBackbone = brushsum / check;
                     res[0] = Math.Round(meantBackbone, 2);
                     check = 0; brushsum = 0;

                     for (int k = 0; k < BrushSideChain.Count; k++)
                     {
                         if (BrushSideChain[k] > 90)
                         {
                             continue;
                         }
                         brushsum += BrushSideChain[k];
                         check++;
                     }
                     meantSideChain = brushsum / check;
                     res[1] = Math.Round(meantSideChain, 2);
                     check = 0; brushsum = 0;


                     for (int k = 0; k < BackBone.Count; k++)
                     {
                         brushsum += Math.Pow((BackBone[k] - meantBackbone), 2);
                     }
                     sqBackBone = Math.Sqrt(brushsum / (BackBone.Count - 1)); brushsum = 0;
                     res[2] = Math.Round(sqBackBone, 2);

                     for (int k = 0; k < BrushSideChain.Count; k++)
                     {
                         brushsum += Math.Pow((BrushSideChain[k] - meantSideChain), 2);
                     }
                     sqSideChain = Math.Sqrt(brushsum / (BrushSideChain.Count - 1)); brushsum = 0;
                     res[3] = Math.Round(sqSideChain, 2);
                     */
                    #endregion
                    
                    double meantradX, meantradY, meantradXY, meantradZ, sqradX, sqradY, sqradXY, sqradZ, mpoldens, mfulldens, mvolpoldens;
                    double sum = 0;

                    for (int k = 0; k < X.Length; k++)  //X-rad
                    {
                        sum += X[k];
                    }
                    meantradX = sum / molAmount; sum = 0;
                    res[0] = Math.Round(meantradX, 2);

                    for (int k = 0; k < Y.Length; k++) //Y - rad
                    {
                        sum += Y[k];
                    }
                    meantradY = sum / molAmount; sum = 0;
                    res[1] = Math.Round(meantradY, 2);


                    for (int k = 0; k < XY.Length; k++) //XY - rad
                    {
                        sum += XY[k];
                    }
                    meantradXY = sum / molAmount; sum = 0;
                    res[2] = Math.Round(meantradXY, 2);

                    
                    for (int k = 0; k < Z.Length; k++) //Z - rad
                    {
                        sum += Z[k];
                    }
                    meantradZ = sum / molAmount; sum = 0;
                    res[3] = Math.Round(meantradZ, 2);
                    
                    for (int k = 0; k < X.Length; k++) //sqX - rad
                    {
                        sum += Math.Pow((X[k] - meantradX), 2);
                    }
                    sqradX = Math.Sqrt(sum / (X.Length - 1)); sum = 0;
                    res[4] = Math.Round(sqradX, 2);
                    
                    for (int k = 0; k < Y.Length; k++) //sqY -rad
                    {
                        sum += Math.Pow((Y[k] - meantradY), 2);
                    }
                    sqradY = Math.Sqrt(sum / (Y.Length - 1)); sum = 0;
                    res[5] = Math.Round(sqradY, 2);

                    for (int k = 0; k < XY.Length; k++) //sqXY -rad
                    {
                        sum += Math.Pow((XY[k] - meantradXY), 2);
                    }
                    //sqradXY = Math.Sqrt(sum / (XYfull.Length - 1)); sum = 0;
                    sqradXY = Math.Sqrt(sum / (XY.Length - 1)); sum = 0;
                    res[6] = Math.Round(sqradXY, 2);

                    for (int k = 0; k < Z.Length; k++) //sqZ - rad
                    {
                        sum += Math.Pow((Z[k] - meantradZ), 2);
                    }
                    sqradZ = Math.Sqrt(sum / (Z.Length - 1)); sum = 0;
                    res[7] = Math.Round(sqradZ, 2);
                    
                    for (int k = 0; k < poldens.Length; k++)
                    {
                        sum += Math.Pow((poldens[k]), 2);
                    }
                    mpoldens = Math.Sqrt(sum / (poldens.Length)); sum = 0;
                    res[8] = Math.Round(mpoldens, 2);

                    for (int k = 0; k < volpoldens.Length; k++)
                    {
                        sum += Math.Pow((volpoldens[k]), 2);
                    }
                    mvolpoldens = Math.Sqrt(sum / (fulldens.Length)); sum = 0;
                    res[9] = Math.Round(mvolpoldens, 2);

                    for (int k = 0; k < fulldens.Length; k++)
                    {
                        sum += Math.Pow((fulldens[k]), 2);
                    }
                    mfulldens = Math.Sqrt(sum / (volpoldens.Length)); sum = 0;
                    res[10] = Math.Round(mfulldens, 2);
                    #endregion

                    #region усредняем параметр порядка
                    double XSystemOrientationalOrderTypeA = 0;
                    double YSystemOrientationalOrderTypeA = 0;
                    double XSystemOrientationalOrderTypeB = 0;
                    double YSystemOrientationalOrderTypeB = 0;


                    for (int q = 0; q < XOrientationalOrderTypeA.Length; q++)
                    {
                        sum += XOrientationalOrderTypeA[q];
                    }
                    XSystemOrientationalOrderTypeA = sum / molAmount; sum = 0;
                    res[11] = XSystemOrientationalOrderTypeA;

                    for (int q = 0; q < YOrientationalOrderTypeA.Length; q++)
                    {
                        sum += YOrientationalOrderTypeA[q];
                    }
                    YSystemOrientationalOrderTypeA = sum / molAmount; sum = 0;
                    res[12] = YSystemOrientationalOrderTypeA;


                    for (int q = 0; q < XOrientationalOrderTypeB.Length; q++)
                    {
                        sum += XOrientationalOrderTypeB[q];
                    }
                    XSystemOrientationalOrderTypeB = sum / molAmount; sum = 0;
                    res[13] = XSystemOrientationalOrderTypeB;

                    for (int q = 0; q < YOrientationalOrderTypeB.Length; q++)
                    {
                        sum += YOrientationalOrderTypeB[q];
                    }
                    YSystemOrientationalOrderTypeB = sum / molAmount; sum = 0;
                    res[14] = YSystemOrientationalOrderTypeB;
                    #endregion

                    GeneralRes.Add(res);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("ошибка в файле № " + i + 1 + "!");
                    MessageBox.Show(ex.ToString());
                }
            }

            var CalcResult = new double[15];

            //усреднение по снэпшотам
            for(int i=0; i<CalcResult.Length; i++)
            {
                CalcResult[i] = Analyzer.GetAverageOfElementFromCollection(GeneralRes, GeneralRes.Count, i);
            }

                    

            e.Result = new object[] { molCount, CalcResult, sizes};
        }
        private void bgWorkerFindGroup_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void bgWorkerFindGroup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = (object[])e.Result;
            tbMolNum.Text = ((int)args[0]).ToString();
            tbXradius.Text = ((double[])args[1])[0].ToString();
            tbYradius.Text = ((double[])args[1])[1].ToString();
            tbXYradius.Text = ((double[])args[1])[2].ToString();
            tbZradius.Text = ((double[])args[1])[3].ToString();
            tbsqXradius.Text = ((double[])args[1])[4].ToString();
            tbsqYradius.Text = ((double[])args[1])[5].ToString();
            tbsqXYradius.Text = ((double[])args[1])[6].ToString();
            tbsqZradius.Text = ((double[])args[1])[7].ToString();
            tbDens1.Text = ((double[])args[1])[8].ToString();
            tbDens2.Text = ((double[])args[1])[9].ToString();
            tbDens3.Text = ((double[])args[1])[10].ToString();
            tbXOrientationalOrderTypeA.Text = ((double[])args[1])[11].ToString();
            tbYOrientationalOrderTypeA.Text = ((double[])args[1])[12].ToString();
            tbXOrientationalOrderTypeB.Text = ((double[])args[1])[13].ToString();
            tbYOrientationalOrderTypeB.Text = ((double[])args[1])[14].ToString();

            pBar.Value = 0;
        }

        private void btnChooseMgel_Click(object sender, EventArgs e)  //in fact it's related to recolor page when user choose path
                                                                                        //to microgel which should be recolored
        {
            openFileDialog.Filter = "Конфиг. файлы Lammps (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.InitialDirectory = openFileDialog.FileName;
                tbMicrogelPath.Text = openFileDialog.FileName;
            }
        }


        #region перекраска одного геля в диблочный

        private void button4_Click(object sender, EventArgs e)
        {
            var colorLength = Convert.ToInt32(tbRecolorLength.Text);
            var subchainLength = Convert.ToInt32(tbSubchainLength.Text);

            List<double[]> inputMg = new List<double[]>();
            List<int[]> mgBonds = new List<int[]>();
            List<int[]> mgAngles = new List<int[]>();

            var sizes = new double[] { 0.0, 0.0, 0.0 };
            try
            {
                FileWorker.LoadConfLines(out sizes[0], out sizes[1], out sizes[2], openFileDialog.FileName, inputMg, mgBonds, mgAngles);           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Произошла ошибка при чтении!\nУбедитесь, что выбранный файл имеет нужный формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            bgWorkerRecolor.RunWorkerAsync(new object[] { inputMg, mgBonds, colorLength, subchainLength, sizes });
        }

        private void bgWorkerRecolor_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (object[])e.Argument;
            var mol = (List<double[]>)args[0];
            var bonds = (List<int[]>)args[1];
            var colorLength = (int)args[2];
            var subchainLength = (int)args[3];
            var sizes = (double[])args[4];

            var mGel = MolData.ConvertToMolData(mol, true, bonds);

            // Do the recolor
            Analyzer.RecolorALL(mGel, subchainLength, colorLength);

            var savePath = Path.GetDirectoryName(tbMicrogelPath.Text);

            e.Result = new object[] { mGel, sizes, savePath };

            //S parameter

            //double result = Analyzer.Calculate_S_Parameter(mGel, subchainLength, colorLength);

           // MessageBox.Show(result.ToString());
        }

        #endregion


        private void bgWorkerRecolor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void bgWorkerRecolor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = (object[])e.Result;
            var mGel = (List<MolData>)args[0];
            var sizes = (double[])args[1];
            var savePath = (string)args[2];
            pBar.Value = 0;

            FileWorker.SaveLammpstrj(false, savePath + "//diblocked" + ".lammpstrj",
                                    1, sizes, 0, mGel);

            MessageBox.Show("Work is done!");

        }

        private void button_S_Parameter_Click(object sender, EventArgs e)
        {
            var colorLength = Convert.ToInt32(tbRecolorLength.Text);
            var subchainLength = Convert.ToInt32(tbSubchainLength.Text);

            List<double[]> inputMg = new List<double[]>();
            List<int[]> mgBonds = new List<int[]>();
            List<int[]> mgAngles = new List<int[]>();

            var sizes = new double[] { 0.0, 0.0, 0.0 };
            FileWorker.LoadConfLines(out sizes[0], out sizes[1], out sizes[2], openFileDialog.FileName, inputMg, mgBonds, mgAngles);
            bgWorkerRecolor.RunWorkerAsync(new object[] { inputMg, mgBonds, colorLength, subchainLength, sizes });

           // MessageBox.Show(result);
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}
