using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using VetApi.Data;

#nullable disable

namespace VetApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("VetApi.Models.Tutor", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("NUMBER(10)").HasColumnName("ID");
                b.Property<string>("Nome").IsRequired().HasMaxLength(200).HasColumnName("NOME");
                b.Property<string>("Email").IsRequired().HasMaxLength(200).HasColumnName("EMAIL");
                b.Property<string>("Telefone").HasMaxLength(20).HasColumnName("TELEFONE");
                b.Property<string>("Cpf").HasMaxLength(14).HasColumnName("CPF");
                b.Property<string>("Endereco").HasMaxLength(500).HasColumnName("ENDERECO");
                b.Property<bool>("Ativo").HasColumnName("ATIVO").HasDefaultValue(true);
                b.Property<DateTime>("CriadoEm").HasColumnName("CRIADO_EM");
                b.Property<DateTime?>("AtualizadoEm").HasColumnName("ATUALIZADO_EM");
                b.HasKey("Id");
                b.ToTable("TUTORES");
            });

            modelBuilder.Entity("VetApi.Models.Pet", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("NUMBER(10)").HasColumnName("ID");
                b.Property<string>("Nome").IsRequired().HasMaxLength(100).HasColumnName("NOME");
                b.Property<string>("Especie").IsRequired().HasMaxLength(50).HasColumnName("ESPECIE");
                b.Property<string>("Raca").HasMaxLength(100).HasColumnName("RACA");
                b.Property<string>("Sexo").HasMaxLength(10).HasColumnName("SEXO");
                b.Property<DateTime?>("DataNascimento").HasColumnName("DATA_NASCIMENTO");
                b.Property<decimal?>("Peso").HasColumnType("NUMBER(8,2)").HasColumnName("PESO");
                b.Property<string>("Cor").HasMaxLength(50).HasColumnName("COR");
                b.Property<bool>("Ativo").HasColumnName("ATIVO").HasDefaultValue(true);
                b.Property<int>("TutorId").HasColumnName("TUTOR_ID");
                b.Property<DateTime>("CriadoEm").HasColumnName("CRIADO_EM");
                b.Property<DateTime?>("AtualizadoEm").HasColumnName("ATUALIZADO_EM");
                b.HasKey("Id");
                b.ToTable("PETS");
            });
#pragma warning restore 612, 618
        }
    }
}
