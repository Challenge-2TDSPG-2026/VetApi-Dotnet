using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetApi.Models;

[Table("TUTORES")]
public class Tutor
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Column("EMAIL")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("TELEFONE")]
    public string? Telefone { get; set; }

    [MaxLength(14)]
    [Column("CPF")]
    public string? Cpf { get; set; }

    [MaxLength(500)]
    [Column("ENDERECO")]
    public string? Endereco { get; set; }

    [Column("ATIVO")]
    public bool Ativo { get; set; } = true;

    [Column("CRIADO_EM")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [Column("ATUALIZADO_EM")]
    public DateTime? AtualizadoEm { get; set; }

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
