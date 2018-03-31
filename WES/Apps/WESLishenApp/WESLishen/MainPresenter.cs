using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
namespace WESLishen
{
    public class MainPresenter
    {
        #region 数据
        private IMainView view = null;
        private PrcsCtlModelsLishen.PrsCtlnodeManage prsNodeManager = null; //流水线控制节点管理对象
        private AsrsControl.AsrsCtlPresenter asrsPresenter = null; //立库控制管理对象
        private CtlManage.CtlNodeManage ctlNodeManager = null; // 总的控制节点管理对象
        private CtlManage.CommDevManage devCommManager = null; //通信设备管理对象
        private FlowCtlBaseModel.MesAccWrapper mesAcc = null;
        private AsrsInterface.IAsrsManageToCtl asrsResManage = null; 
        #endregion
        #region 公有方法
        public AsrsControl.AsrsCtlPresenter AsrsPresenter { get { return asrsPresenter; } }
        public CtlManage.CtlNodeManage CtlNodeManager { get { return ctlNodeManager; } }
        public CtlManage.CommDevManage DevCommManager { get { return devCommManager; } }
        public MainPresenter(IMainView view)
        {
            this.view = view;
            devCommManager = new CtlManage.CommDevManage();
            prsNodeManager = new PrcsCtlModelsLishen.PrsCtlnodeManage();
            asrsPresenter = new AsrsControl.AsrsCtlPresenter();
            ctlNodeManager = new CtlManage.CtlNodeManage();
        }
        public AsrsInterface.IAsrsCtlToManage GetAsrsCtlInterfaceObj()
        {
            return asrsPresenter;
        }
       public void SetAsrsResManage(AsrsInterface.IAsrsManageToCtl asrsRes)
        {
            asrsPresenter.SetAsrsResManage(asrsRes);
            this.asrsResManage = asrsRes;
            PrcsCtlModelsLishen.NodeSwitchInput nodeSwitch = ctlNodeManager.GetNodeByID("4001") as PrcsCtlModelsLishen.NodeSwitchInput;
            nodeSwitch.AsrsResManage = asrsResManage;
            nodeSwitch.AsrsCtl = asrsPresenter.AsrsCtls[0];
            nodeSwitch.AsrsPort = ctlNodeManager.GetNodeByID("2009") as AsrsControl.AsrsPortalModel;
        }
        /// <summary>
        /// 系统控制初始化
        /// </summary>
        /// <returns></returns>
        public bool SysCtlInit()
        {
            try
            {
                mesAcc = new FlowCtlBaseModel.MesAccWrapper();
               
                ctlNodeManager.DevCommManager = devCommManager;
                // 1加载配置文件
                string reStr = "";
                XElement root = null;
                SysCfg.SysCfgModel.cfgFilefullPath = AppDomain.CurrentDomain.BaseDirectory + @"\data\LishenCfg.xml";
                if (!SysCfg.SysCfgModel.LoadCfg(ref root, ref reStr))
                {
                    Console.WriteLine("系统配置解析错误,{0}", reStr);
                    return false;
                }

                //2 初始化通信模块
                XElement commDevXERoot = root.Element("CommDevCfg");
                if (!devCommManager.ParseCommdev(commDevXERoot, ref reStr))
                {
                    Console.WriteLine("通信设备配置参数解析设备:" + reStr);
                    return false;
                }
                devCommManager.CommDevConnect();
                //3 初始化立库控制系统

                XElement asrsRoot = root.Element("AsrsNodes");
                if(!asrsPresenter.CtlInit(asrsRoot,ref reStr))
                {
                    Console.WriteLine("立库控制系统初始化失败:"+reStr);
                    return false;
                }
                asrsPresenter.dlgtAsrsExtParmas = AsrsTaskExtParams;
                foreach(AsrsControl.AsrsCtlModel asrsCtl in asrsPresenter.AsrsCtls)
                {
                    asrsCtl.dlgtAsrsOutportBusiness = AsrsOutportBusiness;
                }
                //4 初始化流水线控制系统
                XElement prcsNodeRoot = root.Element("CtlNodes");
                if(!prsNodeManager.CtlInit(prcsNodeRoot,ref reStr))
                {
                    Console.WriteLine("流水线系统初始化失败:" + reStr);
                    return false;
                }
                //5 注册控制节点
                ctlNodeManager.AddCtlNodeRange(prsNodeManager.GetAllCtlNodes());
                ctlNodeManager.AddCtlNodeRange(asrsPresenter.GetAllCtlNodes());
                foreach (FlowCtlBaseModel.CtlNodeBaseModel node in ctlNodeManager.MonitorNodeList)
                {
                    node.MesAcc = mesAcc;
                }
                foreach(AsrsControl.AsrsCtlModel asrsCtl in asrsPresenter.AsrsCtls)
                {
                    asrsCtl.MesAcc = mesAcc;
                    asrsCtl.dlgtGetLogicArea = AsrsAreaToCheckin;
                    asrsCtl.dlgtUpdateStep = UpdateStepAfterCheckin;
                }
               

                //6 通信设备分配
                ctlNodeManager.AllocateCommdev();

                //建立节点路径
                ctlNodeManager.BuildNodePath();

                //7 线程分配
                XElement threadRoot = root.Element("ThreadAlloc");
                if(!ctlNodeManager.ParseTheadNodes(threadRoot,ref reStr))
                {
                    Console.WriteLine("分配线程时出现错误");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("控制系统初始化错误:"+ex.ToString());
                return false;
                
            }

        }
        public void StartRun()
        {
            this.ctlNodeManager.StartNodeRun();
            asrsPresenter.StartRun();
        }
        public void PauseRun()
        {
            this.ctlNodeManager.PauseNodeRun();
            asrsPresenter.PauseRun();
        }
        public void ExitSystem()
        {
            try
            {
                this.ctlNodeManager.ExitRun();
                asrsPresenter.ExitRun();
            }
            catch (Exception ex)
            {
                Console.WriteLine("退出时发生异常：" + ex.ToString());
              
            }
            
        }
        public void SetLogRecorder(LogInterface.ILogRecorder logRecorder)
        {
            asrsPresenter.SetLogRecorder(logRecorder);
            prsNodeManager.SetLogRecorder(logRecorder);
        }
        public  List<string> GetLogsrcList()
        {
            List<string> logNodes = ctlNodeManager.GetMonitorNodeNames();
            return logNodes;
        }
        #endregion
        #region 立库逻辑扩展
        private bool AsrsOutportBusiness(AsrsControl.AsrsPortalModel port, ref string reStr)
        {
            try
            {
                MesDBAccess.BLL.palletBll palletDBll = new MesDBAccess.BLL.palletBll();
                if(port.PortCata == 1)
                {
                    return true;
                }
               
                if (port.BindedTaskOutput == SysCfg.EnumAsrsTaskType.空筐出库)
                {
                    if (port.Db2Vals[0] == 1)
                    {
                        port.Db1ValsToSnd[0] = 1;
                    }
                    return true;
                }
                else if (port.BindedTaskOutput == SysCfg.EnumAsrsTaskType.产品出库)
                {
                    if (port.Db2Vals[1] == 1)
                    {
                        port.Db1ValsToSnd[0] = 1;
                    }
                }
                else
                {
                    return true;
                }
               
                if (port.Db1ValsToSnd[0] == 2)
                {
                    return true;
                }
                if(port.Db2Vals[1] != 2)
                {
                    return true;
                }
                Int16 palletCata = port.Db2Vals[2];
                if(palletCata <1 || palletCata>3)
                {
                    return true;
                }
   
                AsrsControl.AsrsCtlModel asrsCtl = port.AsrsCtl;
                string houseName = asrsCtl.HouseName;
                AsrsInterface.IAsrsManageToCtl asrsResManage = port.AsrsCtl.AsrsResManage;
                //遍历所有库位，判断材料类别，按照先入先出规则，匹配出库的货位。
                Dictionary<string, AsrsModel.GSMemTempModel> asrsStatDic = new Dictionary<string, AsrsModel.GSMemTempModel>();
                if (!asrsResManage.GetAllGsModel(ref asrsStatDic, ref reStr))
                {
                    Console.WriteLine(string.Format("{0} 获取货位状态失败", houseName));
                    return false;
                }
                List<AsrsModel.GSMemTempModel> validCells = new List<AsrsModel.GSMemTempModel>();
                int r = 1, c = 1, L = 1;
                for (r = 1; r < asrsCtl.AsrsRow + 1; r++)
                {
                    for (c = 1; c < asrsCtl.AsrsCol + 1; c++)
                    {
                        for (L = 1; L < asrsCtl.AsrsLayer + 1; L++)
                        {
                            string strKey = string.Format("{0}:{1}-{2}-{3}", houseName, r, c, L);
                            AsrsModel.GSMemTempModel cellStat = null;
                            if (!asrsStatDic.Keys.Contains(strKey))
                            {
                                continue;
                            }
                            cellStat = asrsStatDic[strKey];
                            if ( (!cellStat.GSEnabled) || (cellStat.GSTaskStatus == AsrsModel.EnumGSTaskStatus.锁定.ToString()) || (cellStat.GSStatus != AsrsModel.EnumCellStatus.满位.ToString()))
                            {
                                // reStr = string.Format("货位{0}-{1}-{2}禁用,无法生成出库任务", cell.Row, cell.Col, cell.Layer);
                                continue;
                            }
                            AsrsModel.CellCoordModel cell = new AsrsModel.CellCoordModel(r, c, L);
                            List<string> storGoods = new List<string>();
                            if (!asrsResManage.GetStockDetail(houseName, cell, ref storGoods))
                            {
                                continue;
                            }
                            if(storGoods.Count()<1)
                            {
                                continue;
                            }
                            MesDBAccess.Model.palletModel pallet = palletDBll.GetModel(storGoods[0]);
                            if (pallet.palletCata == palletCata.ToString())
                            {
                                validCells.Add(cellStat);
                            }
                            //if (storGoods[0].Substring(2, 1) == palletCata.ToString())
                            //{
                            //    validCells.Add(cellStat);
                            //}
                        }
                    }
                }
                if(validCells.Count()>0)
                {
                    //排序，按照先入先出
                    AsrsModel.GSMemTempModel firstGS = validCells[0];
                    if (validCells.Count()>1)
                    {
                        for(int i=1;i<validCells.Count();i++)
                        {
                            AsrsModel.GSMemTempModel tempGS = validCells[i];
                            if(tempGS.InHouseDate <firstGS.InHouseDate)
                            {
                                firstGS = tempGS;
                            }
                        }
                    }
                    //生成出库任务
                    string[] strCellArray = firstGS.GSPos.Split(new string[]{"-"},StringSplitOptions.RemoveEmptyEntries);
                    int row = int.Parse(strCellArray[0]);
                    int col = int.Parse(strCellArray[1]);
                    int layer = int.Parse(strCellArray[2]);
                    AsrsModel.CellCoordModel cell = new AsrsModel.CellCoordModel(row, col, layer);
                   if(asrsCtl.GenerateOutputTask(cell, port.BindedTaskOutput, true,port.PortSeq, ref reStr,new List<short>{ palletCata}))
                    {
                        port.Db1ValsToSnd[0] = 2;
                    }
                    else
                    {
                        Console.WriteLine("生成任务{0}失败,{1}", port.BindedTaskOutput.ToString(),reStr);
                    }
               
                }
                return true;
            }
            catch (Exception ex)
            {
                reStr = ex.ToString();
                return false;
            }
        }
        private bool AsrsTaskExtParams(List<string> cellStoreGoods,ref List<short> extParams,ref string reStr)
        {
            try
            {
                extParams = new List<short>();
                if(cellStoreGoods == null || cellStoreGoods.Count()<1)
                {
                    return true;
                }
                string palletID = cellStoreGoods[0];
                MesDBAccess.BLL.palletBll palletDBAcc = new MesDBAccess.BLL.palletBll();
                MesDBAccess.Model.palletModel palletM = palletDBAcc.GetModel(palletID);
                if(palletM != null)
                {
                    extParams.Add(short.Parse(palletM.palletCata));
                }
                return true;
            }
            catch (Exception ex)
            {
                reStr = ex.ToString();
                return false;
            }
        }
        private string AsrsAreaToCheckin(string palletID,AsrsControl.AsrsCtlModel asrsCtl,int step)
        {
            string area = "其它";
            if(step== 0)
            {
                //不限库区，
                string[] logicAreas = new string[]{"正极材料区","负极材料区","空筐区"};
                foreach(string strArea in logicAreas)
                {
                    int validNum = 0;
                    string reStr="";
                    if(this.asrsResManage.GetHouseAreaLeftGs(asrsCtl.HouseName, strArea, ref validNum, reStr))
                    {
                        if(validNum>0)
                        {
                            area = strArea;
                            break;
                        }
                    }
                }
                return area;
                
            }
            else
            {
                area = SysCfg.SysCfgModel.asrsStepCfg.GetAsrsArea(step);
            }
            return area;
        }
        private bool UpdateStepAfterCheckin(string palletID,AsrsControl.AsrsCtlModel asrsCtl, int curStep)
        {
            return true;
     
        }
        #endregion
    }
}
