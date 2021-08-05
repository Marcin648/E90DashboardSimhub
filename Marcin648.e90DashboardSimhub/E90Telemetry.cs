using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace E90DashboardSimhub {
   public class E90Telemetry {

        public enum Blinkers {
            Off = 0,
            Left = 1,
            Right = 2,
            Hazzard = 3
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct E90TelemetryData {
            [MarshalAs(UnmanagedType.U1)]
            public bool ignition;
            [MarshalAs(UnmanagedType.U1)]
            public bool parkingLights;
            [MarshalAs(UnmanagedType.U1)]
            public bool dippedLights;
            [MarshalAs(UnmanagedType.U1)]
            public bool mainLights;
            [MarshalAs(UnmanagedType.U1)]
            public bool fogLights;
            public byte blinkers;
            [MarshalAs(UnmanagedType.U1)]
            public bool handbrake;
            public ushort rpm;
            public ushort speed;
            public ushort fuel;
            public byte hour;
            public byte minute;
            public byte second;
            public byte day;
            public byte month;
            public ushort year;
        }

        public E90TelemetryData data = new E90TelemetryData();

        private IntPtr dataPointer;
        private byte[] bytes;

        public E90Telemetry() {
            int size = Marshal.SizeOf(this.data);
            this.dataPointer = Marshal.AllocHGlobal(size);
            this.bytes = new byte[size];
        }

        ~E90Telemetry() {
            Marshal.FreeHGlobal(dataPointer);
        }

        public byte[] GetBytes() {
            Marshal.StructureToPtr(data, dataPointer, true);
            Marshal.Copy(dataPointer, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
