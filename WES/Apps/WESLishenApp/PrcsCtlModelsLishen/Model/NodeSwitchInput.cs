using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowCtlBaseModel;
using MesDBAccess.BLL;
using MesDBAccess.Model;
using LishenMesDBAccess.Model;
using LishenMesDBAccess.BLL;
namespace PrcsCtlModelsLishen
{
    public class NodeSwitchInput : CtlNodeBaseModel
    {
        private short barcodeFailedStat = 1;
        private List<FlowPathModel> flowPathList = new List<FlowPathModel>();
        private AsrsControl.AsrsPortalModel asrsInPort = null;
        public AsrsControl.AsrsCtlModel AsrsCtl { get; set; }
        public AsrsControl.AsrsPortalModel AsrsPort { get; set; }
        public AsrsInterface.IAsrsManageToCtl AsrsResManage { get; set; } 
        public NodeSwitchInput()
        {
            AsrsResManage = null;
        }
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
            
                db1ValsToSnd[0] = 0;
                if(this.nodeID=="4001")
                {
                    db1ValsToSnd[1] = 1;
                }
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

            //string strCata = "";// this.rfidUID.Substring(10, 1).ToUpper();
            short productCata = 0;
            string strCataName = "";// "正极材料";
            string reStr="";
            string shopName = "";
            if (!ParsePalletID(this.rfidUID,ref shopName, ref productCata, ref strCataName, ref reStr))
            {
                this.currentTaskDescribe = reStr;
                return false;
            }
           // int step = 0;
            MatProcessBll shopPrsBll = new MatProcessBll();
            MatProcessModel shopPrs= shopPrsBll.GetModel(shopName);
            if(shopPrs == null)
            {
                this.currentTaskDescribe = shopName+"无烘烤工艺配置,请配置";
                return false;
            }
            if (productCata==1) //正极
            {
                if(shopPrs.ZhengjiHongkao>0)
                {
                    switchRe = 2 + shopPrs.ZhengjiHongkao;
                }
                else
                {
                    switchRe = 2;//不烘烤
                }
                
              //  step = 1;
            }
            else if (productCata==3)//负极
            {
              //  step = 2;
                if(shopPrs.FujiHongkao>0)
                {
                    switchRe = 2 + shopPrs.FujiHongkao;
                }
                else
                {
                    switchRe = 2;
                }
                
            }
            else if (productCata == 2) //隔膜
            {
                if(shopPrs.GemoHongkao>0)
                {
                    switchRe = 2 + shopPrs.GemoHongkao;
                }
                else
                {
                    switchRe = 2;
                }
                
            }
            else
            {
                if (this.db1ValsToSnd[0] != barcodeFailedStat)
                {
                    logRecorder.AddDebugLog(nodeName, "不可识别的条码类别：" + this.rfidUID);
                }
                this.db1ValsToSnd[0] = barcodeFailedStat;
                return false;
            }
            //检索库里的空框类型，生成对应的空框出库任务，如果没有可以出库的空框也给出反馈
            
