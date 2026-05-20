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
[Tags("Exames")]
public class ExamesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ExamesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>Lista todos os exames</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var exames = await _context.Exames.Include(e => e.Pet).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<ExameDto>>(exames));
    }

    /// <summary>Busca exame por ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ExameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var exame = await _context.Exames.Include(e => e.Pet).FirstOrDefaultAsync(e => e.Id == id);
        if (exame is null)
            return NotFound(new { message = $"Exame com ID {id} não encontrado." });

        return Ok(_mapper.Map<ExameDto>(exame));
    }

    /// <summary>Lista exames de um pet</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ExameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPet(int petId)
    {
        var existe = await _context.Pets.AnyAsync(p => p.Id == petId);
        if (!existe)
            return NotFound(new { message = "Pet não encontrado." });

        var exames = await _context.Exames
            .Include(e => e.Pet)
            .Where(e => e.PetId == petId)
            .OrderByDescending(e => e.DataExame)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ExameDto>>(exames));
    }

    /// <summary>Filtra exames por tipo</summary>
    [HttpGet("tipo/{tipo}")]
    [ProducesResponseType(typeof(IEnumerable<ExameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTipo(string tipo)
    {
        var exames = await _context.Exames
            .Include(e => e.Pet)
            .Where(e => e.TipoExame.ToLower().Contains(tipo.ToLower()))
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ExameDto>>(exames));
    }

    /// <summary>Registra um exame</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ExameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateExameDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var petExiste = await _context.Pets.AnyAsync(p => p.Id == dto.PetId);
        if (!petExiste)
            return NotFound(new { message = "Pet não encontrado." });

        var exame = _mapper.Map<Exame>(dto);
        _context.Exames.Add(exame);
        await _context.SaveChangesAsync();

        var created = await _context.Exames.Include(e => e.Pet).FirstAsync(e => e.Id == exame.Id);
        return CreatedAtAction(nameof(GetById), new { id = exame.Id }, _mapper.Map<ExameDto>(created));
    }

    /// <summary>Atualiza resultado de um exame</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExameDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exame = await _context.Exames.FindAsync(id);
        if (exame is null)
            return NotFound(new { message = $"Exame com ID {id} não encontrado." });

        _mapper.Map(dto, exame);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um exame</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var exame = await _context.Exames.FindAsync(id);
        if (exame is null)
            return NotFound(new { message = $"Exame com ID {id} não encontrado." });

        _context.Exames.Remove(exame);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
