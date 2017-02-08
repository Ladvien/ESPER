using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPER.LumiCentralManagement
{
    public delegate void CentralStateChangeEventHandler(object source, EventArgs args);
    class LumiCentral: ILumiCentral
    {
    }
}
