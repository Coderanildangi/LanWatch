using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServerAPI.Data;
using ServerAPI.Hubs;
using ServerAPI.Services;
using SharedDLL.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly TelemetryService _svc;
        private readonly IHubContext<TelemetryHub> _hub;


        public TelemetryController(AppDbContext db, TelemetryService svc, IHubContext<TelemetryHub> hub)
        {
            _db = db; _svc = svc; _hub = hub;
        }


        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest([FromBody] List<TelemetryInfo> samples)
        {
            if (samples == null || !samples.Any()) return BadRequest();


            foreach (var s in samples)
            {
                await _svc.StoreAsync(s);
                await _hub.Clients.Group($"client:{s.ClientId}").SendAsync("TelemetryUpdated", s);
            }


            return Ok();
        }


        [HttpPost("hardware/register")]
        public async Task<IActionResult> RegisterHardware([FromBody] TelemetryInfo info)
        {
            if (info == null) return BadRequest();
            await _db.Hardware.AddAsync(new HardwareRecord { ClientId = info.ClientId, CollectedAtUtc = DateTime.UtcNow, RawJson = System.Text.Json.JsonSerializer.Serialize(info) });
            await _db.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("latest/{clientId}")]
        public async Task<IActionResult> Latest(string clientId)
        {
            var rec = await _db.Telemetry.Where(t => t.ClientId == clientId).OrderByDescending(t => t.TimestampUtc).FirstOrDefaultAsync();
            if (rec == null) return NotFound();
            return Ok(rec);
        }
    }
}
