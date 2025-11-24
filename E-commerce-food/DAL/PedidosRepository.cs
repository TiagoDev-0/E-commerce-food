using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace E_commerce_food.DAL
{
    public class ProdutoRepository : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public ProdutoRepository()
        {
            _context = new ApplicationDbContext();
        }

        public List<Produto> GetAll()
        {
            return _context.Produtos
                           .OrderBy(p => p.Nome)
                           .ToList();
        }

        public Produto GetById(int id)
        {
            return _context.Produtos.Find(id);
        }

        public void Add(Produto produto)
        {
            produto.DataCadastro = DateTime.Now;
            _context.Produtos.Add(produto);
        }

        public void Update(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Produto produto = _context.Produtos.Find(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
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
