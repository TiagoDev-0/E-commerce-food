using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace E_commerce_food.Models
{
    public class UsuarioBLL
    {
        // 🔹 Registrar novo usuário
        public bool RegistrarUsuario(string nome, string email, string senha, string telefone, string endereco, out string mensagem)
        {
            mensagem = "";

            // ✅ Validações básicas
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "O nome é obrigatório.";
                return false;
            }

            if (!ValidarEmail(email))
            {
                mensagem = "E-mail inválido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(senha) || senha.Length < 6)
            {
                mensagem = "A senha deve ter no mínimo 6 caracteres.";
                return false;
            }

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    // Verifica se e-mail já existe
                    if (db.Usuarios.Any(u => u.Email == email))
                    {
                        mensagem = "E-mail já cadastrado.";
                        return false;
                    }

                    var usuario = new Usuario
                    {
                        Nome = nome,
                        Email = email,
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
                        Telefone = telefone,
                        Endereco = endereco,
                        Bloqueado = false,
                        Ativo = true,
                        DataCadastro = DateTime.Now
                    };

                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    mensagem = "Usuário cadastrado com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao cadastrar usuário: " + ex.Message;
                return false;
            }
        }

        // 🔹 Login
        public Usuario RealizarLogin(string email, string senha, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email);

                    if (usuario == null)
                    {
                        mensagem = "E-mail ou senha incorretos.";
                        return null;
                    }

                    if (usuario.Bloqueado)
                    {
                        mensagem = "Conta bloqueada. Contate o administrador.";
                        return null;
                    }

                    if (!usuario.Ativo)
                    {
                        mensagem = "Usuário desativado.";
                        return null;
                    }

                    bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);
                    if (!senhaValida)
                    {
                        mensagem = "E-mail ou senha incorretos.";
                        return null;
                    }

                    mensagem = "Login realizado com sucesso!";
                    return usuario;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao realizar login: " + ex.Message;
                return null;
            }
        }

        // 🔹 Atualizar informações básicas
        public bool AtualizarUsuario(int id, string nome, string telefone, string endereco, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.Id == id);
                    if (usuario == null)
                    {
                        mensagem = "Usuário não encontrado.";
                        return false;
                    }

                    usuario.Nome = nome;
                    usuario.Telefone = telefone;
                    usuario.Endereco = endereco;

                    db.SaveChanges();
                    mensagem = "Dados atualizados com sucesso!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar usuário: " + ex.Message;
                return false;
            }
        }

        // 🔹 Listar todos os usuários
        public List<Usuario> ListarUsuarios()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Usuarios.OrderBy(u => u.Nome).ToList();
            }
        }

        // 🔹 Bloquear ou desbloquear usuário
        public bool AlterarBloqueio(int id, bool bloquear, out string mensagem)
        {
            mensagem = "";

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.Id == id);
                    if (usuario == null)
                    {
                        mensagem = "Usuário não encontrado.";
                        return false;
                    }

                    usuario.Bloqueado = bloquear;
                    db.SaveChanges();

                    mensagem = bloquear ? "Usuário bloqueado com sucesso." : "Usuário desbloqueado com sucesso.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao alterar bloqueio: " + ex.Message;
                return false;
            }
        }

        // 🔹 Validação de e-mail
        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // 🔹 Geração de hash de senha (caso precise usar fora do cadastro)
        public static string GerarHash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
    }
}
