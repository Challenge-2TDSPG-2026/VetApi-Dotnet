using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetApi.Models;

[Table("PETS")]
public class Pet
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("ESPECIE")]
    public string Especie { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("RACA")]
    public string? Raca { get; set; }

    [MaxLength(10)]
    [Column("SEXO")]
    public string? Sexo { get; set; }

    [Column("DATA_NASCIMENTO")]
    public DateTime? DataNascimento { get; set; }

    [Column("PESO")]
    public decimal? Peso { get; set; }

    [MaxLength(50)]
    [Column("COR")]
    public string? Cor { get; set; }

    [Column("ATIVO")]
    public bool Ativo { get; set; } = true;

    [Column("TUTOR_ID")]
    public int TutorId { get; set; }

    [ForeignKey(nameof(TutorId))]
    public Tutor Tutor { get; set; } = null!;

    [Column("CRIADO_EM")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [Column("ATUALIZADO_EM")]
    public DateTime? AtualizadoEm { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    public ICollection<Vacinacao> Vacinacoes { get; set; } = new List<Vacinacao>();
    public ICollection<Exame> Exames { get; set; } = new List<Exame>();
}
