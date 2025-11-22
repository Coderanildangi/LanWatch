using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDLL.Models
{
    public class TelemetryInfo
    {
        public string ClientId { get; set; } = string.Empty;
        public DateTime TimestampUtc { get; set; }
        public double CpuPercent { get; set; }
        public int RamUsedMB { get; set; }
        public int RamTotalMB { get; set; }
        public long NetInBytes { get; set; }
        public long NetOutBytes { get; set; }
        public List<DiskLoad>? Disks { get; set; }
    }

    public class DiskLoad
    {
        public string Drive { get; set; } = string.Empty;
        public long UsedMB { get; set; }
        public long TotalMB { get; set; }
    }
}
