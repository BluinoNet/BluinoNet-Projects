using BluinoNet;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace MqttLogger
{
    public class Program
    {
        static double CurrentTemperature, MaximumTemperature, MinimumTemperature, TemperaturePosition;
        static ESP32StarterKit board;
        public static void Main()
        {
            StarterKit.Setup(board);
            CurrentTemperature = StarterKit.TemperatureSensor.ReadTemperature();
            MinimumTemperature = CurrentTemperature;
            MaximumTemperature = CurrentTemperature;

            //network
            MqttClient client = null;
            string clientId;
            bool running = true;

            // Wait for Wifi/network to connect (temp)
            SetupAndConnectNetwork();

            try
            {
                var topic = "mifmasterz@yahoo.com/project-a/data";
                clientId = "device-project-a-32311";
                var MQTT_BROKER_ADDRESS = "103.250.10.88";//"cloud-iot.my.id";//
                var uname = "mifmasterz@yahoo.com";
                var pass = "123qweasd";

                client = new MqttClient(MQTT_BROKER_ADDRESS);

                // register a callback-function (we have to implement, see below) which is called by the library when a message was received
                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                client.MqttMsgSubscribed += Client_MqttMsgSubscribed;

                // use a unique id as client id, each time we start the application
                //clientId = Guid.NewGuid().ToString();
               
                Debug.WriteLine("Connecting MQTT");

                client.Connect(clientId, uname, pass);

                Debug.WriteLine("Connected MQTT");
                // Subscribe topics
                //     client.Subscribe(new string[] { "Test1", "Test2" }, new MqttQoSLevel[] { MqttQoSLevel.ExactlyOnce, MqttQoSLevel.ExactlyOnce });

               
                string[] SubTopics = new string[]
                {
                        topic
                };

               
                client.Subscribe(SubTopics, new MqttQoSLevel[] { MqttQoSLevel.ExactlyOnce });

                Debug.WriteLine("Enter wait loop");
                var sensorData = new SensorData();
                Random rnd = new Random();
                while (true)
                {
                    CurrentTemperature = StarterKit.TemperatureSensor.ReadTemperature();
                    if (CurrentTemperature > MaximumTemperature)
                        MaximumTemperature = CurrentTemperature;
                    if (CurrentTemperature < MinimumTemperature)
                        MinimumTemperature = CurrentTemperature;

                    StarterKit.Display.Clear();

                    StarterKit.Display.DrawString("Current", StarterKit.colorBlue, 39, 0);
                    StarterKit.Display.DrawString(CurrentTemperature.ToString("F1"), StarterKit.colorBlue, 37, 12);

                    StarterKit.Display.DrawString("Minimum", StarterKit.colorBlue, 2, 34);
                    StarterKit.Display.DrawString(MinimumTemperature.ToString("F1"), StarterKit.colorBlue, 0, 46);

                    StarterKit.Display.DrawString("Maximum", StarterKit.colorBlue, 72, 34);
                    StarterKit.Display.DrawString(MaximumTemperature.ToString("F1"), StarterKit.colorBlue, 70, 46);

                    if ((MaximumTemperature - MinimumTemperature) > 0)
                    {
                        TemperaturePosition = (CurrentTemperature - MinimumTemperature) / (MaximumTemperature - MinimumTemperature) * 127;
                        StarterKit.Display.DrawLine(StarterKit.colorBlue, 0, 63, (int)TemperaturePosition, 63);
                        StarterKit.Display.SetPixel(0, 62, StarterKit.colorBlue);
                        StarterKit.Display.SetPixel(127, 62, StarterKit.colorBlue);
                        sensorData.Temp = (int)CurrentTemperature;
                        StarterKit.LightBulb.SetColorRGB((int)(TemperaturePosition / 32), 0, (int)((127 - TemperaturePosition) / 16));
                    }
                    
                    sensorData.Distance = rnd.Next(100);
                    var json = nanoFramework.Json.JsonConvert.SerializeObject(sensorData);
                    byte[] message = Encoding.UTF8.GetBytes(json);
                    client.Publish(topic, message, MqttQoSLevel.ExactlyOnce, false);
                    StarterKit.Display.Flush();

                    Thread.Sleep(1000);
                }
               
            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
                Debug.WriteLine("Main exception " + ex.Message);
            }
            client.Disconnect();

        }
        private const string c_SSID = "POCO F3";
        private const string c_AP_PASSWORD = "123qweasd";

       
        private static void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Debug.WriteLine("Client_MqttMsgSubscribed ");
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;

            string message = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);

            Debug.WriteLine("Publish Received Topic:" + topic + " Message:" + message);

        }
        public static void SetupAndConnectNetwork()
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            if (nis.Length > 0)
            {
                // get the first interface
                NetworkInterface ni = nis[0];

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // network interface is Wi-Fi
                    Debug.WriteLine("Network connection is: Wi-Fi");

                    Wireless80211Configuration wc = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
                    if (wc.Ssid != c_SSID && wc.Password != c_AP_PASSWORD)
                    {
                        // have to update Wi-Fi configuration
                        wc.Ssid = c_SSID;
                        wc.Password = c_AP_PASSWORD;
                        wc.SaveConfiguration();
                    }
                    else
                    {   // Wi-Fi configuration matches
                    }
                }
                else
                {
                    // network interface is Ethernet
                    Debug.WriteLine("Network connection is: Ethernet");

                    ni.EnableDhcp();
                }

                // wait for DHCP to complete
                WaitIP();
            }
            else
            {
                throw new NotSupportedException("ERROR: there is no network interface configured.\r\nOpen the 'Edit Network Configuration' in Device Explorer and configure one.");
            }
        }

        static void WaitIP()
        {
            Debug.WriteLine("Waiting for IP...");

            while (true)
            {
                NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];
                if (ni.IPv4Address != null && ni.IPv4Address.Length > 0)
                {
                    if (ni.IPv4Address[0] != '0')
                    {
                        Debug.WriteLine($"We have an IP: {ni.IPv4Address}");
                        break;
                    }
                }

                Thread.Sleep(500);
            }
        }
    }
    public class SensorData
    {
        public float Distance { get; set; }
        public int Temp { get; set; }
    }
}
