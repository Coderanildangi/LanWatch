using Microsoft.EntityFrameworkCore;

namespace ServerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<ClientRecord> Clients => Set<ClientRecord>();
        public DbSet<TelemetryRecord> Telemetry => Set<TelemetryRecord>();
        public DbSet<HardwareRecord> Hardware => Set<HardwareRecord>();
    }

    public class ClientRecord 
    { 
        public int Id { get; set; } 
        public string ClientId { get; set; } = string.Empty; 
        public string? Hostname { get; set; } 
        public DateTime LastSeenUtc { get; set; } 
    }

    public class TelemetryRecord 
    { 
        public int Id { get; set; } 
        public string ClientId { get; set; } = string.Empty; 
        public DateTime TimestampUtc { get; set; } 
        public double CpuPercent { get; set; } 
        public int RamUsedMB { get; set; } 
        public int RamTotalMB { get; set; } 
        public long NetInBytes { get; set; } 
        public long NetOutBytes { get; set; } 
        public string? RawJson { get; set; } 
    }

    public class HardwareRecord 
    { 
        public int Id { get; set; } 
        public string ClientId { get; set; } = string.Empty; 
        public DateTime CollectedAtUtc { get; set; } 
        public string? RawJson { get; set; } 
    }
}
