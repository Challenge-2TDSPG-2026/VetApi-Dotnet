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
[Tags("Consultas")]
public class ConsultasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ConsultasController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>Lista todas as consultas</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var consultas = await _context.Consultas
            .Include(c => c.Pet).ThenInclude(p => p.Tutor)
            .OrderByDescending(c => c.DataConsulta)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ConsultaDto>>(consultas));
    }

    /// <summary>Busca consulta por ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ConsultaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var consulta = await _context.Consultas
            .Include(c => c.Pet).ThenInclude(p => p.Tutor)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (consulta is null)
            return NotFound(new { message = $"Consulta com ID {id} não encontrada." });

        return Ok(_mapper.Map<ConsultaDto>(consulta));
    }

    /// <summary>Filtra consultas por status</summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var consultas = await _context.Consultas
            .Include(c => c.Pet).ThenInclude(p => p.Tutor)
            .Where(c => c.Status.ToLower() == status.ToLower())
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ConsultaDto>>(consultas));
    }

    /// <summary>Filtra consultas por período</summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime de, [FromQuery] DateTime ate)
    {
        if (de > ate)
            return BadRequest(new { message = "Data inicial deve ser menor que a data final." });

        var consultas = await _context.Consultas
            .Include(c => c.Pet).ThenInclude(p => p.Tutor)
            .Where(c => c.DataConsulta >= de && c.DataConsulta <= ate)
            .OrderBy(c => c.DataConsulta)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ConsultaDto>>(consultas));
    }

    /// <summary>Lista consultas de um pet específico</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPet(int petId)
    {
        var existe = await _context.Pets.AnyAsync(p => p.Id == petId);
        if (!existe)
            return NotFound(new { message = "Pet não encontrado." });

        var consultas = await _context.Consultas
            .Include(c => c.Pet).ThenInclude(p => p.Tutor)
            .Where(c => c.PetId == petId)
            .OrderByDescending(c => c.DataConsulta)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ConsultaDto>>(consultas));
    }

    /// <summary>Agenda uma nova consulta</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConsultaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateConsultaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var petExiste = await _context.Pets.AnyAsync(p => p.Id == dto.PetId);
        if (!petExiste)
            return NotFound(new { message = "Pet não encontrado." });

        var consulta = _mapper.Map<Consulta>(dto);
        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();

        var created = await _context.Consultas
            .Include(c => c.Pet).ThenInclude(p => p.Tutor)
            .FirstAsync(c => c.Id == consulta.Id);

        return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, _mapper.Map<ConsultaDto>(created));
    }

    /// <summary>Atualiza consulta (diagnóstico, prescrição, status)</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateConsultaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta is null)
            return NotFound(new { message = $"Consulta com ID {id} não encontrada." });

        var statusValidos = new[] { "Agendada", "Realizada", "Cancelada", "Em andamento" };
        if (!statusValidos.Contains(dto.Status))
            return BadRequest(new { message = $"Status inválido. Aceitos: {string.Join(", ", statusValidos)}" });

        _mapper.Map(dto, consulta);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove uma consulta</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta is null)
            return NotFound(new { message = $"Consulta com ID {id} não encontrada." });

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
