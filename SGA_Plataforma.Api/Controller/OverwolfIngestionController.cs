using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SGA_Plataforma.Api.Hubs;
using SGA_Plataforma.Api.Services;
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
    public async Task<IActionResult> IngestEvent([FromBody] OverwolfEventIngestionRequest request, CancellationToken cancellationToken)
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

        var hubEvent = new
        {
            id = created.Result.Id,
            matchId = created.Result.MatchId,
            matchMapId = created.Result.MatchMapId,
            roundId = created.Result.RoundId,
            gameId = created.Result.GameId,
            eventType = created.Result.EventType,
            eventTime = created.Result.EventTime,
            provider = created.Result.Provider,
            providerEventId = created.Result.ProviderEventId,
            ingestionSessionId = created.Result.IngestionSessionId,
            sequenceNumber = created.Result.SequenceNumber,
            source = created.Result.Source,
            payload = request.Payload.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null
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

public sealed class OverwolfEventIngestionRequest
{
    public int MatchId { get; set; }
    public int? MatchMapId { get; set; }
    public int? RoundId { get; set; }
    public int? GameId { get; set; }
    public string? EventType { get; set; }
    public DateTime? EventTime { get; set; }
    public string? Provider { get; set; }
    public string? ProviderEventId { get; set; }
    public string? IngestionSessionId { get; set; }
    public long? SequenceNumber { get; set; }
    public string? Source { get; set; }
    public string? DedupKey { get; set; }
    public JsonElement Payload { get; set; }
}