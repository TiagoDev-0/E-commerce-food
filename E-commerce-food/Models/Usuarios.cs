using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_food.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Required, StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(255)]
        public string SenhaHash { get; set; } // 🔒 Senha criptografada com BCrypt

        [StringLength(20)]
        public string TipoUsuario { get; set; } = "Cliente"; // 👤 Pode ser "Administrador" ou "Cliente"

        [StringLength(20)]
        public string Telefone { get; set; } // 📞 Telefone do usuário

        [StringLength(200)]
        public string Endereco { get; set; } // 🏠 Endereço completo

        public DateTime DataCadastro { get; set; }

        public bool Bloqueado { get; set; } = false; // 🚫 Usuário bloqueado ou não

        public bool Ativo { get; set; } = true; // ✅ Ativo por padrão

        
    }
}
