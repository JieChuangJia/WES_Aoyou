using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace CtlMonitorInterface
{
    public interface ICtlnodeMonitor
    {
       
        List<string> GetMonitorNodeNames();
        bool GetDevRunningInfo(string nodeName, ref DataTable db1Dt, ref DataTable db2Dt, ref string taskDetail);
        bool SimSetDB2(string nodeName, int dbItemID, int val);
        void SimSetRFID(string nodeName, string strUID);
        void SimSetBarcode(string nodeName, string barcode);
    }
}
