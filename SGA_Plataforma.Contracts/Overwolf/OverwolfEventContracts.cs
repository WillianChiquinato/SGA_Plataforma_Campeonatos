using System.Text.Json;

namespace SGA_Plataforma.Contracts.Overwolf;

public sealed class OverwolfEventIngestionContract
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

public sealed class OverwolfEventNotificationContract
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int? MatchMapId { get; set; }
    public int? RoundId { get; set; }
    public int? GameId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime EventTime { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string? ProviderEventId { get; set; }
    public string? IngestionSessionId { get; set; }
    public long? SequenceNumber { get; set; }
    public string? Source { get; set; }
    public object? Payload { get; set; }
}

public static class OverwolfRedisChannels
{
    public const string MatchEvents = "overwolf:match-events";
}