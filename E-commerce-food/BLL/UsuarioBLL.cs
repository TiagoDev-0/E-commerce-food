using E_commerce_food.DAL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using BCrypt.Net;

namespace E_commerce_food.BLL
{
    public class UsuarioBLL : IDisposable
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioBLL()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        public Usuario Autenticar(string email, string senha)
        {
            Usuario usuario = _usuarioRepository.GetByEmail(email);

            if (usuario == null)
            {
                return null;
            }

            if (usuario.Bloqueado)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            {
                return usuario;
            }
            else
            {
                return null;
            }
        }

        public bool CadastrarCliente(Usuario novoCliente, string senhaEmTextoPuro, out string mensagem)
        {
            if (_usuarioRepository.EmailExists(novoCliente.Email))
            {
                mensagem = "Este email já está cadastrado.";
                return false;
            }

            if (senhaEmTextoPuro.Length < 6)
            {
                mensagem = "A senha deve ter no mínimo 6 caracteres.";
                return false;
            }

            novoCliente.TipoUsuario = "Cliente";
            novoCliente.Bloqueado = false;
            novoCliente.Ativo = true;
            novoCliente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaEmTextoPuro);

            try
            {
                _usuarioRepository.Add(novoCliente);
                _usuarioRepository.Save();
                mensagem = "Cadastro realizado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = $"Erro ao cadastrar usuário: {ex.Message}";
                return false;
            }
        }

        public List<Usuario> ListarClientes()
        {
            return _usuarioRepository.GetAllClients();
        }

        public bool AlternarBloqueioCliente(int clienteId, out string mensagem)
        {
            Usuario cliente = _usuarioRepository.GetById(clienteId);

            if (cliente == null || cliente.TipoUsuario != "Cliente")
            {
                mensagem = "Cliente não encontrado ou acesso negado.";
                return false;
            }

            cliente.Bloqueado = !cliente.Bloqueado;

            try
            {
                _usuarioRepository.Update(cliente);
                _usuarioRepository.Save();
                mensagem = cliente.Bloqueado ? "Cliente bloqueado com sucesso." : "Cliente desbloqueado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = $"Erro ao alterar status do cliente: {ex.Message}";
                return false;
            }
        }

        public bool AtualizarPerfil(Usuario dadosAtualizados, out string mensagem)
        {
            Usuario usuarioExistente = _usuarioRepository.GetById(dadosAtualizados.Id);

            if (usuarioExistente == null)
            {
                mensagem = "Usuário não encontrado.";
                return false;
            }

            usuarioExistente.Nome = dadosAtualizados.Nome;
            usuarioExistente.Telefone = dadosAtualizados.Telefone;
            usuarioExistente.Endereco = dadosAtualizados.Endereco;

            try
            {
                _usuarioRepository.Update(usuarioExistente);
                _usuarioRepository.Save();
                mensagem = "Perfil atualizado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = $"Erro ao atualizar perfil: {ex.Message}";
                return false;
            }
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return _usuarioRepository.GetById(id);
        }

        public void Dispose()
        {
            _usuarioRepository?.Dispose();
            GC.SuppressFinalize(this);
        }

        public Usuario ObterUsuarioPorEmail(string email)
        {
            return _usuarioRepository.GetByEmail(email);
        }

        public bool AlterarSenha(int usuarioId, string senhaAtual, string novaSenha, out string mensagem)
        {
            if (novaSenha.Length < 6)
            {
                mensagem = "A nova senha deve ter no mínimo 6 caracteres.";
                return false;
            }

            try
            {
                Usuario usuario = _usuarioRepository.GetById(usuarioId);
                if (usuario == null)
                {
                    mensagem = "Usuário não encontrado. Não foi possível alterar a senha.";
                    return false;
                }

                bool senhaAtualValida = BCrypt.Net.BCrypt.Verify(senhaAtual, usuario.SenhaHash);

                if (!senhaAtualValida)
                {
                    mensagem = "A senha atual está incorreta.";
                    return false;
                }

                string novoHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);

                usuario.SenhaHash = novoHash;
                _usuarioRepository.Update(usuario);
                _usuarioRepository.Save();

                mensagem = "Senha alterada com sucesso!";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = "Erro inesperado ao alterar a senha: " + ex.Message;
                return false;
            }
        }
    }
}
