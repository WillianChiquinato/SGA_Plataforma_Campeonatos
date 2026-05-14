using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SGA_Plataforma.Api.Hubs;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Contracts.Overwolf;
using SGA_Plataforma.Infrastructure.Models;
using System.Text.Json;

namespace SGA_Plataforma.Api.Controller;

[ApiController]
[Route("api/ingestion/overwolf")]
public sealed class OverwolfIngestionController : ControllerBase
{
    private readonly IMatchEventService _matchEventService;
    private readonly IHubContext<OverwolfEventsHub> _hubContext;

    public OverwolfIngestionController(
        IMatchEventService matchEventService,
        IHubContext<OverwolfEventsHub> hubContext)
    {
        _matchEventService = matchEventService;
        _hubContext = hubContext;
    }

    [HttpPost("events")]
    [HttpPost("~/events/ingest")]
    public async Task<IActionResult> IngestEvent([FromBody] OverwolfEventIngestionContract request, CancellationToken cancellationToken)
    {
        if (request.MatchId <= 0)
            return BadRequest(new { success = false, errors = new[] { "matchId deve ser maior que zero." } });

        var entity = new MatchEvent
        {
            MatchId = request.MatchId,
            MatchMapId = request.MatchMapId,
            RoundId = request.RoundId,
            GameId = request.GameId,
            EventType = string.IsNullOrWhiteSpace(request.EventType) ? "unknown" : request.EventType,
            EventTime = request.EventTime ?? DateTime.UtcNow,
            Provider = string.IsNullOrWhiteSpace(request.Provider) ? "overwolf" : request.Provider,
            ProviderEventId = request.ProviderEventId,
            IngestionSessionId = request.IngestionSessionId,
            SequenceNumber = request.SequenceNumber,
            ProcessingStatus = "received",
            Source = request.Source,
            DedupKey = request.DedupKey,
            PayloadJson = request.Payload.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null
                ? null
                : request.Payload.GetRawText()
        };

        var created = await _matchEventService.CreateAsync(entity, cancellationToken);
        if (!created.Success)
            return Conflict(created);

        var hubEvent = new OverwolfEventNotificationContract
        {
            Id = created.Result.Id,
            MatchId = created.Result.MatchId,
            MatchMapId = created.Result.MatchMapId,
            RoundId = created.Result.RoundId,
            GameId = created.Result.GameId,
            EventType = created.Result.EventType ?? "unknown",
            EventTime = created.Result.EventTime ?? DateTime.UtcNow,
            Provider = created.Result.Provider ?? "overwolf",
            ProviderEventId = created.Result.ProviderEventId,
            IngestionSessionId = created.Result.IngestionSessionId,
            SequenceNumber = created.Result.SequenceNumber,
            Source = created.Result.Source,
            Payload = request.Payload.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null
                ? null
                : JsonSerializer.Deserialize<object>(request.Payload.GetRawText())
        };

        await _hubContext.Clients.Group(OverwolfEventsHub.MatchGroup(created.Result.MatchId))
            .SendAsync("overwolfEventReceived", hubEvent, cancellationToken);

        await _hubContext.Clients.Group(OverwolfEventsHub.IngestionGroup)
            .SendAsync("overwolfEventReceived", hubEvent, cancellationToken);

        return Ok(created);
    }
}
