﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace ESPER
{
                
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string serverUrl = "http://192.168.1.103/";
        Esper esper = new Esper(serverUrl);

        public MainPage()
        {
            this.InitializeComponent();

            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

            /*
            // Width
            ReceivedTextBoxScrollPresenter.Width = bounds.Width - 2;
            ReceivedTextBox.Width = bounds.Width - 2;


            // Height
            //ReceivedTextBoxScrollPresenter.Height = ReceivedTextBoxScrollPresenter.
            var texBoxHeight = bounds.Height - MainStack.Height - SendButton.Height;
            ReceivedTextBox.Height = texBoxHeight;
            ReceivedTextBoxScrollPresenter.Height = texBoxHeight;
            */

            //esper.PostString("Hey you!");
            //esper.SetPollingDelay(2000);
            //esper.GetDeviceName();
            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };


        }

        private async void Get_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            //string sendMessage = txTextBox.Text;
            ///esperPostString(serverUrl, sendMessage);
            esper.End();
        }

        private async void GetDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            var deviceName = await esper.GetDeviceName();
            DeviceNameLabel.Text = deviceName ?? "";
        }
    }
}
