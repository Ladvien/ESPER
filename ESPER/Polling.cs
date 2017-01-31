using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESPER
{
    class Polling
    {
        public delegate byte[] PollingFunction();
        CancellationTokenSource PollingForDataCancelToken;

        public void Start()
        {
            PollWebServerDataAvailability();
        }

        public void End()
        {
            PollingForDataCancelToken.Cancel();
        }

        private void PollWebServerDataAvailability()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    //PollingFunction();
                    await Task.Delay(50);
                }
            }, PollingForDataCancelToken.Token);
        }
    }
}
