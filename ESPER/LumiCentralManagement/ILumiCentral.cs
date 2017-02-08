using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESPER.LumiPeripheralManagement;

namespace ESPER.LumiCentralManagement
{
    interface ILumiCentral
    {

        #region delegates and events
        // Delegate info:
        // https://www.youtube.com/watch?v=jQgwEsJISy0
        event CentralStateChangeEventHandler CentralStateChanged;
        void OnCentralStateChanged();
        #endregion delegates and events

        #region properties
        LumiDeviceState State { get; set; }
        #endregion properties

        #region methods
        string GetConnectedDevicesNames();
        bool ConnectToDevice(LumiPeripheral peripheral);
        #endregion methods
    }
}
