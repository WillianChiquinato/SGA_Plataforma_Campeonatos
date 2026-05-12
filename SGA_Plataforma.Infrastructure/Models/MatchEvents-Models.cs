using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Match_Events")]
public class MatchEvent : BaseEntity
{
    [Column("match_id")]
    public int MatchId { get; set; }

    [Column("match_map_id")]
    public int? MatchMapId { get; set; }

    [Column("round_id")]
    public int? RoundId { get; set; }

    [Column("game_id")]
    public int? GameId { get; set; }
    
    [Column("event_type")]
    public string? EventType { get; set; }

    [Column("event_time")]
    public DateTime? EventTime { get; set; }

    [Column("provider")]
    public string? Provider { get; set; }

    [Column("provider_event_id")]
    public string? ProviderEventId { get; set; }

    [Column("ingestion_session_id")]
    public string? IngestionSessionId { get; set; }

    [Column("sequence_number")]
    public long? SequenceNumber { get; set; }

    [Column("processing_status")]
    public string? ProcessingStatus { get; set; }

    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [Column("dedup_key")]
    public string? DedupKey { get; set; }

    [Column("source")]
    public string? Source { get; set; }

    [Column("payload_json")]
    public string? PayloadJson { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(MatchId))]
    public Match? Match { get; set; }

    [ForeignKey(nameof(MatchMapId))]
    public MatchMap? MatchMap { get; set; }

    [ForeignKey(nameof(RoundId))]
    public Round? Round { get; set; }

    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }
}