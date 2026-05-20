using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetApi.Models;

[Table("CONSULTAS")]
public class Consulta
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("PET_ID")]
    public int PetId { get; set; }

    [ForeignKey(nameof(PetId))]
    public Pet Pet { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [Column("VETERINARIO")]
    public string Veterinario { get; set; } = string.Empty;

    [Required]
    [Column("DATA_CONSULTA")]
    public DateTime DataConsulta { get; set; }

    [MaxLength(2000)]
    [Column("MOTIVO")]
    public string? Motivo { get; set; }

    [MaxLength(2000)]
    [Column("DIAGNOSTICO")]
    public string? Diagnostico { get; set; }

    [MaxLength(2000)]
    [Column("PRESCRICAO")]
    public string? Prescricao { get; set; }

    [MaxLength(50)]
    [Column("STATUS")]
    public string Status { get; set; } = "Agendada";

    [Column("CRIADO_EM")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [Column("ATUALIZADO_EM")]
    public DateTime? AtualizadoEm { get; set; }
}
