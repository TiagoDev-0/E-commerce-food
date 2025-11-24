using E_commerce_food.DALL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BCrypt.Net; // Adicione esta linha
namespace E_commerce_food.adm
{
    public class ClienteBLL
    {
        private ClienteRepository _repository;

      

        public ClienteBLL()
        {
            _repository = new ClienteRepository();
        }

        // Registrar novo cliente com validações
        public bool RegistrarCliente(string nome, string email, string senha, string telefone, string endereco, out string mensagem)
        {
            mensagem = "";

            // Validações
            if (string.IsNullOrWhiteSpace(nome))
            {
                mensagem = "Nome é obrigatório.";
                return false;
            }

            if (!ValidarEmail(email))
            {
                mensagem = "Email inválido.";
                return false;
            }

            if (_repository.BuscarPorEmail(email) != null)
            {
                mensagem = "Email já cadastrado.";
                return false;
            }

            if (senha.Length < 6)
            {
                mensagem = "Senha deve ter no mínimo 6 caracteres.";
                return false;
            }

            try
            {
                var cliente = new Cliente
                {
                    Nome = nome,
                    Email = email,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
                    Telefone = telefone,
                    Endereco = endereco
                };

                _repository.Inserir(cliente);
                mensagem = "Cliente cadastrado com sucesso!";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao cadastrar cliente: " + ex.Message;
                return false;
            }
        }

        // ⭐ MÉTODO DE LOGIN COMPLETO
        public Cliente RealizarLogin(string email, string senha, out string mensagem)
        {
            mensagem = "";

            try
            {
                var cliente = _repository.BuscarPorEmail(email);

                if (cliente == null)
                {
                    mensagem = "Email não encontrado.";
                    return null;
                }

                if (cliente.Bloqueado)
                {
                    mensagem = "Conta bloqueada. Entre em contato com o suporte.";
                    return null;
                }

                // Verificar senha com BCrypt
                if (!BCrypt.Net.BCrypt.Verify(senha, cliente.SenhaHash))
                {
                    mensagem = "Senha incorreta.";
                    return null;
                }

                mensagem = "Login realizado com sucesso!";
                return (Cliente)cliente;
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao realizar login: " + ex.Message;
                return null;
            }
        }

        // Atualizar dados do cliente
        public bool AtualizarCliente(int id, string nome, string telefone, string endereco, out string mensagem)
        {
            mensagem = "";

            try
            {
                var cliente = _repository.BuscarPorId(id);
                if (cliente == null)
                {
                    mensagem = "Cliente não encontrado.";
                    return false;
                }

                cliente.Nome = nome;
                cliente.Telefone = telefone;
                cliente.Endereco = endereco;

                _repository.Atualizar(cliente);
                mensagem = "Dados atualizados com sucesso!";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar: " + ex.Message;
                return false;
            }
        }

        // Listar todos os clientes
        public List<Cliente> ListarClientes()
        {
            return _repository.ListarTodos();
        }

        // Bloquear/Desbloquear cliente
        public void AlterarBloqueio(int id, bool bloquear)
        {
            _repository.AlterarBloqueio(id, bloquear);
        }

        // Validar email
        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}