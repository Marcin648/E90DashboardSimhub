using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.IO.Ports;

namespace E90DashboardSimhub {
    [PluginDescription("E90 Dashboard for Simhub https://github.com/Marcin648/")]
    [PluginAuthor("Marcin648")]
    [PluginName("E90 Dashboard")]
    public class E90Dashboard : IPlugin, IOutputPlugin, IWPFSettings {
        public E90Telemetry telemetry = new E90Telemetry();
        public COBS cobs = new COBS();
        public SerialPort serialPort = new SerialPort();
        public PluginManager PluginManager { get; set; }
        public void DataUpdate(PluginManager pluginManager, ref GameData data) {
            if (data.GameRunning) {
                if (data.OldData != null && data.NewData != null) {
                    if (serialPort.IsOpen) {
                        telemetry.data.ignition = data.NewData.EngineIgnitionOn != 0;
                        telemetry.data.parkingLights = data.NewData.EngineIgnitionOn != 0;
                        // telemetry.data.dippedLights;
                        // telemetry.data.mainLights;
                        // telemetry.data.fogLights;
                        if(data.NewData.TurnIndicatorLeft == 1 && data.NewData.TurnIndicatorRight == 1) {
                            telemetry.data.blinkers = (byte)E90Telemetry.Blinkers.Hazzard;
                        } else if(data.NewData.TurnIndicatorLeft == 1){
                            telemetry.data.blinkers = (byte)E90Telemetry.Blinkers.Left;
                        } else if (data.NewData.TurnIndicatorRight == 1) {
                            telemetry.data.blinkers = (byte)E90Telemetry.Blinkers.Right;
                        } else {
                            telemetry.data.blinkers = (byte)E90Telemetry.Blinkers.Off;
                        }

                       

                        telemetry.data.handbrake = data.NewData.Handbrake != 0;
                        telemetry.data.rpm = Convert.ToUInt16(data.NewData.Rpms);
                        telemetry.data.speed = Convert.ToUInt16(data.NewData.SpeedKmh);
                        telemetry.data.fuel = Convert.ToUInt16(data.NewData.FuelPercent * 10.0);
                        telemetry.data.hour = (byte)data.NewData.PacketTime.Hour;
                        telemetry.data.minute = (byte)data.NewData.PacketTime.Minute;
                        telemetry.data.second = (byte)data.NewData.PacketTime.Second;
                        telemetry.data.day = (byte)data.NewData.PacketTime.Day;
                        telemetry.data.month = (byte)data.NewData.PacketTime.Month;
                        telemetry.data.year = (ushort)data.NewData.PacketTime.Year;

                        byte[] bytes = telemetry.GetBytes();
                        byte[] bytes_cobs = cobs.Encode(bytes);
                        serialPort.Write(bytes_cobs, 0, bytes_cobs.Length);
                        serialPort.Write("\0");
                    }
                }
            }
        }

        public void Connect(string port) {
            if (serialPort.IsOpen) {
                serialPort.Close();
            }
            serialPort.PortName = port;
            serialPort.WriteTimeout = 200;
            serialPort.Open();
        }
        public void Disconnect() {
            serialPort.Close();
        }
        public void Init(PluginManager pluginManager) {
            this.serialPort.BaudRate = 115200;
            SimHub.Logging.Current.Info("E90 Dashboard plugin init");
        }

        public void End(PluginManager pluginManager) {
            ;
        }

        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager) {
            return new E90DashboardControl(this);
        }

    }
}
