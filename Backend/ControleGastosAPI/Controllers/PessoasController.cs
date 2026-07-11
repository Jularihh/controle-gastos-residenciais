using ControleGastosAPI.Data;
using ControleGastosAPI.DTOs;
using ControleGastosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PessoasController(AppDbContext context)
        {
            _context = context;
        }

        // Lista todas as pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> Get()
        {
            return await _context.Pessoas.ToListAsync();
        }

        // Cadastra uma pessoa
        [HttpPost]
        public async Task<ActionResult<Pessoa>> Post(PessoaDTO dto)
        {
            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Idade = dto.Idade
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return Ok(pessoa);
        }

        // Exclui uma pessoa e suas transações
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return Ok("Pessoa removida com sucesso.");
        }
    }
}