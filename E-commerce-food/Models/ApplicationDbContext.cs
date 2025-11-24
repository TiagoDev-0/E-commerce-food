using System;
using System.Configuration;
using System.Data.Entity;

namespace E_commerce_food.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base(ConfigurationManager.ConnectionStrings["FastFoodConnection"]?.ConnectionString
                ?? throw new Exception("String de conexão 'FastFoodConnection' não encontrada no Web.config."))
        {
        }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }

    }

}

