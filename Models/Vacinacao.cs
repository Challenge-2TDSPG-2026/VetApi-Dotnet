using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetApi.Models;

[Table("VACINACOES")]
public class Vacinacao
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
    [Column("VACINA")]
    public string Vacina { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("FABRICANTE")]
    public string? Fabricante { get; set; }

    [MaxLength(100)]
    [Column("LOTE")]
    public string? Lote { get; set; }

    [Required]
    [Column("DATA_APLICACAO")]
    public DateTime DataAplicacao { get; set; }

    [Column("PROXIMA_DOSE")]
    public DateTime? ProximaDose { get; set; }

    [MaxLength(200)]
    [Column("VETERINARIO")]
    public string? Veterinario { get; set; }

    [Column("CRIADO_EM")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
