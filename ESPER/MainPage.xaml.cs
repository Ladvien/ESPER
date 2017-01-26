using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ESPER
{
                
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HttpClient httpClient = new HttpClient();
        public MainPage()
        {

            const string serverUrl = "http://192.168.1.102/";
            this.InitializeComponent();
            //esperGet("http://192.168.1.102/");
            //esperPost("http://posttestserver.com/");

            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };
            //esperPostByteArray(serverUrl, testPacket);
            //esperPostString(serverUrl, "HEY YOU!");
            //esperGetData(serverUrl);
            //DoWorkPollingTask();

        }

        public async Task<string> esperGetData(string url)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));


            url += "buffer";
            var resourceUri = new Uri(url);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(resourceUri, null);
                var message = await response.Content.ReadAsStringAsync();
                if(message != "") { Debug.WriteLine(message); }
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

        public async void esperPostByteArray(string url, byte[] data)
        {
            
            var httpClient = new HttpClient();
            url += "data";
            var resourceUri = new Uri(url);
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

        public async void esperPostString(string url, string str)
        {

            var httpClient = new HttpClient();
            url += "string";
            var resourceUri = new Uri(url);
            try
            {

                using (HttpStringContent content = new HttpStringContent(str, UnicodeEncoding.Utf8))
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

        void DoWorkPollingTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    esperGetData("http://192.168.1.102/");   
                    // don't run again for at least 200 milliseconds
                    await Task.Delay(50);
                }
            });
        }

        private async void Get_Button_Click(object sender, RoutedEventArgs e)
        {
            var data = await esperGetData("http://192.168.1.102/");
            rxTextBox.Text += data;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            string sendMessage = txTextBox.Text;
            esperPostString("http://192.168.1.102/", sendMessage);
        }
    }
}
