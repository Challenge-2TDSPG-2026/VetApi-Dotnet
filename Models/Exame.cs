using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetApi.Models;

[Table("EXAMES")]
public class Exame
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
    [Column("TIPO_EXAME")]
    public string TipoExame { get; set; } = string.Empty;

    [Required]
    [Column("DATA_EXAME")]
    public DateTime DataExame { get; set; }

    [MaxLength(2000)]
    [Column("RESULTADO")]
    public string? Resultado { get; set; }

    [MaxLength(200)]
    [Column("LABORATORIO")]
    public string? Laboratorio { get; set; }

    [MaxLength(200)]
    [Column("VETERINARIO")]
    public string? Veterinario { get; set; }

    [Column("CRIADO_EM")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [Column("ATUALIZADO_EM")]
    public DateTime? AtualizadoEm { get; set; }
}
