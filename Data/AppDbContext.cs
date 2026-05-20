using Microsoft.EntityFrameworkCore;
using VetApi.Models;

namespace VetApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Vacinacao> Vacinacoes => Set<Vacinacao>();
    public DbSet<Exame> Exames => Set<Exame>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tutor>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique().HasDatabaseName("IDX_TUTORES_EMAIL");
            e.HasIndex(x => x.Cpf).IsUnique().HasDatabaseName("IDX_TUTORES_CPF");
        });

        modelBuilder.Entity<Pet>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Peso).HasColumnType("NUMBER(8,2)");
            e.HasIndex(x => x.Especie).HasDatabaseName("IDX_PETS_ESPECIE");
            e.HasOne(x => x.Tutor)
             .WithMany(t => t.Pets)
             .HasForeignKey(x => x.TutorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Consulta>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Status).HasDatabaseName("IDX_CONSULTAS_STATUS");
            e.HasOne(x => x.Pet)
             .WithMany(p => p.Consultas)
             .HasForeignKey(x => x.PetId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vacinacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Pet)
             .WithMany(p => p.Vacinacoes)
             .HasForeignKey(x => x.PetId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Exame>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Pet)
             .WithMany(p => p.Exames)
             .HasForeignKey(x => x.PetId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
