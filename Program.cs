using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VetApi.Data;
using VetApi.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VetClinic API - FIAP",
        Version = "v1",
        Description = @"
## API RESTful para Clínica Veterinária

Desenvolvida com **ASP.NET Core 8**, **Oracle Database** e **Entity Framework Core**.

### Entidades
- **Tutores** — cadastro dos responsáveis pelos pets
- **Pets** — cadastro dos animais
- **Consultas** — agendamento e registro de consultas
- **Vacinações** — histórico de vacinas aplicadas
- **Exames** — exames solicitados e resultados

### Jornada do Pet
Use `GET /api/pets/{id}/jornada` para ver o histórico completo de um pet.

### Status de Consulta
`Agendada` | `Em andamento` | `Realizada` | `Cancelada`
        ",
        Contact = new OpenApiContact { Name = "FIAP", Url = new Uri("https://www.fiap.com.br") }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    c.EnableAnnotations();
    c.UseInlineDefinitionsForEnums();
});

var app = builder.Build();

// Aplica migrations automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var retries = 10;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso.");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"Oracle ainda nao disponivel. Tentativas restantes: {retries}. Erro: {ex.Message}");
            Thread.Sleep(10000);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VetClinic API v1");
    c.RoutePrefix = string.Empty;
    c.DefaultModelsExpandDepth(2);
    c.DisplayRequestDuration();
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
