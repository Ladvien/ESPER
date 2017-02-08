using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPER
{
    
    class LumiDeviceState
    {
        public enum State : int
        {
            Unknown,
            On,
            Off,
            Connected,
            Disconnected
        }

        public State _State { get; set; }
        public void UpdateState(State state)
        {
            _State = state;
        }
    }
}
