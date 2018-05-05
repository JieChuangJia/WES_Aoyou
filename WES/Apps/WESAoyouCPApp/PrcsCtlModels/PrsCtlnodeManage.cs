﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FlowCtlBaseModel;
using AsrsInterface;
namespace PrcsCtlModelsAoyouCp
{
    public class PrsCtlnodeManage
    {
        private List<CtlNodeBaseModel> monitorNodeList = null;
       // public MesAccWrapper MesAcc { get; set; }
        public PrsCtlnodeManage()
        {
          //  MesAcc = new MesAccAoyou();
        }
     //   public CtlManage.CommDevManage DevCommManager { get; set; }
        public bool CtlInit(XElement CtlnodeRoot, ref string reStr)
        {
            monitorNodeList = new List<CtlNodeBaseModel>();
            if (CtlnodeRoot == null)
            {
                reStr = "系统配置文件错误，不存在CtlNodes节点";
                return false;
            }
            try
            {
                IEnumerable<XElement> nodeXEList =
                from el in CtlnodeRoot.Elements()
                where el.Name == "Node"
                select el;
                foreach (XElement el in nodeXEList)
                {
                    string className = (string)el.Attribute("className");
                    CtlNodeBaseModel ctlNode = null;
                    switch (className)
                    {
                        case "PrcsCtlModelsAoyou.NodeGrasp":
                            {
                                NodeGrasp graspNode = new NodeGrasp();
                              
                                ctlNode = graspNode;
                                break;
                            }
                        case "PrcsCtlModelsAoyou.NodeSwitchInput":
                            {
                                ctlNode = new NodeSwitchInput();
                                break;
                            }
                        case "PrcsCtlModelsAoyou.NodeSwitchOutput":
                            {
                                ctlNode = new NodeSwitchOutput();
                                break;
                            }
                        default:
                            break;
                    }
                    if (ctlNode != null)
                    {
                        
                        if (!ctlNode.BuildCfg(el, ref reStr))
                        {
                            return false;
                        }
                       
                        this.monitorNodeList.Add(ctlNode);
                    }

                }
            }
            catch (Exception ex)
            {
                reStr = ex.ToString();
                return false;
            }

            return true;
        }
        public IList<CtlNodeBaseModel> GetAllCtlNodes()
        {
            return monitorNodeList;
        }
        public void SetLogRecorder(LogInterface.ILogRecorder logRecorder)
        {
            foreach(CtlNodeBaseModel node in monitorNodeList)
            {
                node.LogRecorder = logRecorder;
            }
        }
        public void SetAsrsResManage(IAsrsManageToCtl asrsRes)
        {
            foreach(CtlNodeBaseModel node in monitorNodeList)
            {
                if(node.GetType().ToString() == "PrcsCtlModelsAoyou.NodeSwitchOutput")
                {
                    (node as NodeSwitchOutput).AsrsResManage = asrsRes;
                }
                else if(node.GetType().ToString() == "PrcsCtlModelsAoyou.NodeSwitchInput")
                {
                    (node as NodeSwitchInput).AsrsResManage = asrsRes;
                }
            }

        }
        public bool DevStatusRestore()
        {
            try
            {
                foreach(CtlNodeBaseModel node in monitorNodeList)
                {
                    node.DevStatusRestore();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
