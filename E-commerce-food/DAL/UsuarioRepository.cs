using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace E_commerce_food.DAL
{
    public class UsuarioRepository : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository()
        {
            _context = new ApplicationDbContext();
        }

        public void Add(Usuario usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            _context.Usuarios.Add(usuario);
        }

        public Usuario GetById(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public Usuario GetByEmail(string email)
        {
            return _context.Usuarios
                           .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public List<Usuario> GetAllClients()
        {
            return _context.Usuarios
                           .Where(u => u.TipoUsuario == "Cliente")
                           .OrderBy(u => u.Nome)
                           .ToList();
        }

        public bool EmailExists(string email)
        {
            return _context.Usuarios.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
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
