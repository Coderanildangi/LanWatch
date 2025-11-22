using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedDLL.Models;
using System.Diagnostics;
using System.Management;
using System.Net.Http.Json;
using System.Net.NetworkInformation;

namespace Agent
{
    public class TelemetryWorker : BackgroundService
    {
        private readonly ILogger<TelemetryWorker> _logger;
        private readonly HttpClient _http;
        private readonly string _apiBase;

        public TelemetryWorker(ILogger<TelemetryWorker> logger)
        {
            _logger = logger;
            _http = new HttpClient();
            _apiBase = "https://localhost:7204/api/"; // TODO: move to config later
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Telemetry Worker Started");

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 1. Get static hardware info
                    var hw = HardwareInfoCollector.GetHardwareInfo();

                    // 2. Add dynamic CPU usage
                    hw.CpuUsagePercent = Math.Round(cpuCounter.NextValue(), 2);

                    // 3. RAM (Used + Total)
                    GetRamInfo(out ulong totalMb, out ulong freeMb);
                    hw.RAM_MB = (int)totalMb;
                    hw.RAM_UsedMB = (int)(totalMb - freeMb);

                    // 4. Network usage
                    var net = GetNetworkUsage();
                    hw.NetworkSentKB = net.sent;
                    hw.NetworkReceivedKB = net.recv;

                    // 5. Send telemetry to WebAPI
                    await _http.PostAsJsonAsync(_apiBase + "telemetry/report", hw, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Telemetry Error");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        // -------- RAM WMI --------
        private static void GetRamInfo(out ulong totalMb, out ulong freeMb)
        {
            ulong total = 0, free = 0;

            using var searcher = new ManagementObjectSearcher(
                "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

            foreach (var obj in searcher.Get())
            {
                total = (ulong)obj["TotalVisibleMemorySize"];
                free = (ulong)obj["FreePhysicalMemory"];
            }

            totalMb = total / 1024;
            freeMb = free / 1024;
        }

        // -------- NETWORK --------
        private (double sent, double recv) GetNetworkUsage()
        {
            try
            {
                var nic = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up)
                    .FirstOrDefault();

                if (nic == null) return (0, 0);

                var sent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", nic.Description).NextValue();
                var recv = new PerformanceCounter("Network Interface", "Bytes Received/sec", nic.Description).NextValue();

                return (sent / 1024.0, recv / 1024.0);
            }
            catch
            {
                return (0, 0);
            }
        }

    }
}
