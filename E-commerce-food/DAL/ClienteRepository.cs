using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace E_commerce_food.DALL
{
    internal class ClienteRepository
    {
        private readonly string connectionString;

        public ClienteRepository()
        {
            // Garante que a string de conexão exista
            connectionString = ConfigurationManager.ConnectionStrings["FastFoodConnection"]?.ConnectionString
                ?? throw new Exception("String de conexão 'FastFoodConnection' não encontrada no Web.config.");
        }

        internal void AlterarBloqueio(int id, bool bloquear)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Cliente SET Bloqueado = @Bloqueado WHERE Id = @Id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    comando.Parameters.AddWithValue("@Bloqueado", bloquear);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        internal void Atualizar(Cliente cliente)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string sql = @"
                    UPDATE Cliente 
                    SET Nome = @Nome, Telefone = @Telefone, Endereco = @Endereco
                    WHERE Id = @Id";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Id", cliente.Id);
                    comando.Parameters.AddWithValue("@Nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@Endereco", cliente.Endereco);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        internal Cliente BuscarPorEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, Nome, Email, SenhaHash, Telefone, Endereco, Bloqueado FROM Cliente WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new Cliente
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nome = dr["Nome"].ToString(),
                        Email = dr["Email"].ToString(),
                        SenhaHash = dr["SenhaHash"].ToString(), // ESSENCIAL 👈
                        Telefone = dr["Telefone"].ToString(),
                        Endereco = dr["Endereco"].ToString(),
                        Bloqueado = dr["Bloqueado"] != DBNull.Value && Convert.ToBoolean(dr["Bloqueado"])
                    };
                }
            }
                return null;
            }

        internal Cliente BuscarPorId(int id)
        {
            Cliente cliente = null;

            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Cliente WHERE Id = @Id";
                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    conexao.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = MapearCliente(reader);
                        }
                    }
                }
            }

            return cliente;
        }

        private Cliente MapearCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString(),
                Email = reader["Email"].ToString(),
                Telefone = reader["Telefone"].ToString(),
                Endereco = reader["Endereco"].ToString(),
                SenhaHash = reader["SenhaHash"].ToString().Trim(), // remove espaços extras
                Bloqueado = Convert.ToBoolean(reader["Bloqueado"])
            };
        }

        internal void Inserir(Cliente cliente)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO Cliente (Nome, Email, Telefone, Endereco, SenhaHash, Bloqueado)
                    VALUES (@Nome, @Email, @Telefone, @Endereco, @SenhaHash, 0)";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@Email", cliente.Email);
                    comando.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                    comando.Parameters.AddWithValue("@SenhaHash", cliente.SenhaHash);

                    conexao.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        internal List<Cliente> ListarTodos()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Cliente";

                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    conexao.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(MapearCliente(reader));
                        }
                    }
                }
            }

            return clientes;
        }
    }
}
