using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_food.Models
{
    [Table("Produtos")]
    public class Produto
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        [Required]
        [StringLength(50)]
        public string Categoria { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [StringLength(200)]
        public string ImagemUrl { get; set; }

        public int Estoque { get; set; }

        public bool Disponivel { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual ICollection<ItemPedido> ItensPedido { get; set; }
    }
}