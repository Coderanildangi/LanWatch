using Microsoft.EntityFrameworkCore;
using ServerAPI.Data;
using SharedDLL.Models;

namespace ServerAPI.Services
{
    public interface ITelemetryService { Task StoreAsync(TelemetryInfo s); }

    public class TelemetryService : ITelemetryService
    {
        private readonly AppDbContext _db;
        public TelemetryService(AppDbContext db) { _db = db; }
        public async Task StoreAsync(TelemetryInfo s)
        {
            var tr = new TelemetryRecord
            {
                ClientId = s.ClientId,
                TimestampUtc = s.TimestampUtc,
                CpuPercent = s.CpuPercent,
                RamUsedMB = s.RamUsedMB,
                RamTotalMB = s.RamTotalMB,
                NetInBytes = s.NetInBytes,
                NetOutBytes = s.NetOutBytes,
                RawJson = System.Text.Json.JsonSerializer.Serialize(s)
            };
            await _db.Telemetry.AddAsync(tr);
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.ClientId == s.ClientId);
            if (client == null) await _db.Clients.AddAsync(new ClientRecord { ClientId = s.ClientId, Hostname = s.ClientId, LastSeenUtc = DateTime.UtcNow });
            else client.LastSeenUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}
