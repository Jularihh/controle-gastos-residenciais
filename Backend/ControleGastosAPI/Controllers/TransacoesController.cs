using ControleGastosAPI.Data;
using ControleGastosAPI.DTOs;
using ControleGastosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacoesController(AppDbContext context)
        {
            _context = context;
        }

        // Lista todas as transações
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacao>>> Get()
        {
            return await _context.Transacoes
                .Include(t => t.Pessoa)
                .ToListAsync();
        }

        // Cadastra uma transação
        [HttpPost]
        public async Task<ActionResult> Post(TransacaoDTO dto)
        {
            var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);

            if (pessoa == null)
                return BadRequest("Pessoa não encontrada.");

            if (pessoa.Idade < 18 &&
                dto.Tipo.Equals("Receita", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Menores de idade só podem cadastrar despesas.");
            }

            var transacao = new Transacao
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                PessoaId = dto.PessoaId
            };

            _context.Transacoes.Add(transacao);

            await _context.SaveChangesAsync();

            return Ok(transacao);
        }
    }
}