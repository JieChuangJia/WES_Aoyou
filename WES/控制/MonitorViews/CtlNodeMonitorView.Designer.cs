﻿namespace MonitorViews
{
    partial class CtlNodeMonitorView
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanelAsrs = new System.Windows.Forms.FlowLayoutPanel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.checkBoxAutorefresh = new System.Windows.Forms.CheckBox();
            this.groupBoxCtlSim = new System.Windows.Forms.GroupBox();
            this.comboBoxBarcodeGun = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxBarcode = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonDB2Reset = new System.Windows.Forms.Button();
            this.comboBoxDB2 = new System.Windows.Forms.ComboBox();
            this.textBoxRfidVal = new System.Windows.Forms.TextBox();
            this.textBoxDB2ItemVal = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.buttonRfidSimWrite = new System.Windows.Forms.Button();
            this.buttonDB2SimSet = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxDevList = new System.Windows.Forms.ComboBox();
            this.buttonClearDevCmd = new System.Windows.Forms.Button();
            this.buttonRefreshDevStatus = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxTaskInfo = new System.Windows.Forms.RichTextBox();
            this.dataGridViewDevDB1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewDevDB2 = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panelDevConn = new System.Windows.Forms.Panel();
            this.timerNodeStatus = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel13.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBoxCtlSim.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevDB1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevDB2)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 376F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanelAsrs, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel13, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.60163F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.39838F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1341, 738);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanelAsrs
            // 
            this.flowLayoutPanelAsrs.AutoScroll = true;
            this.flowLayoutPanelAsrs.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowLayoutPanelAsrs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelAsrs.Location = new System.Drawing.Point(380, 4);
            this.flowLayoutPanelAsrs.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanelAsrs.Name = "flowLayoutPanelAsrs";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanelAsrs, 2);
            this.flowLayoutPanelAsrs.Size = new System.Drawing.Size(957, 730);
            this.flowLayoutPanelAsrs.TabIndex = 7;
            this.flowLayoutPanelAsrs.SizeChanged += new System.EventHandler(this.flowLayoutPanelAsrs_SizeChanged);
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(4, 4);
            this.panel13.Margin = new System.Windows.Forms.Padding(4);
            this.panel13.Name = "panel13";
            this.tableLayoutPanel1.SetRowSpan(this.panel13, 2);
            this.panel13.Size = new System.Drawing.Size(368, 730);
            this.panel13.TabIndex = 2;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Margin = new System.Windows.Forms.Padding(4);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(368, 730);
            this.panel14.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1357, 778);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1349, 746);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "立库状态监控";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1349, 746);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "通信监控";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(4, 4);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer2.Panel1.Controls.Add(this.checkBoxAutorefresh);
            this.splitContainer2.Panel1.Controls.Add(this.groupBoxCtlSim);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.comboBoxDevList);
            this.splitContainer2.Panel1.Controls.Add(this.buttonClearDevCmd);
            this.splitContainer2.Panel1.Controls.Add(this.buttonRefreshDevStatus);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer2.Size = new System.Drawing.Size(1341, 738);
            this.splitContainer2.SplitterDistance = 298;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 1;
            // 
            // checkBoxAutorefresh
            // 
            this.checkBoxAutorefresh.AutoSize = true;
            this.checkBoxAutorefresh.Location = new System.Drawing.Point(14, 80);
            this.checkBoxAutorefresh.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAutorefresh.Name = "checkBoxAutorefresh";
            this.checkBoxAutorefresh.Size = new System.Drawing.Size(106, 22);
            this.checkBoxAutorefresh.TabIndex = 9;
            this.checkBoxAutorefresh.Text = "自动刷新";
            this.checkBoxAutorefresh.UseVisualStyleBackColor = true;
            // 
            // groupBoxCtlSim
            // 
            this.groupBoxCtlSim.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCtlSim.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxCtlSim.Controls.Add(this.comboBoxBarcodeGun);
            this.groupBoxCtlSim.Controls.Add(this.label16);
            this.groupBoxCtlSim.Controls.Add(this.textBoxBarcode);
            this.groupBoxCtlSim.Controls.Add(this.label17);
            this.groupBoxCtlSim.Controls.Add(this.buttonDB2Reset);
            this.groupBoxCtlSim.Controls.Add(this.comboBoxDB2);
            this.groupBoxCtlSim.Controls.Add(this.textBoxRfidVal);
            this.groupBoxCtlSim.Controls.Add(this.textBoxDB2ItemVal);
            this.groupBoxCtlSim.Controls.Add(this.label23);
            this.groupBoxCtlSim.Controls.Add(this.buttonRfidSimWrite);
            this.groupBoxCtlSim.Controls.Add(this.buttonDB2SimSet);
            this.groupBoxCtlSim.Controls.Add(this.label7);
            this.groupBoxCtlSim.Controls.Add(this.label25);
            this.groupBoxCtlSim.Location = new System.Drawing.Point(10, 172);
            this.groupBoxCtlSim.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxCtlSim.Name = "groupBoxCtlSim";
            this.groupBoxCtlSim.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxCtlSim.Size = new System.Drawing.Size(284, 363);
            this.groupBoxCtlSim.TabIndex = 8;
            this.groupBoxCtlSim.TabStop = false;
            this.groupBoxCtlSim.Text = "仿真模拟";
            // 
            // comboBoxBarcodeGun
            // 
            this.comboBoxBarcodeGun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBarcodeGun.FormattingEnabled = true;
            this.comboBoxBarcodeGun.Location = new System.Drawing.Point(141, 222);
            this.comboBoxBarcodeGun.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxBarcodeGun.Name = "comboBoxBarcodeGun";
            this.comboBoxBarcodeGun.Size = new System.Drawing.Size(38, 26);
            this.comboBoxBarcodeGun.TabIndex = 12;
            this.comboBoxBarcodeGun.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 226);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 18);
            this.label16.TabIndex = 13;
            this.label16.Text = "条码枪";
            this.label16.Visible = false;
            // 
            // textBoxBarcode
            // 
            this.textBoxBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBarcode.Location = new System.Drawing.Point(96, 261);
            this.textBoxBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxBarcode.Name = "textBoxBarcode";
            this.textBoxBarcode.Size = new System.Drawing.Size(88, 28);
            this.textBoxBarcode.TabIndex = 11;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 274);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 18);
            this.label17.TabIndex = 10;
            this.label17.Text = "条码值";
            // 
            // buttonDB2Reset
            // 
            this.buttonDB2Reset.Location = new System.Drawing.Point(4, 111);
            this.buttonDB2Reset.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDB2Reset.Name = "buttonDB2Reset";
            this.buttonDB2Reset.Size = new System.Drawing.Size(134, 36);
            this.buttonDB2Reset.TabIndex = 7;
            this.buttonDB2Reset.Text = "DB2复位";
            this.buttonDB2Reset.UseVisualStyleBackColor = true;
            this.buttonDB2Reset.Visible = false;
            // 
            // comboBoxDB2
            // 
            this.comboBoxDB2.FormattingEnabled = true;
            this.comboBoxDB2.Location = new System.Drawing.Point(126, 27);
            this.comboBoxDB2.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDB2.Name = "comboBoxDB2";
            this.comboBoxDB2.Size = new System.Drawing.Size(145, 26);
            this.comboBoxDB2.TabIndex = 1;
            // 
            // textBoxRfidVal
            // 
            this.textBoxRfidVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRfidVal.Location = new System.Drawing.Point(96, 170);
            this.textBoxRfidVal.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRfidVal.Name = "textBoxRfidVal";
            this.textBoxRfidVal.Size = new System.Drawing.Size(88, 28);
            this.textBoxRfidVal.TabIndex = 5;
            // 
            // textBoxDB2ItemVal
            // 
            this.textBoxDB2ItemVal.Location = new System.Drawing.Point(128, 74);
            this.textBoxDB2ItemVal.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxDB2ItemVal.Name = "textBoxDB2ItemVal";
            this.textBoxDB2ItemVal.Size = new System.Drawing.Size(144, 28);
            this.textBoxDB2ItemVal.TabIndex = 5;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 32);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(107, 36);
            this.label23.TabIndex = 2;
            this.label23.Text = "DB2索引\r\n（从1开始）";
            // 
            // buttonRfidSimWrite
            // 
            this.buttonRfidSimWrite.Location = new System.Drawing.Point(9, 297);
            this.buttonRfidSimWrite.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRfidSimWrite.Name = "buttonRfidSimWrite";
            this.buttonRfidSimWrite.Size = new System.Drawing.Size(264, 40);
            this.buttonRfidSimWrite.TabIndex = 0;
            this.buttonRfidSimWrite.Text = "模拟写入";
            this.buttonRfidSimWrite.UseVisualStyleBackColor = true;
            this.buttonRfidSimWrite.Click += new System.EventHandler(this.buttonRfidSimWrite_Click);
            // 
            // buttonDB2SimSet
            // 
            this.buttonDB2SimSet.Location = new System.Drawing.Point(147, 111);
            this.buttonDB2SimSet.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDB2SimSet.Name = "buttonDB2SimSet";
            this.buttonDB2SimSet.Size = new System.Drawing.Size(126, 36);
            this.buttonDB2SimSet.TabIndex = 0;
            this.buttonDB2SimSet.Text = "模拟写入";
            this.buttonDB2SimSet.UseVisualStyleBackColor = true;
            this.buttonDB2SimSet.Click += new System.EventHandler(this.buttonDB2SimSet_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 183);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 2;
            this.label7.Text = "RFID数值";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(14, 84);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(44, 18);
            this.label25.TabIndex = 2;
            this.label25.Text = "数值";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 6;
            this.label6.Text = "选择工位";
            // 
            // comboBoxDevList
            // 
            this.comboBoxDevList.FormattingEnabled = true;
            this.comboBoxDevList.Location = new System.Drawing.Point(8, 40);
            this.comboBoxDevList.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDevList.Name = "comboBoxDevList";
            this.comboBoxDevList.Size = new System.Drawing.Size(276, 26);
            this.comboBoxDevList.TabIndex = 5;
            // 
            // buttonClearDevCmd
            // 
            this.buttonClearDevCmd.Location = new System.Drawing.Point(150, 112);
            this.buttonClearDevCmd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearDevCmd.Name = "buttonClearDevCmd";
            this.buttonClearDevCmd.Size = new System.Drawing.Size(135, 40);
            this.buttonClearDevCmd.TabIndex = 3;
            this.buttonClearDevCmd.Text = "复位";
            this.buttonClearDevCmd.UseVisualStyleBackColor = true;
            this.buttonClearDevCmd.Visible = false;
            // 
            // buttonRefreshDevStatus
            // 
            this.buttonRefreshDevStatus.Location = new System.Drawing.Point(10, 112);
            this.buttonRefreshDevStatus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRefreshDevStatus.Name = "buttonRefreshDevStatus";
            this.buttonRefreshDevStatus.Size = new System.Drawing.Size(130, 40);
            this.buttonRefreshDevStatus.TabIndex = 4;
            this.buttonRefreshDevStatus.Text = "手动刷新";
            this.buttonRefreshDevStatus.UseVisualStyleBackColor = true;
            this.buttonRefreshDevStatus.Click += new System.EventHandler(this.buttonRefreshDevStatus_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 297F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.label24, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.dataGridViewDevDB1, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.dataGridViewDevDB2, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label10, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.panelDevConn, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1037, 738);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Orange;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point(4, 0);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(289, 34);
            this.label24.TabIndex = 10;
            this.label24.Text = "设备通信状态";
            // 
            // groupBox1
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.richTextBoxTaskInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(301, 602);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(732, 132);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "流程详细";
            // 
            // richTextBoxTaskInfo
            // 
            this.richTextBoxTaskInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxTaskInfo.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxTaskInfo.Location = new System.Drawing.Point(4, 25);
            this.richTextBoxTaskInfo.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxTaskInfo.Name = "richTextBoxTaskInfo";
            this.richTextBoxTaskInfo.Size = new System.Drawing.Size(724, 103);
            this.richTextBoxTaskInfo.TabIndex = 0;
            this.richTextBoxTaskInfo.Text = "";
            // 
            // dataGridViewDevDB1
            // 
            this.dataGridViewDevDB1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewDevDB1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDevDB1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDevDB1.Location = new System.Drawing.Point(301, 38);
            this.dataGridViewDevDB1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewDevDB1.Name = "dataGridViewDevDB1";
            this.dataGridViewDevDB1.RowTemplate.Height = 23;
            this.dataGridViewDevDB1.Size = new System.Drawing.Size(362, 556);
            this.dataGridViewDevDB1.TabIndex = 3;
            // 
            // dataGridViewDevDB2
            // 
            this.dataGridViewDevDB2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDevDB2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDevDB2.Location = new System.Drawing.Point(671, 38);
            this.dataGridViewDevDB2.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewDevDB2.Name = "dataGridViewDevDB2";
            this.dataGridViewDevDB2.RowTemplate.Height = 23;
            this.dataGridViewDevDB2.Size = new System.Drawing.Size(362, 556);
            this.dataGridViewDevDB2.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Orange;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(301, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(362, 34);
            this.label8.TabIndex = 5;
            this.label8.Text = "DB1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Orange;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(671, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(362, 34);
            this.label10.TabIndex = 6;
            this.label10.Text = "DB2";
            // 
            // panelDevConn
            // 
            this.panelDevConn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDevConn.Location = new System.Drawing.Point(3, 37);
            this.panelDevConn.Name = "panelDevConn";
            this.tableLayoutPanel3.SetRowSpan(this.panelDevConn, 2);
            this.panelDevConn.Size = new System.Drawing.Size(291, 698);
            this.panelDevConn.TabIndex = 11;
            // 
            // timerNodeStatus
            // 
            this.timerNodeStatus.Interval = 200;
            this.timerNodeStatus.Tick += new System.EventHandler(this.timerNodeStatus_Tick);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1365, 786);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // CtlNodeMonitorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1365, 786);
            this.Controls.Add(this.tableLayoutPanel6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CtlNodeMonitorView";
            this.Text = "ProcessMonitorView";
            this.Load += new System.EventHandler(this.ProcessMonitorView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBoxCtlSim.ResumeLayout(false);
            this.groupBoxCtlSim.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevDB1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevDB2)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox checkBoxAutorefresh;
        private System.Windows.Forms.GroupBox groupBoxCtlSim;
        private System.Windows.Forms.ComboBox comboBoxBarcodeGun;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxBarcode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonDB2Reset;
        private System.Windows.Forms.ComboBox comboBoxDB2;
        private System.Windows.Forms.TextBox textBoxRfidVal;
        private System.Windows.Forms.TextBox textBoxDB2ItemVal;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button buttonRfidSimWrite;
        private System.Windows.Forms.Button buttonDB2SimSet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxDevList;
        private System.Windows.Forms.Button buttonClearDevCmd;
        private System.Windows.Forms.Button buttonRefreshDevStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dataGridViewDevDB1;
        private System.Windows.Forms.DataGridView dataGridViewDevDB2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer timerNodeStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxTaskInfo;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelAsrs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Panel panelDevConn;
    }
}