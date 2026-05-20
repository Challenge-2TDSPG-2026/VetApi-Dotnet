using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetApi.Data;
using VetApi.DTOs;
using VetApi.Models;

namespace VetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Vacinações")]
public class VacinacoesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public VacinacoesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>Lista todas as vacinações</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VacinacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var vacinas = await _context.Vacinacoes.Include(v => v.Pet).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<VacinacaoDto>>(vacinas));
    }

    /// <summary>Busca vacinação por ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(VacinacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var vacina = await _context.Vacinacoes.Include(v => v.Pet).FirstOrDefaultAsync(v => v.Id == id);
        if (vacina is null)
            return NotFound(new { message = $"Vacinação com ID {id} não encontrada." });

        return Ok(_mapper.Map<VacinacaoDto>(vacina));
    }

    /// <summary>Lista vacinações de um pet</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<VacinacaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPet(int petId)
    {
        var existe = await _context.Pets.AnyAsync(p => p.Id == petId);
        if (!existe)
            return NotFound(new { message = "Pet não encontrado." });

        var vacinas = await _context.Vacinacoes
            .Include(v => v.Pet)
            .Where(v => v.PetId == petId)
            .OrderByDescending(v => v.DataAplicacao)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<VacinacaoDto>>(vacinas));
    }

    /// <summary>Lista vacinações com próxima dose vencendo até uma data</summary>
    [HttpGet("proximas-doses")]
    [ProducesResponseType(typeof(IEnumerable<VacinacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProximasDoses([FromQuery] DateTime? ate)
    {
        var limite = ate ?? DateTime.UtcNow.AddDays(30);

        var vacinas = await _context.Vacinacoes
            .Include(v => v.Pet)
            .Where(v => v.ProximaDose.HasValue && v.ProximaDose <= limite)
            .OrderBy(v => v.ProximaDose)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<VacinacaoDto>>(vacinas));
    }

    /// <summary>Registra uma vacinação</summary>
    [HttpPost]
    [ProducesResponseType(typeof(VacinacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateVacinacaoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var petExiste = await _context.Pets.AnyAsync(p => p.Id == dto.PetId);
        if (!petExiste)
            return NotFound(new { message = "Pet não encontrado." });

        var vacina = _mapper.Map<Vacinacao>(dto);
        _context.Vacinacoes.Add(vacina);
        await _context.SaveChangesAsync();

        var created = await _context.Vacinacoes.Include(v => v.Pet).FirstAsync(v => v.Id == vacina.Id);
        return CreatedAtAction(nameof(GetById), new { id = vacina.Id }, _mapper.Map<VacinacaoDto>(created));
    }

    /// <summary>Remove registro de vacinação</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var vacina = await _context.Vacinacoes.FindAsync(id);
        if (vacina is null)
            return NotFound(new { message = $"Vacinação com ID {id} não encontrada." });

        _context.Vacinacoes.Remove(vacina);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
