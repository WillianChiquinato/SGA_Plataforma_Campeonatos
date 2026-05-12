using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Team_Match_Stats")]
public class TeamMatchStats : BaseEntity
{
    [Required]
    [Column("team_id")]
    public int TeamId { get; set; }

    [Required]
    [Column("match_id")]
    public int MatchId { get; set; }

    [Column("rounds_won")]
    public int RoundsWon { get; set; }

    [Column("rounds_lost")]
    public int RoundsLost { get; set; }

    [Column("plants")]
    public int Plants { get; set; }

    [Column("defuses")]
    public int Defuses { get; set; }

    [ForeignKey("TeamId")]
    public Team Team { get; set; } = null!;

    [ForeignKey("MatchId")]
    public Match Match { get; set; } = null!;
}