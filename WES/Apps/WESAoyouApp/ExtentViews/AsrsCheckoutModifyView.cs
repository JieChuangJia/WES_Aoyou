using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AsrsModel;
using ModuleCrossPnP;
using AsrsInterface;
namespace ExtentViews
{
    public partial class AsrsCheckoutModifyView : BaseChildView
    {
        public delegate void DlgtQueryCells();
        private IAsrsManageToCtl asrsResMana = null;
        public AsrsControl.AsrsCtlPresenter AsrsPresenter { get; set; }
        public AsrsCheckoutModifyView(string captionText)
            : base(captionText)
        {
            InitializeComponent();
            this.Text = captionText;
            this.dateTimePicker1.Value = System.DateTime.Now - (new TimeSpan(7, 0, 0, 0));
        }
       
        public void SetAsrsResManage(IAsrsManageToCtl asrsResManage)
        {
            this.asrsResMana = asrsResManage;
        }
      
        #region UI事件

        /// <summary>
        /// 库位查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            QueryAsrsCells();
        }

        private void CtlTaskView_Load(object sender, EventArgs e)
        {
            this.textBoxPrivilege.Text = "2000";
            this.comboBox1.Items.AddRange(new string[] { "C1库房", "C2库房", "C3库房" });
            this.comboBox1.SelectedIndex = 0;

            this.comboBox3.Items.Add("所有");
            this.comboBox3.Items.Add(AsrsModel.EnumGSTaskStatus.锁定.ToString());
            this.comboBox3.Items.Add(AsrsModel.EnumGSTaskStatus.完成.ToString());
            this.comboBox3.SelectedIndex = 0;

            this.comboBox2.Items.AddRange(new string[] { "OCV后常温静置" });
            this.comboBox2.SelectedIndex = 0;
            this.comboBox2.Enabled = false;

            this.toolTip1.SetToolTip(this.textBoxPrivilege,"请输入0~1000的数值，数值越大，优先级越高");
            OnRefreshBatteryCata();
          
        }
        private void btnDelTask_Click(object sender, EventArgs e)
        {
            OnDelTask();
        }
        private void OnDelTask()
        {
            if(parentPNP.RoleID>2)
            {
                MessageBox.Show("没有足够的权限，请切换到管理员用户");
                return;
            }
            List<string> taskIds = new List<string>();
            foreach(DataGridViewRow dr in dataGridView1.SelectedRows)
            {
                taskIds.Add(dr.Cells["任务ID"].Value.ToString());
            }
           
        }
        #endregion   

