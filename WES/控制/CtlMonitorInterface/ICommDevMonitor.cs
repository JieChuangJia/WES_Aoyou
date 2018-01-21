using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CtlMonitorInterface
{
    public interface ICommDevMonitor
    {
        IDictionary<string, DevConnStat> GetPLCConnStatDic();
    }
}
