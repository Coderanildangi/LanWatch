using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDLL.Models
{
    public class HardwareInfo
    {
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public int RAM_MB { get; set; }
        public string Motherboard { get; set; }
        public string BIOS { get; set; }
        public List<DiskInfo> Disks { get; set; }

        // dynamic telemetry
        public double CpuUsagePercent { get; set; }
        public int RAM_UsedMB { get; set; }
        public double NetworkSentKB { get; set; }
        public double NetworkReceivedKB { get; set; }
    }

    public class DiskInfo
    {
        public string Name { get; set; }
        public double TotalGB { get; set; }
        public double FreeGB { get; set; }
    }
}
