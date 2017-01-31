﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace ESPER
{
    class Esper
    {
        const int defaultPollingDelay = 50; 

        HttpClient httpClient = new HttpClient();
        CancellationTokenSource PollingForDataCancelToken = new CancellationTokenSource();

        private string WebServerUrl { get; set; }
        private int PollingDelay { get; set; } = defaultPollingDelay;
        private bool PollingActive { get; set; } = false;

        public Esper(string consumerUrl)
        {
            WebServerUrl = consumerUrl;
        }

        public async void PostByteArray(byte[] data)
        {

            var httpClient = new HttpClient();
            var webService = WebServerUrl + "data";
            var resourceUri = new Uri(WebServerUrl);
            try
            {
                IBuffer buffer = data.AsBuffer();
                using (HttpBufferContent content = new HttpBufferContent(buffer))
                {
                    content.Headers.Add("Content-Type", "text/html; charset=utf-8");
                    content.Headers.ContentLength = buffer.Length;
                    var response = await httpClient.PostAsync(resourceUri, content);
                    Debug.WriteLine(response);
                }
            }
            catch (TaskCanceledException ex)
            {
                // Handle request being canceled due to timeout.
            }
        }

        public async void PostString(string str)
        {
            var httpClient = new HttpClient();
            var webService = WebServerUrl + "string";
            var resourceUri = new Uri(webService);
            try
            {
                using (HttpStringContent content = new HttpStringContent(str, Windows.Storage.Streams.UnicodeEncoding.Utf8))
                {
                    content.Headers.ContentLength = (ulong)str.Length;
                    using (var response = await httpClient.PostAsync(resourceUri, content)) { };
                }
            }
            catch (TaskCanceledException ex)
            {
                // Handle request being canceled due to timeout.
            }
        }

        public async Task<string> GetDeviceName()
        {
            var httpClient = new HttpClient();
            var webService = WebServerUrl + "name";
            var resourceUri = new Uri(webService);
            try
            {
                var response = await httpClient.PostAsync(resourceUri, null);
                Debug.WriteLine(response);
                var returnMessage = response.Content.ToString();
                response.Dispose();
                return returnMessage;
            }
            catch (TaskCanceledException ex)
            {
                // Handle request being canceled due to timeout.
            }
            return "";
        }

        public void Start()
        {
            if(false == PollingActive)
            {
                PollingActive = true;
                PollingForDataCancelToken = new CancellationTokenSource();
                PollWebServerDataAvailability();
            }
        }

        public void End()
        {
            PollingActive = false;
            PollingForDataCancelToken.Cancel();
        }

        public void SetPollingDelay(int delayInMilliseconds) { PollingDelay = delayInMilliseconds; }

        private void PollWebServerDataAvailability()
        {   
            try
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (PollingForDataCancelToken.IsCancellationRequested)
                        {
                            PollingForDataCancelToken.Token.ThrowIfCancellationRequested();
                        }
                        await GetData();
                        await Task.Delay(PollingDelay);
                    }
                }, PollingForDataCancelToken.Token);
            } catch (TaskCanceledException)
            {
                // TODO: Add cancelation callback here.
            }
        }

        public async Task<string> GetData()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            var webService = WebServerUrl + "buffer";
            var resourceUri = new Uri(webService);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(resourceUri, null);
                var message = await response.Content.ReadAsStringAsync();
                if (message != "") { Debug.WriteLine(message); }
                response.Dispose();
                cts.Dispose();
                return message;
            }
            catch (TaskCanceledException ex)
            {
                // Handle request being canceled due to timeout.
                return "";
            }
            return "";
        }
    }
}
