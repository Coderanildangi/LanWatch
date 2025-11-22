using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime.Versioning; // Add this for SupportedOSPlatform attribute
// Add reference to System.Management in your project if you want to use ManagementObjectSearcher
using System.Management;

namespace SharedDLL.Models
{
    public static class HardwareInfoCollector
    {
        public static HardwareInfo GetHardwareInfo()
        {
            return new HardwareInfo
            {
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion.ToString(),
                CPU = GetCPU(),
                GPU = GetGPU(),
                RAM_MB = GetTotalRamMB(),
                Disks = GetDisks()
            };
        }

        [SupportedOSPlatform("windows")]
        private static string GetCPU()
        {
            if (!OperatingSystem.IsWindows())
                return "Unknown CPU";

            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            return searcher.Get().Cast<ManagementObject>()
                .FirstOrDefault()?["Name"]?.ToString() ?? "Unknown CPU";
        }

        private static string GetGPU()
        {
            if (!OperatingSystem.IsWindows())
                return "Unknown CPU";

            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
            return searcher.Get().Cast<ManagementObject>()
                .FirstOrDefault()?["Name"]?.ToString() ?? "Unknown GPU";
        }

        private static int GetTotalRamMB()
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            if (obj["TotalPhysicalMemory"] is ulong totalMemory)
                            {
                                return (int)(totalMemory / 1024 / 1024);
                            }
                        }
                    }
                }
                catch
                {
                    // Ignore and fallback
                }
            }
            // Fallback for non-Windows or if above fails
            return 0;
        }

        private static List<DiskInfo> GetDisks()
        {
            return DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(d => new DiskInfo
                {
                    Name = d.Name,
                    TotalGB = Math.Round(d.TotalSize / 1024d / 1024d / 1024d, 2),
                    FreeGB = Math.Round(d.AvailableFreeSpace / 1024d / 1024d / 1024d, 2)
                })
                .ToList();
        }
    }
}
