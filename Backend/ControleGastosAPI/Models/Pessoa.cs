using System.ComponentModel.DataAnnotations;

namespace ControleGastosAPI.Models
{
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int Idade { get; set; }

        public List<Transacao> Transacoes { get; set; } = new();
    }
}