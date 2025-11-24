using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_food.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key, ForeignKey("Usuario")] // mesmo Id do usuário
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Required, StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [StringLength(255)]
        public string Endereco { get; set; }

        [StringLength(100)]
        public string Cidade { get; set; }

        [StringLength(2)]
        public string Estado { get; set; }

        [Required, StringLength(255)]
        public string SenhaHash { get; set; } // senha criptografada

        public bool Bloqueado { get; set; } = false; // status ativo/bloqueado

        // 🔗 Relação inversa (Cliente → Usuario)
        public virtual Usuario Usuario { get; set; }
    }
}
