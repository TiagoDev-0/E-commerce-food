using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_food.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [Required]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Required, StringLength(50)]
        public string Status { get; set; } = "Pendente";

        [StringLength(200)]
        public string MotivoCancelamento { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
       
        public decimal ValorTotal { get; set; }

        [StringLength(300)]
        public string Observacoes { get; set; }

        public virtual ICollection<ItemPedido> Itens { get; set; }

        [NotMapped]
        public object Total { get; internal set; }
    }
}
