namespace ControleGastosAPI.DTOs
{
    public class ResumoGeralDTO
    {
        public List<ResumoPessoaDTO> Pessoas { get; set; } = new();

        public decimal TotalReceitas { get; set; }

        public decimal TotalDespesas { get; set; }

        public decimal SaldoTotal { get; set; }
    }
}