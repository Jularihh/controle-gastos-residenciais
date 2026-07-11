using ControleGastosAPI.Data;
using ControleGastosAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TotaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TotaisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ResumoGeralDTO>> Get()
        {
            var pessoas = await _context.Pessoas
                .Include(p => p.Transacoes)
                .ToListAsync();

            var resumo = new ResumoGeralDTO();

            foreach (var pessoa in pessoas)
            {
                decimal receitas = pessoa.Transacoes
                    .Where(t => t.Tipo == "Receita")
                    .Sum(t => t.Valor);

                decimal despesas = pessoa.Transacoes
                    .Where(t => t.Tipo == "Despesa")
                    .Sum(t => t.Valor);

                resumo.Pessoas.Add(new ResumoPessoaDTO
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome,
                    TotalReceitas = receitas,
                    TotalDespesas = despesas,
                    Saldo = receitas - despesas
                });
            }

            resumo.TotalReceitas = resumo.Pessoas.Sum(p => p.TotalReceitas);
            resumo.TotalDespesas = resumo.Pessoas.Sum(p => p.TotalDespesas);
            resumo.SaldoTotal = resumo.TotalReceitas - resumo.TotalDespesas;

            return Ok(resumo);
        }
    }
}