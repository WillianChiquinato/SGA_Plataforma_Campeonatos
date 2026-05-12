using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Matches_Teams")]
public class MatchesTeams : BaseEntity
{
    [Column("match_id")]
    public int MatchId { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("side")]
    public string? Side { get; set; }

    [Column("score")]
    public int? Score { get; set; }

    [Column("is_winner")]
    public bool IsWinner { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(MatchId))]
    public Match? Match { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team? Team { get; set; }
}