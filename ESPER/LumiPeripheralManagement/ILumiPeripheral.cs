using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPER.LumiPeripheralManagement
{
    interface ILumiPeripheral
    {
        #region delegates and events
        // Delegate info:
        // https://www.youtube.com/watch?v=jQgwEsJISy0
        event ReceivedDataEventHandler ReceivedData;
        void OnReceivedData();
        event ReceivedDataEventHandler SentData;
        void OnSentData();
        #endregion delegates and events

        #region properties
        LumiDeviceState State { get; set; }
        List<byte> ReceivedBufferUpdated { get; set; }
        #endregion properties

        #region methods
        Dictionary<string, string> Search();
        bool AddDataToSendBuffer(byte[] data);
        bool AddStringToSendBuffer(string str);
        #endregion methods
    }
}
