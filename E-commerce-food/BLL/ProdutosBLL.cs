using E_commerce_food.DAL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace E_commerce_food.BLL
{
    public class ProdutoBLL : IDisposable
    {
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoBLL()
        {
            _produtoRepository = new ProdutoRepository();
        }

        public List<Produto> ListarProdutos(string filtroCategoria = "", string busca = "")
        {
            var produtos = _produtoRepository.GetAll();

            if (!string.IsNullOrEmpty(filtroCategoria) && filtroCategoria != "Todos")
            {
                produtos = produtos
                    .Where(p => p.Categoria.Equals(filtroCategoria, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(busca))
            {
                string termo = busca.Trim().ToLower();
                produtos = produtos
                    .Where(p => p.Nome.ToLower().Contains(termo) ||
                                p.Descricao.ToLower().Contains(termo))
                    .ToList();
            }

            return produtos;
        }

        public bool CadastrarProduto(Produto novoProduto, out string mensagem)
        {
            if (string.IsNullOrWhiteSpace(novoProduto.Nome))
            {
                mensagem = "O nome do produto é obrigatório.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(novoProduto.Categoria))
            {
                mensagem = "A categoria deve ser selecionada.";
                return false;
            }

            if (novoProduto.Preco <= 0)
            {
                mensagem = "O preço deve ser maior que zero.";
                return false;
            }

            if (novoProduto.Estoque < 0)
                novoProduto.Estoque = 0;

            try
            {
                _produtoRepository.Add(novoProduto);
                _produtoRepository.Save();
                mensagem = "Produto cadastrado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao cadastrar produto: " + ex.Message;
                return false;
            }
        }

        public bool ExcluirProduto(int produtoId, out string mensagem)
        {
            try
            {
                _produtoRepository.Delete(produtoId);
                _produtoRepository.Save();
                mensagem = "Produto excluído com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao excluir produto: " + ex.Message;
                return false;
            }
        }

        public Produto ObterProdutoPorId(int id)
        {
            try
            {
                return _produtoRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar produto: " + ex.Message);
            }
        }

        public bool AtualizarProduto(Produto p, out string mensagem)
        {
            mensagem = "";

            try
            {
                Produto produtoDb = _produtoRepository.GetById(p.Id);

                if (produtoDb == null)
                {
                    mensagem = "Produto não encontrado.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(p.Nome))
                {
                    mensagem = "O nome do produto não pode ser vazio.";
                    return false;
                }

                if (p.Preco <= 0)
                {
                    mensagem = "O preço deve ser maior que zero.";
                    return false;
                }

                produtoDb.Nome = p.Nome;
                produtoDb.Descricao = p.Descricao;
                produtoDb.Categoria = p.Categoria;
                produtoDb.Preco = p.Preco;
                produtoDb.Estoque = p.Estoque;
                produtoDb.ImagemUrl = p.ImagemUrl;
                produtoDb.Disponivel = p.Disponivel;

                _produtoRepository.Update(produtoDb);
                _produtoRepository.Save();

                mensagem = "Produto atualizado com sucesso!";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = "Erro ao atualizar produto: " + ex.Message;
                return false;
            }
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
