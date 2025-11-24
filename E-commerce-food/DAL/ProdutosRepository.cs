using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace E_commerce_food.DAL
{
    public class PedidoRepository : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository()
        {
            _context = new ApplicationDbContext();
        }

        public void Add(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public List<Pedido> GetAll()
        {
            return _context.Pedidos
                           .Include(p => p.Usuario)
                           .Include(p => p.Itens)
                           .OrderByDescending(p => p.DataPedido)
                           .ToList();
        }

        public Pedido GetById(int id)
        {
            return _context.Pedidos
                           .Include(p => p.Usuario)
                           .Include(p => p.Itens)
                           .FirstOrDefault(p => p.Id == id);
        }

        public void Update(Pedido pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
