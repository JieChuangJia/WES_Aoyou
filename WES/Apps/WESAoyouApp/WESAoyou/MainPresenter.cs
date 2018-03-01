﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Data;
using CtlMonitorInterface;
using AsrsModel;
using AsrsInterface;
using AsrsControl;
using AsrsExtctlSvc;
namespace WESAoyou
{
     [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MainPresenter : IWESMonitorSvc
    {
        #region 数据
        private IMainView view = null;
        private PrcsCtlModelsAoyou.PrsCtlnodeManage prsNodeManager = null; //流水线控制节点管理对象
        private AsrsControl.AsrsCtlPresenter asrsPresenter = null; //立库控制管理对象
        private CtlManage.CtlNodeManage ctlNodeManager = null; // 总的控制节点管理对象
        private CtlManage.CommDevManage devCommManager = null; //通信设备管理对象
        private FlowCtlBaseModel.MesAccWrapper mesAcc = null;
        private AsrsInterface.IAsrsManageToCtl asrsResManage = null;
     
       // private HkFenrongSvc hkFenrongSvc = null;
        #endregion
        #region 公有方法
        public string[] ExtLogSrc { get; set; }
        public LogInterface.ILogRecorder logRecorder { get; set; }
        public AsrsControl.AsrsCtlPresenter AsrsPresenter { get { return asrsPresenter; } }
        public CtlManage.CtlNodeManage CtlNodeManager { get { return ctlNodeManager; } }
        public CtlManage.CommDevManage DevCommManager { get { return devCommManager; } }
        public MainPresenter(IMainView view)
        {
            this.view = view;
            devCommManager = new CtlManage.CommDevManage();
            prsNodeManager = new PrcsCtlModelsAoyou.PrsCtlnodeManage();
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
            prsNodeManager.SetAsrsResManage(asrsRes);
            this.asrsResManage = asrsRes;
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
                foreach (AsrsControl.AsrsCtlModel asrsCtl in asrsPresenter.AsrsCtls)
                {
                    asrsCtl.dlgtAsrsOutportBusiness = AsrsOutportBusiness;
                    if(asrsCtl.HouseName == "A1库房" || asrsCtl.HouseName == "A2库房")
                    {
                        asrsCtl.dlgtGetTaskTorun = AsrsGetCheckoutOfGaowen;
                    }
                    if(asrsCtl.HouseName=="B1库房")
                    {
                        asrsCtl.dlgtAsrsOutTaskPost = AsrsOutTaskBusiness;
                    }
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
                }
                //6 通信设备分配
                ctlNodeManager.AllocateCommdev();

                asrsPresenter.DevStatusRestore();
                prsNodeManager.DevStatusRestore();

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
        public bool LoadAsrsExtSvc()
        {
            try
            {
               
                //杭可分容服务部署
                // hkFenrongSvc = new HkFenrongSvc(this.asrsResManage);
                Uri _baseAddress = new Uri("http://localhost:8999/JCJ/AoyouFenrongSvc/");
                EndpointAddress _Address = new EndpointAddress(_baseAddress);
                BasicHttpBinding _Binding = new BasicHttpBinding();
                ContractDescription _Contract = ContractDescription.GetContract(typeof(AsrsExtctlSvc.Interface.IHangkeFenrong));
                ServiceEndpoint endpoint = new ServiceEndpoint(_Contract, _Binding, _Address);
                AsrsExtctlSvc.HkFenrongSvc hkFenrongSvc = new AsrsExtctlSvc.HkFenrongSvc(this.asrsResManage, "B1库房");
                hkFenrongSvc.logRecorder = logRecorder;
               
                //添加终结点ABC
                ServiceHost host = new ServiceHost(hkFenrongSvc, _baseAddress);
                host.Description.Endpoints.Add(endpoint);
                //启用元数据交换
                ServiceMetadataBehavior meta = new ServiceMetadataBehavior();

                meta.HttpGetEnabled = true;
                host.Description.Behaviors.Add(meta);
                host.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("部署分容服务失败:" + ex.ToString());
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
        
        #endregion
        #region IMonitorSvc方法
        
        public string hello()
        {
            return "hello,WES监控服务已经启动";
        }
        public string[] GetLogSrcList()
        {

            //List<string> storLogSrcs = storageView.GetLogsrcList();
            //if(storLogSrcs != null)
            //{
            //    logSrcs.AddRange(logSrcs);
            //}

            //logView.SetLogsrcList(logSrcs);

            List<string> logSrcs = ctlNodeManager.GetMonitorNodeNames();
            logSrcs.AddRange(ExtLogSrc);
            return logSrcs.ToArray();
        }
        public List<string> GetMonitorNodeNames()
        {
            return ctlNodeManager.GetMonitorNodeNames();
        }
       
        public bool GetDevRunningInfo(string nodeName, ref DataTable db1Dt, ref DataTable db2Dt, ref string taskDetail)
        {
            return ctlNodeManager.GetDevRunningInfo(nodeName, ref db1Dt, ref db2Dt, ref taskDetail);
        }
       
        public bool SimSetDB2(string nodeName, int dbItemID, int val)
        {
            return ctlNodeManager.SimSetDB2(nodeName, dbItemID, val);
        }
        
        public void SimSetRFID(string nodeName, string strUID)
        {
            ctlNodeManager.SimSetRFID(nodeName, strUID);
        }
         
        public void SimSetBarcode(string nodeName, string barcode)
        {
            ctlNodeManager.SimSetBarcode(nodeName, barcode);
        }
       
        public IDictionary<string, DevConnStat> GetPLCConnStatDic()
        {
            return devCommManager.GetPLCConnStatDic();
        }

        public string[] GetAllAsrsHousNames()
        {
            List<string> houseNameList = new List<string>();
            foreach(AsrsCtlModel asrsCtl in  asrsPresenter.AsrsCtls)
            {
                houseNameList.Add(asrsCtl.HouseName);
            }
            return houseNameList.ToArray();
        }
        public IDictionary<string, string> GetAllAsrsPortNames()
        {
            IDictionary<string, string> portNameMap = new Dictionary<string, string>();
            foreach(AsrsCtlModel asrsCtl in  asrsPresenter.AsrsCtls)
            {
                foreach(AsrsPortalModel port in asrsCtl.Ports)
                {
                    if(port.PortCata != 2)
                    {
                        portNameMap[port.NodeName]=port.NodeID;
                    }
                }
            }
            return portNameMap;
        }
         public bool GetAsrsStat(string asrsHoseName, ref int errCode, ref string[] status)
        {
            AsrsCtlModel asrsCtl = asrsPresenter.GetAsrsCtlByName(asrsHoseName);
            if(asrsCtl == null)
            {
                return false;
            }
            return asrsCtl.StackDevice.GetRunningStatus(ref errCode, ref status);

        }
         public void SetPortBuffer(string portName,string[] barcodes)
         {

         }
         public void ClearPortBuffer(string portName)
         {

         }
        #endregion
       
        #region 立库逻辑扩展
        private bool AsrsOutportBusiness(AsrsControl.AsrsPortalModel port, ref string reStr)
        {
            try
            {
                
                if(port.PortCata == 2)
                {
                    //出口，无板时，出库完成信号复位
                    if (port.Db2Vals[0] == 2) //无板时，DB1复位
                    {
                        // port.DevCmdReset();
                        port.Db1ValsToSnd[0] = 1;
                        port.Db1ValsToSnd[1] = 0;
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
        private bool AsrsOutTaskBusiness(AsrsControl.AsrsPortalModel outPort, AsrsControl.AsrsTaskParamModel taskParam,ref string reStr)
        {
            try
            {
                outPort.Db1ValsToSnd[1] = (short)taskParam.CellPos1.Row;
                if (!outPort.NodeCmdCommit(true, ref reStr))
                {
                    reStr = string.Format("出库站台{0}状态'出库完成'提交失败", outPort.PortSeq);
                    return false;
                }
                System.Threading.Thread.Sleep(500);
               
                return true;
            }
            catch (Exception ex)
            {
                reStr = ex.ToString();
                return false;
              
            }
            
        }
        public CtlDBAccess.Model.ControlTaskModel AsrsGetCheckoutOfGaowen(AsrsControl.AsrsCtlModel asrsCtl, IAsrsManageToCtl asrsResManage, IList<CtlDBAccess.Model.ControlTaskModel> taskList, SysCfg.EnumAsrsTaskType taskType)
        {
            try
            {
                if (taskList == null)
                {
                    return null;
                }
                //AsrsPortalModel port = null;
                //if(asrsCtl.HouseName == AsrsModel.EnumStoreHouse.A1库房.ToString())
                //{
                //    port = ctlNodeManager.GetNodeByID("2002") as AsrsPortalModel;
                //}
                //else if (asrsCtl.HouseName == AsrsModel.EnumStoreHouse.A2库房.ToString())
                //{
                //    port = ctlNodeManager.GetNodeByID("2006") as AsrsPortalModel;
                //}
                //else
                //{
                //    return null;
                //}
                 string houseName = asrsCtl.HouseName;
                 CtlDBAccess.Model.ControlTaskModel task = null;
                foreach (CtlDBAccess.Model.ControlTaskModel t in taskList)
                {
                    if(t.TaskStatus != "待执行")
                    {
                        continue;
                    }
                    string reStr = "";
                    AsrsTaskParamModel paramModel = new AsrsTaskParamModel();
                    if (!paramModel.ParseParam(taskType, t.TaskParam, ref reStr))
                    {
                        continue;
                    }
                    AsrsPortalModel port = asrsCtl.Ports[paramModel.OutputPort-1];
                    int switchPathSeq = 1;
                    CellCoordModel cell = paramModel.CellPos1;
                    string area = "注液高温区";
                    if(!this.asrsResManage.GetLogicAreaName(houseName, cell, ref area))
                    {
                        continue;
                    }
                    if(area == "注液高温区")
                    {
                        switchPathSeq = 1;
                    }
                    else if (area == "化成高温区")
                    {
                        switchPathSeq = 2;
                    }
                    else
                    {
                        continue;
                    }
                    if(port.Db2Vals[switchPathSeq] !=1)
                    {
                        continue;
                    }
                    AsrsModel.EnumGSEnabledStatus cellEnabledStatus = AsrsModel.EnumGSEnabledStatus.启用;
                    if (!asrsResManage.GetCellEnabledStatus(houseName, paramModel.CellPos1, ref cellEnabledStatus))
                    {
                        // reStr = "获取货位启用状态失败";
                        continue;
                    }
                    if (cellEnabledStatus == AsrsModel.EnumGSEnabledStatus.禁用)
                    {
                        continue;
                    }
                    task = t;
                    break;
                }
                return task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
           
           
        }
        
        #endregion

    }
}
