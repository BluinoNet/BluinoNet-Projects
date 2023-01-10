
using nanoFramework.Networking;
using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Device.Wifi;
using SASBuilder;
using System.Text;
using nanoFramework.M2Mqtt;

namespace AzureDevice
{
    public class Program
    {
        const string Ssid = "WholeOffice";
        const string Password = "123qweasd";

        /// <summary>
        /// Event handler for when Wifi scan completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            Debug.WriteLine("Wifi_AvailableNetworksChanged - get report");

            // Get Report of all scanned Wifi networks
            WifiNetworkReport report = sender.NetworkReport;

            // Enumerate though networks looking for our network
            foreach (WifiAvailableNetwork net in report.AvailableNetworks)
            {
                // Show all networks found
                Debug.WriteLine($"Net SSID :{net.Ssid},  BSSID : {net.Bsid},  rssi : {net.NetworkRssiInDecibelMilliwatts.ToString()},  signal : {net.SignalBars.ToString()}");

                // If its our Network then try to connect
                if (net.Ssid == Ssid)
                {
                    // Disconnect in case we are already connected
                    sender.Disconnect();

                    // Connect to network
                    WifiConnectionResult result = sender.Connect(net, WifiReconnectionKind.Automatic, Password);

                    // Display status
                    if (result.ConnectionStatus == WifiConnectionStatus.Success)
                    {
                        Debug.WriteLine("Connected to Wifi network");
                        Connected = true;
                    }
                    else
                    {
                        Debug.WriteLine($"Error {result.ConnectionStatus.ToString()} connecting o Wifi network");
                    }
                }
            }
        }
        static bool Connected = false;
        public static void Main()
        {
            try
            {
                // Get the first WiFI Adapter
                WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];

                // Set up the AvailableNetworksChanged event to pick up when scan has completed
                wifi.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;

                // give it some time to perform the initial "connect"
                // trying to scan while the device is still in the connect procedure will throw an exception
                Thread.Sleep(10_000);

                // Loop forever scanning every 30 seconds

                try
                {
                    Debug.WriteLine("starting Wi-Fi scan");
                    wifi.ScanAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failure starting a scan operation: {ex}");
                }
                while (!Connected)
                {
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("message:" + ex.Message);
                Debug.WriteLine("stack:" + ex.StackTrace);
            }

            var caCert = new X509Certificate(Resource1.GetBytes(Resource1.BinaryResources.AzureRoot));

            var iotHubName = "BmcIoTHub.azure-devices.net";
            var iotHubPort = 8883;

            // device/client information
            const string deviceId = "myFirstDevice";

            var username = string.Format("{0}/{1}", iotHubName, deviceId);

            // Data time is important for calculate expire time
            //SystemTime.SetTime(new DateTime(2023, 1, 10));
            /*
            var sas = new SharedAccessSignatureBuilder()
            {
                Key = "vj7aZFDZikR7XJv75Ah50EC5fXPsHHHWrcBQ6OugcxU",
                KeyName = "iothubowner",
                Target = "BmcIoTHub.azure-devices.net",
                TimeToLive = TimeSpan.FromDays(365) // at least 1 day.
            };
            */
            // define topics
            var topicDeviceToServer =
            string.Format("devices/{0}/messages/events/", deviceId);

            var topicService2Device =
            string.Format("devices/{0}/messages/devicebound/#", deviceId);

            try
            {

                var client = new MqttClient(iotHubName, iotHubPort, true, caCert, null, MqttSslProtocols.TLSv1_2);

                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;


                var sas = "SharedAccessSignature sr=BmcIoTHub.azure-devices.net&sig=uhce%2BkWkx5k299CKCqQbhe5DT9EyzrISTEczzSznA%2BE%3D&se=1704889619&skn=iothubowner";
                var returnCode = client.Connect(deviceId, username, sas);

                if (returnCode != nanoFramework.M2Mqtt.Messages.MqttReasonCode.Success)
                    throw new Exception("Could not connect!");

                ushort packetId = 1;

                client.Subscribe(new string[] { topicService2Device }, new nanoFramework.M2Mqtt.Messages.MqttQoSLevel[]
                    {  nanoFramework.M2Mqtt.Messages.MqttQoSLevel.ExactlyOnce });

                client.Subscribe(new string[] { topicDeviceToServer }, new nanoFramework.M2Mqtt.Messages.MqttQoSLevel[]
                    {  nanoFramework.M2Mqtt.Messages.MqttQoSLevel.ExactlyOnce });

                for (int i = 0; i < 100; i++)
                {
                    client.Publish(topicDeviceToServer, Encoding.UTF8.GetBytes($"Count-{i}"));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }


            Thread.Sleep(Timeout.InfiniteTimeSpan);
        }

        private static void Client_MqttMsgPublishReceived(object sender, nanoFramework.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {

            Debug.WriteLine("Received message: " + Encoding.UTF8.GetString(e.Message, 0, e.Message.Length));

        }


    }
}
/*
  
 */