using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowCtlBaseModel;
using MesDBAccess.BLL;
using MesDBAccess.Model;
namespace PrcsCtlModelsLishen
{
    public class NodeSwitchInput : CtlNodeBaseModel
    {
        private short barcodeFailedStat = 1;
        private List<FlowPathModel> flowPathList = new List<FlowPathModel>();
        private AsrsControl.AsrsPortalModel asrsInPort = null;
        /// <summary>
        /// 建立路径列表，只建两级路径，分流点-入口-堆垛机
        /// </summary>
        public override void BuildPathList()
        {
            int pathSeq = 1;
            foreach(CtlNodeBaseModel node in NextNodes)
            {
                foreach(CtlNodeBaseModel nextNode in node.NextNodes)
                {
                     FlowPathModel path = new FlowPathModel();
                     path.PathSeq = pathSeq;
                     path.AddNode(node);
                     path.AddNode(nextNode);
                     flowPathList.Add(path);
                     pathSeq++;
                }
            }
            asrsInPort = flowPathList[0].NodeList[0] as AsrsControl.AsrsPortalModel ;
        }
        public override bool ExeBusiness(ref string reStr)
        {
            if (db2Vals[0] == 1)
            {
                currentTaskPhase = 0;
                DevCmdReset();
                db1ValsToSnd[0] = 0;

                rfidUID = string.Empty;
                currentTaskDescribe = "等待新的任务";
                //return true;
            }
            if (db2Vals[0] == 2)
            {
                if (currentTaskPhase == 0)
                {
                    currentTaskPhase = 1;
                }
            }
            switch(this.currentTaskPhase)
            {
                case 1:
                    {
                        currentTaskDescribe = "开始读RFID";
                        this.rfidUID = "";
                        if (SysCfg.SysCfgModel.UnbindMode)
                        {
                            this.rfidUID = System.Guid.NewGuid().ToString();
                        }
                        else
                        {
                            if (SysCfg.SysCfgModel.SimMode || SysCfg.SysCfgModel.RfidSimMode)
                            {
                                this.rfidUID = this.SimRfidUID;
                            }
                            else
                            {
                                this.rfidUID = this.barcodeRW.ReadBarcode().Trim();
                               
                            }
                        }
                        if (string.IsNullOrWhiteSpace(this.rfidUID))
                        {
                            if (this.db1ValsToSnd[0] != barcodeFailedStat)
                            {
                                logRecorder.AddDebugLog(nodeName, "读料框条码失败");
                            }
                            
                            this.db1ValsToSnd[0] = barcodeFailedStat;
                            break;
                        }
                        if(this.rfidUID.Length<13)
                        {
                            if (this.db1ValsToSnd[0] != barcodeFailedStat)
                            {
                                logRecorder.AddDebugLog(nodeName, "读码错误，料框条码长度不足13字符:"+this.rfidUID);
                            }

                            this.db1ValsToSnd[0] = barcodeFailedStat;
                            break;
                        }
                
                        logRecorder.AddDebugLog(this.nodeName, "读到托盘号:" + this.rfidUID);
                        this.currentTaskPhase++;
                        break;
                    }
                case 2:
                    {
                        //分流
                        if(this.nodeID=="4001")
                        {
                            if(!Switch4001())
                            {
                                break;
                            }
                        }
                        else if(this.nodeID=="4003")
                        {
                            if(!Switch4003())
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                        //FlowPathModel switchPath = FindFirstValidPath(this.rfidUID, ref reStr);
                        //if(switchPath == null)
                        //{
                        //    switchRe = this.flowPathList.Count() + 2; //无可用路径，等待
                        //    this.db1ValsToSnd[0] = (short)switchRe;
                        //    break;
                        //}
                        //else
                        //{
                        //    switchRe = switchPath.PathSeq + 1;
                        //    CtlNodeBaseModel node = switchPath.NodeList[0];
                        //    if(node.GetType().ToString() == "AsrsControl.AsrsPortalModel")
                        //    {
                        //        (node as AsrsControl.AsrsPortalModel).PushPalletID(this.rfidUID);
                        //    }
                        //    this.db1ValsToSnd[0] = (short)switchRe;
                        //    logRecorder.AddDebugLog(nodeName, string.Format("{0}分流，进入{1}", this.rfidUID, switchPath.NodeList[0].NodeName));
                        //}
                       
                        this.currentTaskPhase++;
                        break;
                    }
                case 3:
                    {
                        currentTaskDescribe = "分流完成";
                        break;
                    }
                default:
                    break;
            }
            return true;
        }
        private bool Switch4001()
        {
            int switchRe = 0;
            string strCata = this.rfidUID.Substring(10, 1).ToUpper();
            int productCata = 0;
            string strCataName = "正极材料";
            int step = 0;
            string reStr = "";
            if (strCata == "C")
            {
                strCataName = "正极材料";
                productCata = 1;
                switchRe = 2;
                step = 1;
            }
            else if (strCata == "A")
            {
                strCataName = "负极材料";
                productCata = 3;
                step = 2;
                switchRe = 3;
            }
            else if (strCata == "S")
            {
                strCataName = "隔膜材料";
                productCata = 2;
                step = 0;
                switchRe = 2;
            }
            else
            {
                if (this.db1ValsToSnd[0] != barcodeFailedStat)
                {
                    logRecorder.AddDebugLog(nodeName, "不可识别的条码类别：" + this.rfidUID);
                }
                return false;
            }
            if (switchRe == 2)
            {
                if (asrsInPort.PalletBuffer.Count() >= asrsInPort.PortinBufCapacity)
                {
                    this.currentTaskDescribe = string.Format("入库口托盘缓存已满,分流等待,{0}", this.rfidUID);
                    this.db1ValsToSnd[0] = 4;
                    return false;
                }
                //在线产品记录
                if (!RecordPalletInfo(this.rfidUID,ref strCataName,ref reStr))
                {
                    return false;
                }
      
                logRecorder.AddDebugLog(nodeName, string.Format("{0}{1}，分流进立库", this.rfidUID, strCataName));
                asrsInPort.PushPalletID(this.rfidUID);
                this.db1ValsToSnd[0] = 2;
                return true;
            }
            else
            {
                this.db1ValsToSnd[0] = 3;
                logRecorder.AddDebugLog(nodeName, string.Format("{0}负极材料，分流进烘烤线", this.rfidUID));
                return true;
            }
        }
        private bool Switch4003()
        {
            int switchRe = 0;
            string reStr = "";
            FlowPathModel switchPath = FindFirstValidPath(this.rfidUID, ref reStr);
            if (switchPath == null)
            {
                switchRe = this.flowPathList.Count() + 2; //无可用路径，等待
                this.db1ValsToSnd[0] = 3;
                return false;
            }
            else
            {
                //string strCata = this.rfidUID.Substring(10, 1).ToUpper();
              
                string strCataName = "正极材料";
                if(!RecordPalletInfo(this.rfidUID,ref strCataName,ref reStr))
                {
                    return false;
                }

                switchRe = switchPath.PathSeq + 1;
                CtlNodeBaseModel node = switchPath.NodeList[0];
                if (node.GetType().ToString() == "AsrsControl.AsrsPortalModel")
                {
                    (node as AsrsControl.AsrsPortalModel).PushPalletID(this.rfidUID);
                }
                this.db1ValsToSnd[0] = (short)switchRe;
                logRecorder.AddDebugLog(nodeName, string.Format("{0}分流，进入{1}", this.rfidUID, switchPath.NodeList[0].NodeName));
                return true;
            }
        }
        private FlowPathModel FindFirstValidPath(string palletID,ref string reStr)
        {
            List<FlowPathModel> validPathList = new List<FlowPathModel>();

            foreach (FlowPathModel path in flowPathList)
            {
                if (path.IsPathConnected(palletID, ref reStr))
                {
                    validPathList.Add(path);
                }
            }
            if(validPathList.Count()==0)
            {
                reStr = "没有可用分流路径";
                return null;
            }
            //排序
            FlowPathModel rePath = validPathList[0];
            if (validPathList.Count()>1)
            {
                for(int i=1;i<validPathList.Count();i++)
                {
                    FlowPathModel path = validPathList[i];
                    CtlNodeBaseModel node1 = rePath.NodeList[0];
                    CtlNodeBaseModel node2 = path.NodeList[0];
                    if(node2.PathValidWeight(palletID,ref reStr)>node1.PathValidWeight(palletID,ref reStr))
                    {
                        rePath = path;
                    }

                }
            }
            return rePath;
        }
        private bool RecordPalletInfo(string palletID,ref string strCataName,ref string reStr)
        {
            string strCata = palletID.Substring(10, 1).ToUpper();
            int productCata = 0;
            int step = 0;
            strCataName = "正极材料";

            if (strCata == "C")
            {
                strCataName = "正极材料";
                productCata = 1;

                step = 1;
            }
            else if (strCata == "A")
            {
                strCataName = "负极材料";
                productCata = 3;
                step = 2;

            }
            else if (strCata == "S")
            {
                strCataName = "隔膜材料";
                productCata = 2;
                step = 0;

            }
            else
            {
                if (this.db1ValsToSnd[0] != barcodeFailedStat)
                {
                    logRecorder.AddDebugLog(nodeName, "不可识别的条码类别：" + this.rfidUID);
                }
                return false;
            }

            palletBll palletDBll = new palletBll();
            palletModel pallet = palletDBll.GetModel(this.rfidUID);
            if (pallet == null)
            {
                pallet = new palletModel();
                pallet.stepNO = step;
                pallet.bind = true;
                pallet.palletID = this.rfidUID;
                pallet.palletCata = productCata.ToString();
                if (!palletDBll.Add(pallet))
                {
                    logRecorder.AddDebugLog(nodeName, string.Format("物料{0}数据记录到数据库发生错误", this.rfidUID));
                    return false;
                }
            }
            else
            {
                pallet.bind = true;
                pallet.palletCata = productCata.ToString();
                pallet.stepNO = step;
                if (!palletDBll.Update(pallet))
                {
                    logRecorder.AddDebugLog(nodeName, string.Format("物料{0}数据记录到数据库发生错误", this.rfidUID));
                    return false;
                }
            }
            return true;
        }
    }
}
