using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Player_Match_stats")]
public class PlayerMatchStats : BaseEntity
{
    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("game_account_id")]
    public int? GameAccountId { get; set; }

    [Column("match_id")]
    public int MatchId { get; set; }

    [Column("match_map_id")]
    public int? MatchMapId { get; set; }

    [Column("kills")]
    public int Kills { get; set; }

    [Column("deaths")]
    public int Deaths { get; set; }

    [Column("assists")]
    public int Assists { get; set; }

    [Column("adr")]
    public decimal ADR { get; set; }

    [Column("hs_percentage")]
    public decimal HsPercentage { get; set; }

    [Column("first_kills")]
    public int FirstKills { get; set; }

    [Column("kast")]
    public decimal Kast { get; set; }

    [Column("acs")]
    public decimal ACS { get; set; }

    [Column("role_name")]
    public string? RoleName { get; set; }

    [Column("character_name")]
    public string? CharacterName { get; set; }

    [Column("stats_json")]
    public string? StatsJson { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(PlayerId))]
    public Player? Player { get; set; }

    [ForeignKey(nameof(GameAccountId))]
    public GameAccount? GameAccount { get; set; }

    [ForeignKey(nameof(MatchId))]
    public Match? Match { get; set; }

    [ForeignKey(nameof(MatchMapId))]
    public MatchMap? MatchMap { get; set; }
}