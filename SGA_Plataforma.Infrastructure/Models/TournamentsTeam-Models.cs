using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Tournament_Team")]
public class TournamentTeam : BaseEntity
{
    [Column("tournament_id")]
    public int TournamentId { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("status_id")]
    public int? StatusId { get; set; }

    [Column("seed")]
    public int? Seed { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(TournamentId))]
    public Tournament? Tournament { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team? Team { get; set; }

    [ForeignKey(nameof(StatusId))]
    public Status? Status { get; set; }
}