            short emptypalletReqRe = 0;
          //  if(this.db1ValsToSnd[1] ==1)
          //  {
            if(AsrsPort.Db2Vals[0] == 2) //允许出库时才生成任务
            {
                if (EmptyPalletOutrequire(shopName, productCata, ref emptypalletReqRe, ref reStr))
                {
                    this.db1ValsToSnd[1] = 2;
                }
                else
                {
                    if (this.db1ValsToSnd[1] != 3)
                    {
                        logRecorder.AddDebugLog(nodeName, string.Format("无{0}空框{1}可以出库,{2}", shopName, strCataName, reStr));
                    }
                    this.db1ValsToSnd[1] = 3;
                    
                    //  return false;
                }
            }
            else
            {
                if (this.db1ValsToSnd[1] != 3)
                {
                    logRecorder.AddDebugLog(nodeName, string.Format("因为空筐出口处于有料状态,无{0}空框{1}可以出库,{2}", shopName, strCataName, reStr));
                }
                this.db1ValsToSnd[1] = 3;
            }
        
           
            if (switchRe == 2)
            {
                if (asrsInPort.PalletBuffer.Count() >= asrsInPort.PortinBufCapacity)
                {
                    this.currentTaskDescribe = string.Format("入库口托盘缓存已满,分流等待,{0}", this.rfidUID);
                    this.db1ValsToSnd[0] = 0;
                    return false;
                }
                //在线产品记录
                if (!RecordPalletInfo(this.rfidUID,ref strCataName,ref reStr))
                {
                    return false;
                }
      
                logRecorder.AddDebugLog(nodeName, string.Format("{0}{1}，分流进立库", this.rfidUID, strCataName));
                asrsInPort.PushPalletID(this.rfidUID);
                this.db1ValsToSnd[0] = (short)switchRe;
                return true;
            }
            else
            {
                this.db1ValsToSnd[0] = (short)switchRe;
                logRecorder.AddDebugLog(nodeName, string.Format("{0},材料：{1}，分流进烘烤线{2}", this.rfidUID, strCataName, switchRe - 2));
                return true;
            }
        }
        private bool EmptyPalletOutrequire(string shopName,short palletCata,ref short re,ref string reStr)
        {
            
            string houseName = "A1库房";
            List<CtlDBAccess.Model.ControlTaskModel> taskList=ctlTaskBll.GetModelList(string.Format("TaskType= {0} and (TaskStatus='{1}' or TaskStatus='{2}')", (int)SysCfg.EnumAsrsTaskType.空筐出库, SysCfg.EnumTaskStatus.待执行.ToString(), SysCfg.EnumTaskStatus.执行中.ToString()));
            if(taskList != null && taskList.Count()>1)
            {
                reStr = "已经存在待执行空筐出库任务";
                return false;
            }
            //遍历所有库位，判断材料类别，按照先入先出规则，匹配出库的货位。
            Dictionary<string, AsrsModel.GSMemTempModel> asrsStatDic = new Dictionary<string, AsrsModel.GSMemTempModel>();
            
            if (!AsrsResManage.GetAllGsModel(ref asrsStatDic, ref reStr))
            {
                Console.WriteLine(string.Format("{0} 获取货位状态失败", houseName));
                return false;
            }
            List<AsrsModel.GSMemTempModel> validCells = new List<AsrsModel.GSMemTempModel>();
            int r = 1, c = 1, L = 1;
            for (r = 1; r < AsrsCtl.AsrsRow + 1; r++)
            {
                for (c = 1; c < AsrsCtl.AsrsCol + 1; c++)
                {
                    for (L = 1; L <AsrsCtl.AsrsLayer + 1; L++)
                    {
                        string strKey = string.Format("{0}:{1}-{2}-{3}", houseName, r, c, L);
                        AsrsModel.GSMemTempModel cellStat = null;
                        if (!asrsStatDic.Keys.Contains(strKey))
                        {
                            continue;
                        }
                        cellStat = asrsStatDic[strKey];
                        if ((!cellStat.GSEnabled) || (cellStat.GSTaskStatus == AsrsModel.EnumGSTaskStatus.锁定.ToString()) || (cellStat.GSStatus != AsrsModel.EnumCellStatus.空料框.ToString()))
                        {
                            // reStr = string.Format("货位{0}-{1}-{2}禁用,无法生成出库任务", cell.Row, cell.Col, cell.Layer);
                            continue;
                        }
                        AsrsModel.CellCoordModel cell = new AsrsModel.CellCoordModel(r, c, L);
                        List<string> storGoods = new List<string>();
                        if (!AsrsResManage.GetStockDetail(houseName, cell, ref storGoods))
                        {
                            continue;
                        }
                        if (storGoods.Count() < 1)
                        {
                            continue;
                        }
                        string palletID = storGoods[0];
                      //  string strCata = "";// this.rfidUID.Substring(10, 1).ToUpper();
                        short productCata = 0;
                        string strCataName = "";// "正极材料";
                        string storeShopName = "";
                        if (!ParsePalletID(palletID, ref storeShopName, ref productCata, ref strCataName, ref reStr))
                        {
                            continue;
                        }
                        if ((productCata ==palletCata) && (shopName== storeShopName))
                        {
                            validCells.Add(cellStat);
                        }
                    }
                }
            }
            if (validCells.Count() > 0)
            {
                //排序，按照先入先出
                AsrsModel.GSMemTempModel firstGS = validCells[0];
                if (validCells.Count() > 1)
                {
                    for (int i = 1; i < validCells.Count(); i++)
                    {
                        AsrsModel.GSMemTempModel tempGS = validCells[i];
                        if (tempGS.InHouseDate < firstGS.InHouseDate)
                        {
                            firstGS = tempGS;
                        }
                    }
                }
                //生成出库任务
                string[] strCellArray = firstGS.GSPos.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                int row = int.Parse(strCellArray[0]);
                int col = int.Parse(strCellArray[1]);
                int layer = int.Parse(strCellArray[2]);
                AsrsModel.CellCoordModel cell = new AsrsModel.CellCoordModel(row, col, layer);
                if (AsrsCtl.GenerateOutputTask(cell, SysCfg.EnumAsrsTaskType.空筐出库, true, AsrsPort.PortSeq, ref reStr, new List<short> { palletCata },AsrsPort.AsrsTaskPri))
                {
                 //   AsrsPort.Db1ValsToSnd[0] = 2; //
                    return true;
                }
                else
                {
                    Console.WriteLine("生成任务{0}失败,{1}", AsrsPort.BindedTaskOutput.ToString(), reStr);
                    return false;
                }
            }
            else
            {
                return false;
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
        public static bool ParsePalletID(string palletID, ref string shopName,ref short cata, ref string strCataName, ref string reStr)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(palletID))
                {
                    reStr = "料筐条码为空";
                    return false;
                }
                if(palletID.Length<11)
                {
                    reStr = "料筐条码长度不足";
                    return false;
                }
                int seqNo = 0;
                string strSeqNo=palletID.Substring(11, 2);
                if(!int.TryParse(strSeqNo,out seqNo))
                {
                    reStr = "治具流水号解析错误，应当为数字,实际：" + strSeqNo;
                    return false;
                }
                string strCata = palletID.Substring(10, 1).ToUpper();
                cata = 0;
                strCataName = "";
                if(strCata=="S")
                {
                    if (seqNo > 0 && seqNo < 30)
                    {
                        shopName = "1号车间";
                    }
                    else if (seqNo > 30 && seqNo < 60)
                    {
                        shopName = "2号车间";
                    }
                    else if (seqNo > 60 && seqNo < 90)
                    {
                        shopName = "3号车间";
                    }
                    else
                    {
                        reStr = "不可识别的车间标识，治具流水号：" + seqNo.ToString();
                        return false;
                    }
       
                }
                else
                {
                    if (seqNo > 0 && seqNo < 20)
                    {
                        shopName = "1号车间";
                    }
                    else if (seqNo > 20 && seqNo < 70)
                    {
                        shopName = "2号车间";
                    }
                    else if (seqNo > 70 && seqNo < 89)
                    {
                        shopName = "3号车间";
                    }
                    else
                    {
                        reStr = "不可识别的车间标识，治具流水号：" + seqNo.ToString();
                        return false;
                    }
                }
                if (strCata == "C")
                {
                    strCataName = "正极材料";
                    cata = 1;
                  
                }
                else if (strCata == "A")
                {
                    strCataName = "负极材料";
                    cata = 3;
                    
                }
                else if (strCata == "S")
                {
                    strCataName = "隔膜材料";
                    cata = 2;
                   
                }
                else
                {
                    reStr = "无法解析条码" + palletID;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                reStr = ex.ToString();
                return false;
            }
        }
    }
}
