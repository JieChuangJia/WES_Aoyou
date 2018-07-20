using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Linq;

using LogInterface;
using ModuleCrossPnP;
using LogManage;
using ProductRecordView;
using AsrsControl;
using ASRSStorManage.View;
using MonitorViews;

namespace WESAoyouMonitor
{
    public partial class MonitorMainForm : Form, ILogDisp, IParentModule
    {
        #region 数据
        private string appTitle = "捷创嘉锂电WES监控终端";
        private string version = "系统版本:1.1.8  2018-7-6";
        private int roleID = 3;
        private string userName = "操作员";
        const int CLOSE_SIZE = 10;
        int iconWidth = 16;
        int iconHeight = 16;

        //子窗体相关
       
        private List<BaseChildView> childViews = null;
        private List<string> childList = null;
        private LogView logView = null;
        // private RecordView recordView = null;
        private ProduceTraceView palletTraceView = null;
        private AsrsMonitorView asrsMonitorView = null;
        private StorageMainView storageView = null;
        private CtlNodeMonitorView nodeMonitorView = null;

        #endregion
        public string[] ExtLogSrc { get; set; }
        public MonitorMainForm()
        {
            this.IsMdiContainer = true;
            InitializeComponent();
            this.menuStrip1.BackColor = Color.FromArgb(0x2D, 0x5B, 0x86);
            this.panel2.BackColor = Color.FromArgb(0x2D, 0x5B, 0x86);
            this.menuStrip1.ForeColor = Color.FromArgb(255, 255, 255);
            //  this.label5.ForeColor = Color.FromArgb(0x2D, 0x5B, 0x86);
            this.Text = appTitle;
        }
        #region 接口实现
        public string CurUsername { get { return this.userName; } }
        public int RoleID { get { return this.roleID; } }
        private delegate void DelegateDispLog(LogModel log);//委托，显示日志
        public void DispLog(LogModel log)
        {
            if (this.richTextBoxLog.InvokeRequired)
            {
                DelegateDispLog delegateLog = new DelegateDispLog(DispLog);
                this.Invoke(delegateLog, new object[] { log });
            }
            else
            {
                if (this.richTextBoxLog.Text.Count() > 10000)
                {
                    this.richTextBoxLog.Text = "";
                }
                this.richTextBoxLog.Text += (string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}]{1},{2}", log.LogTime, log.LogSource, log.LogContent) + "\r\n");
            }

        }
        public void AttachModuleView(System.Windows.Forms.Form childView)
        {
            TabPage tabPage = null;
            if (this.childList.Contains(childView.Text))
            {
                tabPage = this.MainTabControl.TabPages[childView.Text];
                this.MainTabControl.SelectedTab = tabPage;
                return;
            }

            this.MainTabControl.TabPages.Add(childView.Text, childView.Text);
            tabPage = this.MainTabControl.TabPages[childView.Text];
            tabPage.Controls.Clear();
            this.MainTabControl.SelectedTab = tabPage;
            childView.MdiParent = this;

            tabPage.Controls.Add(childView);
            this.childList.Add(childView.Text);
            childView.Dock = DockStyle.Fill;
            childView.Size = this.panelCenterview.Size;
            childView.Show();

        }
        public void RemoveModuleView(System.Windows.Forms.Form childView)
        {
            TabPage tabPage = null;
            if (this.childList.Contains(childView.Text))
            {
                tabPage = this.MainTabControl.TabPages[childView.Text];
                this.childList.Remove(childView.Text);
                this.MainTabControl.TabPages.Remove(tabPage);

            }
        }
        #endregion
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Text = "";
        }

        private void MonitorMainForm_Click(object sender, EventArgs e)
        {

        }

        private void MonitorMainForm_Load(object sender, EventArgs e)
        {
            Console.SetOut(new WESAoyou.TextBoxWriter(this.richTextBoxLog));
            this.WindowState = FormWindowState.Maximized;
            try
            {
                 #region 数据库配置
                string dbSrc = ConfigurationManager.AppSettings["DBSource"];
                //CtlDBAccess.DBUtility.PubConstant.ConnectionString = string.Format(@"{0}Initial Catalog=ACEcams;User ID=sa;Password=123456;", dbSrc);
                string dbConn1 = string.Format(@"{0}Initial Catalog=AoyouEcams;User ID=sa;Password=123456;", dbSrc);
                CtlDBAccess.DBUtility.DbHelperSQL.SetConnstr(dbConn1);
                string dbConn2 = string.Format(@"{0}Initial Catalog=AoyouLocalMes;User ID=sa;Password=123456;", dbSrc);
                MesDBAccess.DBUtility.DbHelperSQL.SetConnstr(dbConn2);
                AsrsStorDBAcc.DbHelperSQL.SetConnstr(string.Format(@"{0}Initial Catalog=AoyouWMSDB;User ID=sa;Password=123456;", dbSrc));
                #endregion
                childList = new List<string>();
                childViews = new List<BaseChildView>();
                this.labelUser.Text = "当前用户：" + this.userName;
                this.labelVersion.Text = this.version;
                this.MainTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                this.MainTabControl.Padding = new System.Drawing.Point(CLOSE_SIZE, CLOSE_SIZE);
                this.MainTabControl.DrawItem += new DrawItemEventHandler(this.tabControlMain_DrawItem);
                this.MainTabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlMain_MouseDown);
                if (!SysCtlInit())
                {
                    return;
                }
                if (!LoadModules())//加载子模块
                {
                    MessageBox.Show("子模块加载错误");

                    return;
                }
                if(!InitWESMonitor())
                {
                    MessageBox.Show("WES监控初始化失败");
                    return;
                }
               
            }
            catch (Exception ex)
            {
                
               Console.WriteLine(ex.ToString());
            }

        }
        private bool LoadModules()
        {
            logView = new LogView("日志");
            childViews.Add(logView);
            logView.SetParent(this);
            logView.RegisterMenus(this.menuStrip1, "日志查询");
            logView.SetLogDispInterface(this);

            nodeMonitorView = new CtlNodeMonitorView("流程监控");
            childViews.Add(nodeMonitorView);
            nodeMonitorView.SetParent(this);
            nodeMonitorView.RegisterMenus(this.menuStrip1, "流程监控");
            nodeMonitorView.SetLoginterface(logView.GetLogrecorder());

            palletTraceView = new ProduceTraceView("托盘追溯");
            childViews.Add(palletTraceView);
            palletTraceView.SetParent(this);
            palletTraceView.RegisterMenus(this.menuStrip1, "托盘追溯");
            palletTraceView.SetLoginterface(logView.GetLogrecorder());

            asrsMonitorView = new AsrsMonitorView("立库监控");
            childViews.Add(asrsMonitorView);
            asrsMonitorView.SetParent(this);
            asrsMonitorView.RegisterMenus(this.menuStrip1, "立库控制");
            asrsMonitorView.SetLoginterface(logView.GetLogrecorder());
           

            storageView = new StorageMainView();
            childViews.Add(storageView);
            storageView.SetParent(this);
            storageView.RegisterMenus(this.menuStrip1, "库存管理");
            storageView.SetLoginterface(logView.GetLogrecorder());
            nodeMonitorView.SetAsrsBatchSetCtl(storageView.BatchSetControl);
           
            AsrsInterface.IAsrsManageToCtl asrsResManage = null;
            string svcAddr = ConfigurationManager.AppSettings["AsrsCtlSvcAddr"];
            AsrsInterface.IAsrsCtlToManage asrsCtl = ChannelFactory<AsrsInterface.IAsrsCtlToManage>.CreateChannel(new BasicHttpBinding(), new EndpointAddress(svcAddr));
            if (asrsCtl == null)
            {
                Console.WriteLine("WCS服务未启动");
                return false;
            }
            // AsrsInterface.IAsrsCtlToManage asrsCtl = presenter.GetAsrsCtlInterfaceObj(); //通过主控wcs服务获取接口对象
            string reStr = "";
            if (!storageView.Init(asrsCtl, ref asrsResManage, ref reStr))
            {
                // logView.GetLogrecorder().AddLog(new LogModel("主模块", "立库管理层模块初始化错误," + reStr, EnumLoglevel.错误));
                Console.WriteLine("立库管理层模块初始化错误," + reStr);
                return false;
            }
            nodeMonitorView.SetAsrsBatchSetCtl(storageView.BatchSetControl);

            foreach (BaseChildView childView in childViews)
            {
                childView.ChangeRoleID(this.roleID);
            }
            AttachModuleView(nodeMonitorView);
            return true;
        }
        private bool InitWESMonitor()
        {
            try
            {
                string svcAddr = ConfigurationManager.AppSettings["MonitorSvcAddr"];
                CtlMonitorInterface.IWESMonitorSvc wesMonitorSvc = ChannelFactory<CtlMonitorInterface.IWESMonitorSvc>.CreateChannel(new BasicHttpBinding(), new EndpointAddress(svcAddr));
                if (wesMonitorSvc == null)
                {
                    Console.WriteLine("WCS服务未启动");
                    return false;
                }
                Console.WriteLine(wesMonitorSvc.hello());
                this.nodeMonitorView.NodeMonitor = wesMonitorSvc;
                this.nodeMonitorView.AsrsMonitor = wesMonitorSvc;
                nodeMonitorView.Init();
                this.nodeMonitorView.InitDevDic(wesMonitorSvc.GetPLCConnStatDic());
                this.nodeMonitorView.DevMonitorView.devCommMonitor = wesMonitorSvc;
                asrsMonitorView.SetAsrsMonitor(wesMonitorSvc);
                asrsMonitorView.Init();
                nodeMonitorView.SetAsrsMonitors(asrsMonitorView.AsrsMonitors);

                 List<string> logSrcs = new List<string>();
                 logSrcs.AddRange(wesMonitorSvc.GetLogSrcList());
                List<string> storLogSrcs = storageView.GetLogsrcList();
                if (storLogSrcs != null)
                {
                    logSrcs.AddRange(logSrcs);
                }
                logView.SetLogsrcList(logSrcs);
                logView.AddLogsrcList(ExtLogSrc);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
        }
        private void tabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {

                Image icon = this.imageList1.Images[0];
                Brush biaocolor = Brushes.Transparent; //选项卡的背景色
                Graphics g = e.Graphics;
                Rectangle r = MainTabControl.GetTabRect(e.Index);
                if (e.Index == this.MainTabControl.SelectedIndex)    //当前选中的Tab页，设置不同的样式以示选中
                {
                    Brush selected_color = Brushes.Wheat; //选中的项的背景色
                    g.FillRectangle(selected_color, r); //改变选项卡标签的背景色
                    string title = MainTabControl.TabPages[e.Index].Text + "  ";

                    g.DrawString(title, this.Font, new SolidBrush(Color.Black), new PointF(r.X, r.Y + 6));//PointF选项卡标题的位置

                    r.Offset(r.Width - iconWidth - 3, 2);
                    g.DrawImage(icon, new Point(r.X + 2, r.Y + 2));//选项卡上的图标的位置 fntTab = new System.Drawing.Font(e.Font, FontStyle.Bold);
                }
                else//非选中的
                {
                    g.FillRectangle(biaocolor, r); //改变选项卡标签的背景色
                    string title = this.MainTabControl.TabPages[e.Index].Text + "  ";

                    g.DrawString(title, this.Font, new SolidBrush(Color.Black), new PointF(r.X, r.Y + 6));//PointF选项卡标题的位置
                    r.Offset(r.Width - iconWidth - 3, 2);
                    g.DrawImage(icon, new Point(r.X + 2, r.Y + 2));//选项卡上的图标的位置
                }
                //Rectangle myTabRect = this.MainTabControl.GetTabRect(e.Index);

                ////先添加TabPage属性   
                //e.Graphics.DrawString(this.MainTabControl.TabPages[e.Index].Text
                //, this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);

                //myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                //myTabRect.Width = CLOSE_SIZE;
                //myTabRect.Height = CLOSE_SIZE;
                ////再画一个矩形框
                //using (Pen p = new Pen(Color.Red))
                //{

                //    //  e.Graphics.DrawRectangle(p, myTabRect);
                //}


                ////画关闭符号
                //using (Pen objpen = new Pen(Color.DarkGray, 1.0f))
                //{
                //    //"\"线
                //    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                //    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                //    e.Graphics.DrawLine(objpen, p1, p2);

                //    //"/"线
                //    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                //    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                //    e.Graphics.DrawLine(objpen, p3, p4);
                //}

                //e.Graphics.Dispose();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
        private void tabControlMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = e.Location;
                Rectangle r = MainTabControl.GetTabRect(this.MainTabControl.SelectedIndex);
                r.Offset(r.Width - iconWidth, 4);
                r.Width = iconWidth;
                r.Height = iconHeight;
                if (this.MainTabControl.SelectedTab.Text == nodeMonitorView.Text)
                {
                    return;
                }
                string tabText = this.MainTabControl.SelectedTab.Text;

                if (r.Contains(p))
                {
                    this.childList.Remove(tabText);
                    this.MainTabControl.TabPages.RemoveAt(this.MainTabControl.SelectedIndex);
                }

                //int x = e.X, y = e.Y;

                ////计算关闭区域   
                //Rectangle myTabRect = this.MainTabControl.GetTabRect(this.MainTabControl.SelectedIndex);

                //myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                //myTabRect.Width = CLOSE_SIZE;
                //myTabRect.Height = CLOSE_SIZE;

                ////如果鼠标在区域内就关闭选项卡   
                //bool isClose = x > myTabRect.X && x < myTabRect.Right
                // && y > myTabRect.Y && y < myTabRect.Bottom;

                //if (isClose == true)
                //{
                //    if (this.MainTabControl.SelectedTab.Text == nodeMonitorView.Text)
                //    {
                //        return;
                //    }
                //    string tabText = this.MainTabControl.SelectedTab.Text;
                //    this.childList.Remove(tabText);
                //    this.MainTabControl.TabPages.Remove(this.MainTabControl.SelectedTab);

                //}
            }
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnChangeRoleID();
        }
        private void OnChangeRoleID()
        {
            try
            {
                NbssECAMS.LoginView2 logView2 = new NbssECAMS.LoginView2();
                if (DialogResult.OK == logView2.ShowDialog())
                {
                    string tempUserName = "";
                    int tempRoleID = logView2.GetLoginRole(ref tempUserName);
                    if (tempRoleID < 1)
                    {
                        return;
                    }
                    this.roleID = tempRoleID;
                    this.userName = tempUserName;
                    this.labelUser.Text = "当前用户：" + this.userName;
                    foreach (BaseChildView childView in childViews)
                    {
                        childView.ChangeRoleID(this.roleID);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool SysCtlInit()
        {
            try
            {
                string reStr = "";
                XElement root = null;
                SysCfg.SysCfgModel.cfgFilefullPath = AppDomain.CurrentDomain.BaseDirectory + @"\data\AoyouCfg.xml";
                if (!SysCfg.SysCfgModel.LoadCfg(ref root, ref reStr))
                {
                    Console.WriteLine("系统配置解析错误,{0}", reStr);
                    return false;
                }

                if (root.Element("sysSet").Element("ExtLogSrc") != null)
                {
                    string logSrcStr = root.Element("sysSet").Element("ExtLogSrc").Value.ToString();
                    ExtLogSrc = logSrcStr.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                }
                SysCfg.SysCfgModel.AsrsHouseList.AddRange(new string[] { "C1库房", "C2库房", "C3库房" });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
               
            }
        }
        private void OnStart()
        {
            
            nodeMonitorView.StartMonitor();
            asrsMonitorView.StartMonitor();
            this.btnPause.Enabled = true;
            this.btnStart.Enabled = false;
            this.label6.Text = "系统已经启动！";
        }
        private void OnStop()
        {
          
            this.nodeMonitorView.StopMonitor();
            this.asrsMonitorView.StopMonitor();
            this.btnPause.Enabled = false;
            this.btnStart.Enabled = true;
            this.label6.Text = "系统已经暂停！";
        }
        protected int PoupAskmes(string info)
        {
            if (DialogResult.OK == MessageBox.Show(info, "提示", MessageBoxButtons.OKCancel))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public bool OnExit()
        {
            if (0 == PoupAskmes("确定要退出系统？"))
            {
                return false;
            }
            // if (presenter.NeedSafeClosing())
            {
                ClosingWaitDlg dlg = new ClosingWaitDlg();
                if (DialogResult.Cancel == dlg.ShowDialog())
                {
                    return false;
                }
            }
            OnStop();
           // this.presenter.ExitSystem();
            System.Environment.Exit(0);
            return true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            OnStart();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            OnStop();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            OnExit();
        }
    }
}