        private void buttonModifyPri_Click(object sender, EventArgs e)
        {
            try
            {
                int pri = 0;
                if (!int.TryParse(this.textBoxPrivilege.Text, out pri))
                {
                    MessageBox.Show("请输入正确的优先级值0~1000,值越大优先级越高!");
                    return;
                }
                if (pri < 0 || pri > 1000)
                {
                    MessageBox.Show("请输入正确的优先级值0~1000,值越大优先级越高!");
                    return;
                }
                foreach (DataGridViewRow dr in this.dataGridView1.SelectedRows)
                {
                    string taskID = dr.Cells["任务ID"].Value.ToString();
                    
                
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()) ;
            }
           
        }
        private void AsynQueryAsrsCells()
        {
            if (this.dataGridView1.InvokeRequired)
            {
                DlgtQueryCells dlgt = new DlgtQueryCells(AsynQueryAsrsCells);
                this.Invoke(dlgt, null);
            }
            else
            {
                try
                {

                    CtlDBAccess.BLL.ControlTaskBll ctlTaskBll = new CtlDBAccess.BLL.ControlTaskBll();
                    string logicArea = "OCV常温区";
                    string batCata = this.cbxBatteryCata.Text;
                    DateTime stTime = this.dateTimePicker1.Value;
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[] { new DataColumn("库房"), new DataColumn("库位"), new DataColumn("出库任务ID"), new DataColumn("库位锁定状态"), new DataColumn("托盘码"), new DataColumn("入库时间"), new DataColumn("在库时间(小时)"), new DataColumn("任务优先级") });
                    string reStr = "";
                    //查询库位
                    string houseName = this.comboBox1.Text;
                    string cellTaskStat = this.comboBox3.Text;
                    Dictionary<string, GSMemTempModel> asrsStatDic = new Dictionary<string, GSMemTempModel>();
                    if (!this.asrsResMana.GetAllGsModel(ref asrsStatDic, ref reStr))
                    {
                        Console.WriteLine(string.Format("{0} 获取货位状态失败", houseName));
                        return;
                    }

                    foreach (string strKey in asrsStatDic.Keys)
                    {
                        GSMemTempModel gs = asrsStatDic[strKey];
                        if (gs.GSTaskStatus != cellTaskStat && cellTaskStat != "所有")
                        {
                            continue;
                        }
                        if (gs.GSStatus != AsrsModel.EnumCellStatus.满位.ToString())
                        {
                            continue;
                        }
                        if (gs.InHouseDate == null)
                        {
                            continue;
                        }
                        if (gs.InHouseDate > stTime)
                        {
                            continue;
                        }
                        if (gs.StoreAreaName != logicArea)
                        {
                            continue;
                        }
                        AsrsModel.CellCoordModel cell = new CellCoordModel(0, 0, 0);
                        if (!cell.Parse(gs.GSPos))
                        {
                            continue;
                        }
                        List<string> storePallets = new List<string>();
                        if (!this.asrsResMana.GetStockDetail(houseName, cell, ref storePallets))
                        {
                            continue;
                        }
                        if (storePallets.Count() < 1 || storePallets[0].Length < 4)
                        {
                            continue;
                        }
                        if (batCata != "所有" && (storePallets[0].Substring(0, 4).ToUpper() != batCata.ToUpper()))
                        {
                            continue;
                        }
                        string palletIDS = "";
                        for (int i = 0; i < storePallets.Count(); i++)
                        {
                            if (i != storePallets.Count() - 1)
                            {
                                palletIDS = palletIDS + storePallets[i] + ",";
                            }
                            else
                            {
                                palletIDS = palletIDS + storePallets[i];
                            }
                        }
                        DataRow dr = dt.Rows.Add();
                        dr["库房"] = houseName;
                        dr["库位"] = gs.GSPos;
                        string strWhere = string.Format(" TaskType={0} and (TaskStatus = '待执行' or TaskStatus='执行中') and tag1='{1}' and tag2='{2}'", (int)SysCfg.EnumAsrsTaskType.产品出库, houseName, gs.GSPos);
                        List<CtlDBAccess.Model.ControlTaskModel> runningTasks = ctlTaskBll.GetModelList(strWhere);
                        if (runningTasks != null && runningTasks.Count > 0)
                        {
                            dr["出库任务ID"] = runningTasks[0].TaskID;
                            dr["任务优先级"] = runningTasks[0].tag4;
                        }

                        dr["库位锁定状态"] = gs.GSTaskStatus;
                        dr["托盘码"] = palletIDS;
                        dr["入库时间"] = ((DateTime)gs.InHouseDate).ToString("yyyy-MM-dd HH:mm:ss");
                        TimeSpan ts = System.DateTime.Now - (DateTime)gs.InHouseDate;
                        dr["在库时间(小时)"] = ts.TotalHours.ToString("0.00");

                        //CellCoordModel cell = new CellCoordModel()
                        // asrsResMana.GetStockDetail(houseName,)
                        //gs.GSPos
                    }

                    //显示
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dt;
                    bindingNavigator1.BindingSource = bs;
                    dataGridView1.DataSource = bs;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    this.dataGridView1.Columns["入库时间"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                    this.dataGridView1.Sort(this.dataGridView1.Columns["入库时间"], ListSortDirection.Ascending);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    this.picWaitting.Visible = false;
                }
            }
            
        }
        private void QueryAsrsCells()
        {
            this.picWaitting.Visible = true;
            DlgtQueryCells dlgt = new DlgtQueryCells(AsynQueryAsrsCells);
            dlgt.BeginInvoke(null, dlgt);
        }
        private void OnRefreshBatteryCata()
        {
            try
            {
                this.cbxBatteryCata.Items.Clear();
               
                MesDBAccess.BLL.BatteryCataBll batteryCataBll = new MesDBAccess.BLL.BatteryCataBll();
                List<MesDBAccess.Model.BatteryCataModel> batteryCatas = batteryCataBll.GetModelList("");
                foreach (MesDBAccess.Model.BatteryCataModel m in batteryCatas)
                {
                    this.cbxBatteryCata.Items.Add(m.batteryCataCode);
                }
                if (this.cbxBatteryCata.Items.Count > 0)
                {
                    this.cbxBatteryCata.SelectedIndex = 0;
                }
                this.cbxBatteryCata.Items.Add("所有");
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }
           
        }
        private void btnRefreshBatCata_Click(object sender, EventArgs e)
        {
            OnRefreshBatteryCata();
        }

        private void buttonAsrsCheckout_Click(object sender, EventArgs e)
        {
            GenerateAsrsCheckoutTask();
        }
        private void GenerateAsrsCheckoutTask()
        {
            try
            {
                BindingSource bs = this.dataGridView1.DataSource as BindingSource;
                DataTable dt = bs.DataSource as DataTable;
                if(dt == null)
                {
                    return;
                }
                int pri=int.Parse(this.textBoxPrivilege.Text);
                foreach(DataRow dr in dt.Rows)
                {
                    string houseName = dr["库房"].ToString();
                    string strCell = dr["库位"].ToString();
                    AsrsControl.AsrsCtlModel asrsCtl = AsrsPresenter.GetAsrsCtlByName(houseName);
                    AsrsModel.CellCoordModel cell = new CellCoordModel(0,0,0);
                    if(!cell.Parse(strCell))
                    {
                        continue;
                    }
                    EnumCellStatus cellStoreStat = EnumCellStatus.空闲;
                    EnumGSTaskStatus cellTaskStat = EnumGSTaskStatus.完成;
                    if(!asrsResMana.GetCellStatus(houseName, cell, ref cellStoreStat, ref cellTaskStat))
                    {
                        Console.WriteLine("获取货位{0}:{1}状态失败", houseName, strCell);
                        continue;
                    }
                    if(cellStoreStat != EnumCellStatus.满位 || cellTaskStat == EnumGSTaskStatus.锁定)
                    {
                        continue;
                    }
                    CtlDBAccess.Model.ControlTaskModel asrsTask = null;
                    if(!asrsCtl.GenerateOutputTask(cell, null, SysCfg.EnumAsrsTaskType.产品出库, false, ref asrsTask,null,pri))
                    {
                        logRecorder.AddDebugLog(this.Text,string.Format("生成{0}:{1}出库任务失败", houseName, strCell));
                    }
                    else
                    {
                        logRecorder.AddDebugLog(this.Text, string.Format("生成{0}:{1}产品出库任务成功", houseName, strCell));
                    }
                    dr["出库任务ID"] = asrsTask.TaskID;

                }
                this.dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void btnSelALl_Click(object sender, EventArgs e)
        {
            this.dataGridView1.SelectAll();
        }
        public override bool RoleEnabled()
        {
            if(parentPNP.RoleID>2)
            {
                return false;
            }
            return true;
        }
    }
}
