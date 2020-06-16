﻿namespace GelAnalyzer
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 12);
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
            this.label1.Location = new System.Drawing.Point(1, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Число частиц в молекуле:";
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(4, 309);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(159, 25);
            this.pBar.TabIndex = 2;
            this.pBar.Visible = false;
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(124, 12);
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
            this.label2.Location = new System.Drawing.Point(1, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Inertia\'s radius value on XY - flat:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Inertia\'s radius value on Z-axis:";
            // 
            // tbXYradius
            // 
            this.tbXYradius.Location = new System.Drawing.Point(165, 127);
            this.tbXYradius.Name = "tbXYradius";
            this.tbXYradius.Size = new System.Drawing.Size(66, 20);
            this.tbXYradius.TabIndex = 6;
            this.tbXYradius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbZradius
            // 
            this.tbZradius.Location = new System.Drawing.Point(165, 157);
            this.tbZradius.Name = "tbZradius";
            this.tbZradius.Size = new System.Drawing.Size(66, 20);
            this.tbZradius.TabIndex = 7;
            this.tbZradius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Number of molecules (for single):";
            // 
            // tbMolNum
            // 
            this.tbMolNum.Location = new System.Drawing.Point(165, 98);
            this.tbMolNum.Name = "tbMolNum";
            this.tbMolNum.Size = new System.Drawing.Size(42, 20);
            this.tbMolNum.TabIndex = 9;
            this.tbMolNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbMolNum.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(183, 293);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(174, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Сохранить в таблицу";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(0, 263);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(162, 27);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Пуск (одиночная молекула)";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbN
            // 
            this.tbN.Location = new System.Drawing.Point(147, 68);
            this.tbN.Name = "tbN";
            this.tbN.Size = new System.Drawing.Size(60, 20);
            this.tbN.TabIndex = 12;
            this.tbN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblProgBar
            // 
            this.lblProgBar.AutoSize = true;
            this.lblProgBar.Location = new System.Drawing.Point(1, 293);
            this.lblProgBar.Name = "lblProgBar";
            this.lblProgBar.Size = new System.Drawing.Size(59, 13);
            this.lblProgBar.TabIndex = 14;
            this.lblProgBar.Text = "Прогресс:";
            this.lblProgBar.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Surface area:";
            // 
            // tbSurfArea
            // 
            this.tbSurfArea.Location = new System.Drawing.Point(89, 183);
            this.tbSurfArea.Name = "tbSurfArea";
            this.tbSurfArea.Size = new System.Drawing.Size(142, 20);
            this.tbSurfArea.TabIndex = 16;
            this.tbSurfArea.Visible = false;
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
            this.dgvData.Location = new System.Drawing.Point(439, 12);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(476, 301);
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
            this.btnClearTable.Location = new System.Drawing.Point(217, 329);
            this.btnClearTable.Name = "btnClearTable";
            this.btnClearTable.Size = new System.Drawing.Size(109, 25);
            this.btnClearTable.TabIndex = 18;
            this.btnClearTable.Text = "Очистить";
            this.btnClearTable.UseVisualStyleBackColor = true;
            this.btnClearTable.Click += new System.EventHandler(this.btnClearTable_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(808, 325);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 25);
            this.button2.TabIndex = 19;
            this.button2.Text = "Выход";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnStart1
            // 
            this.btnStart1.Location = new System.Drawing.Point(192, 265);
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
            this.label7.Location = new System.Drawing.Point(1, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Число молекул:";
            // 
            // tbA
            // 
            this.tbA.Location = new System.Drawing.Point(89, 45);
            this.tbA.Name = "tbA";
            this.tbA.Size = new System.Drawing.Size(100, 20);
            this.tbA.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Dispersion of R on XY- flat:";
            // 
            // tbsqXYradius
            // 
            this.tbsqXYradius.Location = new System.Drawing.Point(147, 206);
            this.tbsqXYradius.Name = "tbsqXYradius";
            this.tbsqXYradius.Size = new System.Drawing.Size(100, 20);
            this.tbsqXYradius.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Dispersion of R on Z-axis:";
            // 
            // tbsqZradius
            // 
            this.tbsqZradius.Location = new System.Drawing.Point(135, 233);
            this.tbsqZradius.Name = "tbsqZradius";
            this.tbsqZradius.Size = new System.Drawing.Size(100, 20);
            this.tbsqZradius.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(213, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Центр плотности:";
            // 
            // tbDens1
            // 
            this.tbDens1.Location = new System.Drawing.Point(329, 94);
            this.tbDens1.Name = "tbDens1";
            this.tbDens1.Size = new System.Drawing.Size(40, 20);
            this.tbDens1.TabIndex = 29;
            // 
            // tbStep
            // 
            this.tbStep.Location = new System.Drawing.Point(306, 45);
            this.tbStep.Name = "tbStep";
            this.tbStep.Size = new System.Drawing.Size(63, 20);
            this.tbStep.TabIndex = 30;
            this.tbStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(195, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Шаг по координате:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(209, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Плотность полимера:";
            // 
            // tbDensCentre
            // 
            this.tbDensCentre.Location = new System.Drawing.Point(315, 68);
            this.tbDensCentre.Name = "tbDensCentre";
            this.tbDensCentre.Size = new System.Drawing.Size(40, 20);
            this.tbDensCentre.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(237, 130);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "Об. доля полимера:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(237, 160);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "Общая плотность:";
            // 
            // tbDens3
            // 
            this.tbDens3.Location = new System.Drawing.Point(343, 157);
            this.tbDens3.Name = "tbDens3";
            this.tbDens3.Size = new System.Drawing.Size(40, 20);
            this.tbDens3.TabIndex = 36;
            // 
            // tbDens2
            // 
            this.tbDens2.Location = new System.Drawing.Point(350, 127);
            this.tbDens2.Name = "tbDens2";
            this.tbDens2.Size = new System.Drawing.Size(40, 20);
            this.tbDens2.TabIndex = 37;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 396);
            this.Controls.Add(this.tbDens2);
            this.Controls.Add(this.tbDens3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbDensCentre);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbStep);
            this.Controls.Add(this.tbDens1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbsqZradius);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbsqXYradius);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbA);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnStart1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnClearTable);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.tbSurfArea);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblProgBar);
            this.Controls.Add(this.tbN);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbMolNum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbZradius);
            this.Controls.Add(this.tbXYradius);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Microgel Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
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
    }
}

