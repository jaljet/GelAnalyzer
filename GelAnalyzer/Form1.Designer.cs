namespace GelAnalyzer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbXYradius = new System.Windows.Forms.TextBox();
            this.tbZradius = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMolNum = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbN = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblProgBar = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSurfArea = new System.Windows.Forms.TextBox();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.bgSurfWorker = new System.ComponentModel.BackgroundWorker();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.MolType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MolCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radXY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.surfCov = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RxyDisp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RzDisp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClearTable = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStart1 = new System.Windows.Forms.Button();
            this.bgWorkerFindGroup = new System.ComponentModel.BackgroundWorker();
            this.label7 = new System.Windows.Forms.Label();
            this.tbA = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbsqXYradius = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbsqZradius = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDens1 = new System.Windows.Forms.TextBox();
            this.tbStep = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbDensCentre = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDens3 = new System.Windows.Forms.TextBox();
            this.tbDens2 = new System.Windows.Forms.TextBox();
            this.tcGeneral = new System.Windows.Forms.TabControl();
            this.tpMonolayers = new System.Windows.Forms.TabPage();
            this.tbXOrientationalOrderTypeA = new System.Windows.Forms.TextBox();
            this.checkCenterCut = new System.Windows.Forms.CheckBox();
            this.checkAxOrientationalParameter = new System.Windows.Forms.CheckBox();
            this.tpageRecolor = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.tbSubchainLength = new System.Windows.Forms.TextBox();
            this.btnChooseMgel = new System.Windows.Forms.Button();
            this.tbMicrogelPath = new System.Windows.Forms.TextBox();
            this.tbRecolorLength = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.bgWorkerRecolor = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbXOrientationalOrderTypeB = new System.Windows.Forms.TextBox();
            this.lblOrientationalOrderTypeA = new System.Windows.Forms.Label();
            this.lblOrientationalOrderTypeB = new System.Windows.Forms.Label();
            this.tbYOrientationalOrderTypeA = new System.Windows.Forms.TextBox();
            this.tbYOrientationalOrderTypeB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.tcGeneral.SuspendLayout();
            this.tpMonolayers.SuspendLayout();
            this.tpageRecolor.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "Выберите папку...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Число частиц в молекуле:";
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(31, 445);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(159, 25);
            this.pBar.TabIndex = 2;
            this.pBar.Visible = false;
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(149, 19);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(233, 20);
            this.tbPath.TabIndex = 3;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "C:\\";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Inertia\'s radius value on XY - flat:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Inertia\'s radius value on Z-axis:";
            // 
            // tbXYradius
            // 
            this.tbXYradius.Location = new System.Drawing.Point(190, 134);
            this.tbXYradius.Name = "tbXYradius";
            this.tbXYradius.Size = new System.Drawing.Size(66, 20);
            this.tbXYradius.TabIndex = 6;
            this.tbXYradius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbZradius
            // 
            this.tbZradius.Location = new System.Drawing.Point(190, 164);
            this.tbZradius.Name = "tbZradius";
            this.tbZradius.Size = new System.Drawing.Size(66, 20);
            this.tbZradius.TabIndex = 7;
            this.tbZradius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Number of molecules (for single):";
            // 
            // tbMolNum
            // 
            this.tbMolNum.Location = new System.Drawing.Point(190, 105);
            this.tbMolNum.Name = "tbMolNum";
            this.tbMolNum.Size = new System.Drawing.Size(42, 20);
            this.tbMolNum.TabIndex = 9;
            this.tbMolNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbMolNum.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(24, 357);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(174, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Сохранить в таблицу";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(24, 324);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(162, 27);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Пуск (одиночная молекула)";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbN
            // 
            this.tbN.Location = new System.Drawing.Point(172, 75);
            this.tbN.Name = "tbN";
            this.tbN.Size = new System.Drawing.Size(60, 20);
            this.tbN.TabIndex = 12;
            this.tbN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblProgBar
            // 
            this.lblProgBar.AutoSize = true;
            this.lblProgBar.Location = new System.Drawing.Point(28, 429);
            this.lblProgBar.Name = "lblProgBar";
            this.lblProgBar.Size = new System.Drawing.Size(59, 13);
            this.lblProgBar.TabIndex = 14;
            this.lblProgBar.Text = "Прогресс:";
            this.lblProgBar.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 301);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Surface area:";
            // 
            // tbSurfArea
            // 
            this.tbSurfArea.Location = new System.Drawing.Point(114, 298);
            this.tbSurfArea.Name = "tbSurfArea";
            this.tbSurfArea.Size = new System.Drawing.Size(142, 20);
            this.tbSurfArea.TabIndex = 16;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // bgSurfWorker
            // 
            this.bgSurfWorker.WorkerReportsProgress = true;
            this.bgSurfWorker.WorkerSupportsCancellation = true;
            this.bgSurfWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSurfWorker_DoWork);
            this.bgSurfWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSurfWorker_ProgressChanged);
            this.bgSurfWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSurfWorker_RunWorkerCompleted);
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MolType,
            this.MolCount,
            this.radXY,
            this.radZ,
            this.surfCov,
            this.RxyDisp,
            this.RzDisp});
            this.dgvData.Location = new System.Drawing.Point(573, 19);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(544, 352);
            this.dgvData.TabIndex = 17;
            // 
            // MolType
            // 
            this.MolType.HeaderText = "Молекула";
            this.MolType.Name = "MolType";
            // 
            // MolCount
            // 
            this.MolCount.HeaderText = "Число мол-л";
            this.MolCount.Name = "MolCount";
            // 
            // radXY
            // 
            this.radXY.HeaderText = "XY радиус";
            this.radXY.Name = "radXY";
            // 
            // radZ
            // 
            this.radZ.HeaderText = "Z радиус";
            this.radZ.Name = "radZ";
            // 
            // surfCov
            // 
            this.surfCov.HeaderText = "% пов-ти";
            this.surfCov.Name = "surfCov";
            // 
            // RxyDisp
            // 
            this.RxyDisp.HeaderText = "Дисперсия XY-радиуса";
            this.RxyDisp.Name = "RxyDisp";
            // 
            // RzDisp
            // 
            this.RzDisp.HeaderText = "Дисперсия Z-радиуса";
            this.RzDisp.Name = "RzDisp";
            // 
            // btnClearTable
            // 
            this.btnClearTable.Location = new System.Drawing.Point(241, 360);
            this.btnClearTable.Name = "btnClearTable";
            this.btnClearTable.Size = new System.Drawing.Size(109, 25);
            this.btnClearTable.TabIndex = 18;
            this.btnClearTable.Text = "Очистить";
            this.btnClearTable.UseVisualStyleBackColor = true;
            this.btnClearTable.Click += new System.EventHandler(this.btnClearTable_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1097, 608);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 25);
            this.button2.TabIndex = 19;
            this.button2.Text = "Выход";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnStart1
            // 
            this.btnStart1.Location = new System.Drawing.Point(216, 326);
            this.btnStart1.Name = "btnStart1";
            this.btnStart1.Size = new System.Drawing.Size(153, 23);
            this.btnStart1.TabIndex = 20;
            this.btnStart1.Text = "Пуск (группа молекул)";
            this.btnStart1.UseVisualStyleBackColor = true;
            this.btnStart1.Click += new System.EventHandler(this.btnStart1_Click);
            // 
            // bgWorkerFindGroup
            // 
            this.bgWorkerFindGroup.WorkerReportsProgress = true;
            this.bgWorkerFindGroup.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerFindGroup_DoWork);
            this.bgWorkerFindGroup.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerFindGroup_ProgressChanged);
            this.bgWorkerFindGroup.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerFindGroup_RunWorkerCompleted);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Число молекул:";
            // 
            // tbA
            // 
            this.tbA.Location = new System.Drawing.Point(114, 52);
            this.tbA.Name = "tbA";
            this.tbA.Size = new System.Drawing.Size(100, 20);
            this.tbA.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Dispersion of R on XY- flat:";
            // 
            // tbsqXYradius
            // 
            this.tbsqXYradius.Location = new System.Drawing.Point(160, 192);
            this.tbsqXYradius.Name = "tbsqXYradius";
            this.tbsqXYradius.Size = new System.Drawing.Size(86, 20);
            this.tbsqXYradius.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 222);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Dispersion of R on Z-axis:";
            // 
            // tbsqZradius
            // 
            this.tbsqZradius.Location = new System.Drawing.Point(160, 218);
            this.tbsqZradius.Name = "tbsqZradius";
            this.tbsqZradius.Size = new System.Drawing.Size(86, 20);
            this.tbsqZradius.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(238, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Центр плотности:";
            // 
            // tbDens1
            // 
            this.tbDens1.Location = new System.Drawing.Point(354, 101);
            this.tbDens1.Name = "tbDens1";
            this.tbDens1.Size = new System.Drawing.Size(40, 20);
            this.tbDens1.TabIndex = 29;
            // 
            // tbStep
            // 
            this.tbStep.Location = new System.Drawing.Point(331, 52);
            this.tbStep.Name = "tbStep";
            this.tbStep.Size = new System.Drawing.Size(63, 20);
            this.tbStep.TabIndex = 30;
            this.tbStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(220, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Шаг по координате:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(234, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Плотность полимера:";
            // 
            // tbDensCentre
            // 
            this.tbDensCentre.Location = new System.Drawing.Point(340, 75);
            this.tbDensCentre.Name = "tbDensCentre";
            this.tbDensCentre.Size = new System.Drawing.Size(40, 20);
            this.tbDensCentre.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(262, 137);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "Об. доля полимера:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(262, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "Общая плотность:";
            // 
            // tbDens3
            // 
            this.tbDens3.Location = new System.Drawing.Point(368, 164);
            this.tbDens3.Name = "tbDens3";
            this.tbDens3.Size = new System.Drawing.Size(40, 20);
            this.tbDens3.TabIndex = 36;
            // 
            // tbDens2
            // 
            this.tbDens2.Location = new System.Drawing.Point(375, 134);
            this.tbDens2.Name = "tbDens2";
            this.tbDens2.Size = new System.Drawing.Size(40, 20);
            this.tbDens2.TabIndex = 37;
            // 
            // tcGeneral
            // 
            this.tcGeneral.Controls.Add(this.tpMonolayers);
            this.tcGeneral.Controls.Add(this.tpageRecolor);
            this.tcGeneral.Location = new System.Drawing.Point(2, 1);
            this.tcGeneral.Name = "tcGeneral";
            this.tcGeneral.SelectedIndex = 0;
            this.tcGeneral.Size = new System.Drawing.Size(1144, 425);
            this.tcGeneral.TabIndex = 38;
            // 
            // tpMonolayers
            // 
            this.tpMonolayers.BackColor = System.Drawing.SystemColors.Control;
            this.tpMonolayers.Controls.Add(this.tbYOrientationalOrderTypeB);
            this.tpMonolayers.Controls.Add(this.tbYOrientationalOrderTypeA);
            this.tpMonolayers.Controls.Add(this.lblOrientationalOrderTypeB);
            this.tpMonolayers.Controls.Add(this.lblOrientationalOrderTypeA);
            this.tpMonolayers.Controls.Add(this.tbXOrientationalOrderTypeB);
            this.tpMonolayers.Controls.Add(this.tbXOrientationalOrderTypeA);
            this.tpMonolayers.Controls.Add(this.checkCenterCut);
            this.tpMonolayers.Controls.Add(this.checkAxOrientationalParameter);
            this.tpMonolayers.Controls.Add(this.dgvData);
            this.tpMonolayers.Controls.Add(this.tbSurfArea);
            this.tpMonolayers.Controls.Add(this.label6);
            this.tpMonolayers.Controls.Add(this.tbDens2);
            this.tpMonolayers.Controls.Add(this.button1);
            this.tpMonolayers.Controls.Add(this.tbDens3);
            this.tpMonolayers.Controls.Add(this.label1);
            this.tpMonolayers.Controls.Add(this.label13);
            this.tpMonolayers.Controls.Add(this.label12);
            this.tpMonolayers.Controls.Add(this.tbPath);
            this.tpMonolayers.Controls.Add(this.tbDensCentre);
            this.tpMonolayers.Controls.Add(this.label2);
            this.tpMonolayers.Controls.Add(this.label11);
            this.tpMonolayers.Controls.Add(this.label3);
            this.tpMonolayers.Controls.Add(this.label10);
            this.tpMonolayers.Controls.Add(this.tbXYradius);
            this.tpMonolayers.Controls.Add(this.tbStep);
            this.tpMonolayers.Controls.Add(this.tbZradius);
            this.tpMonolayers.Controls.Add(this.tbDens1);
            this.tpMonolayers.Controls.Add(this.label4);
            this.tpMonolayers.Controls.Add(this.label5);
            this.tpMonolayers.Controls.Add(this.tbMolNum);
            this.tpMonolayers.Controls.Add(this.tbsqZradius);
            this.tpMonolayers.Controls.Add(this.btnSave);
            this.tpMonolayers.Controls.Add(this.label9);
            this.tpMonolayers.Controls.Add(this.btnStart);
            this.tpMonolayers.Controls.Add(this.tbsqXYradius);
            this.tpMonolayers.Controls.Add(this.tbN);
            this.tpMonolayers.Controls.Add(this.label8);
            this.tpMonolayers.Controls.Add(this.tbA);
            this.tpMonolayers.Controls.Add(this.label7);
            this.tpMonolayers.Controls.Add(this.btnStart1);
            this.tpMonolayers.Controls.Add(this.btnClearTable);
            this.tpMonolayers.Location = new System.Drawing.Point(4, 22);
            this.tpMonolayers.Name = "tpMonolayers";
            this.tpMonolayers.Padding = new System.Windows.Forms.Padding(3);
            this.tpMonolayers.Size = new System.Drawing.Size(1136, 399);
            this.tpMonolayers.TabIndex = 0;
            this.tpMonolayers.Text = "Монослои";
            // 
            // tbXOrientationalOrderTypeA
            // 
            this.tbXOrientationalOrderTypeA.Location = new System.Drawing.Point(262, 233);
            this.tbXOrientationalOrderTypeA.Name = "tbXOrientationalOrderTypeA";
            this.tbXOrientationalOrderTypeA.Size = new System.Drawing.Size(66, 20);
            this.tbXOrientationalOrderTypeA.TabIndex = 40;
            // 
            // checkCenterCut
            // 
            this.checkCenterCut.AutoSize = true;
            this.checkCenterCut.Location = new System.Drawing.Point(386, 77);
            this.checkCenterCut.Name = "checkCenterCut";
            this.checkCenterCut.Size = new System.Drawing.Size(172, 17);
            this.checkCenterCut.TabIndex = 39;
            this.checkCenterCut.Text = "Вырезать центральные гели";
            this.checkCenterCut.UseVisualStyleBackColor = true;
            // 
            // checkAxOrientationalParameter
            // 
            this.checkAxOrientationalParameter.AutoSize = true;
            this.checkAxOrientationalParameter.Location = new System.Drawing.Point(24, 247);
            this.checkAxOrientationalParameter.Name = "checkAxOrientationalParameter";
            this.checkAxOrientationalParameter.Size = new System.Drawing.Size(232, 17);
            this.checkAxOrientationalParameter.TabIndex = 38;
            this.checkAxOrientationalParameter.Text = "Посчитать ориентационный параметр S:";
            this.checkAxOrientationalParameter.UseVisualStyleBackColor = true;
            // 
            // tpageRecolor
            // 
            this.tpageRecolor.BackColor = System.Drawing.SystemColors.Control;
            this.tpageRecolor.Controls.Add(this.label15);
            this.tpageRecolor.Controls.Add(this.tbSubchainLength);
            this.tpageRecolor.Controls.Add(this.btnChooseMgel);
            this.tpageRecolor.Controls.Add(this.tbMicrogelPath);
            this.tpageRecolor.Controls.Add(this.tbRecolorLength);
            this.tpageRecolor.Controls.Add(this.label14);
            this.tpageRecolor.Controls.Add(this.button4);
            this.tpageRecolor.Location = new System.Drawing.Point(4, 22);
            this.tpageRecolor.Name = "tpageRecolor";
            this.tpageRecolor.Padding = new System.Windows.Forms.Padding(3);
            this.tpageRecolor.Size = new System.Drawing.Size(1136, 399);
            this.tpageRecolor.TabIndex = 1;
            this.tpageRecolor.Text = "Перекраска";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(257, 52);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Длина субцепи:";
            // 
            // tbSubchainLength
            // 
            this.tbSubchainLength.Location = new System.Drawing.Point(368, 49);
            this.tbSubchainLength.Name = "tbSubchainLength";
            this.tbSubchainLength.Size = new System.Drawing.Size(63, 20);
            this.tbSubchainLength.TabIndex = 32;
            this.tbSubchainLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnChooseMgel
            // 
            this.btnChooseMgel.Location = new System.Drawing.Point(27, 16);
            this.btnChooseMgel.Name = "btnChooseMgel";
            this.btnChooseMgel.Size = new System.Drawing.Size(114, 20);
            this.btnChooseMgel.TabIndex = 24;
            this.btnChooseMgel.Text = "Выберите микрогель";
            this.btnChooseMgel.UseVisualStyleBackColor = true;
            this.btnChooseMgel.Click += new System.EventHandler(this.btnChooseMgel_Click);
            // 
            // tbMicrogelPath
            // 
            this.tbMicrogelPath.Location = new System.Drawing.Point(147, 16);
            this.tbMicrogelPath.Name = "tbMicrogelPath";
            this.tbMicrogelPath.Size = new System.Drawing.Size(233, 20);
            this.tbMicrogelPath.TabIndex = 25;
            // 
            // tbRecolorLength
            // 
            this.tbRecolorLength.Location = new System.Drawing.Point(136, 49);
            this.tbRecolorLength.Name = "tbRecolorLength";
            this.tbRecolorLength.Size = new System.Drawing.Size(100, 20);
            this.tbRecolorLength.TabIndex = 28;
            this.tbRecolorLength.Text = "4";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(24, 52);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "Длина перекраски:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(27, 130);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(153, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "Пуск";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // bgWorkerRecolor
            // 
            this.bgWorkerRecolor.WorkerReportsProgress = true;
            this.bgWorkerRecolor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerRecolor_DoWork);
            this.bgWorkerRecolor.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerRecolor_ProgressChanged);
            this.bgWorkerRecolor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerRecolor_RunWorkerCompleted);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // tbXOrientationalOrderTypeB
            // 
            this.tbXOrientationalOrderTypeB.Location = new System.Drawing.Point(262, 259);
            this.tbXOrientationalOrderTypeB.Name = "tbXOrientationalOrderTypeB";
            this.tbXOrientationalOrderTypeB.Size = new System.Drawing.Size(66, 20);
            this.tbXOrientationalOrderTypeB.TabIndex = 41;
            // 
            // lblOrientationalOrderTypeA
            // 
            this.lblOrientationalOrderTypeA.AutoSize = true;
            this.lblOrientationalOrderTypeA.Location = new System.Drawing.Point(414, 236);
            this.lblOrientationalOrderTypeA.Name = "lblOrientationalOrderTypeA";
            this.lblOrientationalOrderTypeA.Size = new System.Drawing.Size(63, 13);
            this.lblOrientationalOrderTypeA.TabIndex = 42;
            this.lblOrientationalOrderTypeA.Text = "X,Y - type A";
            // 
            // lblOrientationalOrderTypeB
            // 
            this.lblOrientationalOrderTypeB.AutoSize = true;
            this.lblOrientationalOrderTypeB.Location = new System.Drawing.Point(414, 262);
            this.lblOrientationalOrderTypeB.Name = "lblOrientationalOrderTypeB";
            this.lblOrientationalOrderTypeB.Size = new System.Drawing.Size(63, 13);
            this.lblOrientationalOrderTypeB.TabIndex = 43;
            this.lblOrientationalOrderTypeB.Text = "X,Y - type B";
            // 
            // tbYOrientationalOrderTypeA
            // 
            this.tbYOrientationalOrderTypeA.Location = new System.Drawing.Point(342, 233);
            this.tbYOrientationalOrderTypeA.Name = "tbYOrientationalOrderTypeA";
            this.tbYOrientationalOrderTypeA.Size = new System.Drawing.Size(66, 20);
            this.tbYOrientationalOrderTypeA.TabIndex = 45;
            // 
            // tbYOrientationalOrderTypeB
            // 
            this.tbYOrientationalOrderTypeB.Location = new System.Drawing.Point(340, 259);
            this.tbYOrientationalOrderTypeB.Name = "tbYOrientationalOrderTypeB";
            this.tbYOrientationalOrderTypeB.Size = new System.Drawing.Size(68, 20);
            this.tbYOrientationalOrderTypeB.TabIndex = 46;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 645);
            this.Controls.Add(this.tcGeneral);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.lblProgBar);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "Microgel Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.tcGeneral.ResumeLayout(false);
            this.tpMonolayers.ResumeLayout(false);
            this.tpMonolayers.PerformLayout();
            this.tpageRecolor.ResumeLayout(false);
            this.tpageRecolor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbXYradius;
        private System.Windows.Forms.TextBox tbZradius;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMolNum;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbN;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblProgBar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSurfArea;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.ComponentModel.BackgroundWorker bgSurfWorker;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnClearTable;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnStart1;
        private System.ComponentModel.BackgroundWorker bgWorkerFindGroup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbA;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbsqXYradius;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbsqZradius;
        private System.Windows.Forms.DataGridViewTextBoxColumn MolType;
        private System.Windows.Forms.DataGridViewTextBoxColumn MolCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn radXY;
        private System.Windows.Forms.DataGridViewTextBoxColumn radZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn surfCov;
        private System.Windows.Forms.DataGridViewTextBoxColumn RxyDisp;
        private System.Windows.Forms.DataGridViewTextBoxColumn RzDisp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDens1;
        private System.Windows.Forms.TextBox tbStep;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbDensCentre;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbDens3;
        private System.Windows.Forms.TextBox tbDens2;
        private System.Windows.Forms.TabControl tcGeneral;
        private System.Windows.Forms.TabPage tpMonolayers;
        private System.Windows.Forms.TabPage tpageRecolor;
        private System.Windows.Forms.Button btnChooseMgel;
        private System.Windows.Forms.TextBox tbMicrogelPath;
        private System.Windows.Forms.TextBox tbRecolorLength;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button4;
        private System.ComponentModel.BackgroundWorker bgWorkerRecolor;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbSubchainLength;
        private System.Windows.Forms.CheckBox checkCenterCut;
        private System.Windows.Forms.CheckBox checkAxOrientationalParameter;
        private System.Windows.Forms.TextBox tbXOrientationalOrderTypeA;
        private System.Windows.Forms.TextBox tbXOrientationalOrderTypeB;
        private System.Windows.Forms.Label lblOrientationalOrderTypeB;
        private System.Windows.Forms.Label lblOrientationalOrderTypeA;
        private System.Windows.Forms.TextBox tbYOrientationalOrderTypeB;
        private System.Windows.Forms.TextBox tbYOrientationalOrderTypeA;
    }
}

