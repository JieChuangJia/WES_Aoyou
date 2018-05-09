using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FlowCtlBaseModel;
namespace PrcsCtlModelsLishen
{
    public class PrsCtlnodeManage
    {
        private List<CtlNodeBaseModel> monitorNodeList = null;
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
                        
                        case "PrcsCtlModelsLishen.NodeSwitchInput":
                            {
                                ctlNode = new NodeSwitchInput();
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
        public bool DevStatusRestore()
        {
            try
            {
                foreach (CtlNodeBaseModel node in monitorNodeList)
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
