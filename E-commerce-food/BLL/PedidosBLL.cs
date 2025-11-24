using E_commerce_food.DAL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace E_commerce_food.BLL
{
    public class PedidoBLL : IDisposable
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly ProdutoBLL _produtoBLL;

        public PedidoBLL()
        {
            _pedidoRepository = new PedidoRepository();
            _produtoBLL = new ProdutoBLL();
        }

        private readonly string[] STATUS_VALIDOS = { "Pendente", "Em Preparo", "Entregue", "Cancelado" };
        
        public bool CriarPedido(Pedido novoPedido, List<ItemPedido> itens, out string mensagem)
        {

            if (itens == null || !itens.Any())
            {
                mensagem = "O pedido não pode ser vazio.";
                return false;
            }


            novoPedido.DataPedido = DateTime.Now;
            novoPedido.Status = "Pendente";

            try
            {
                novoPedido.Itens = itens;
                _pedidoRepository.Add(novoPedido);
                _pedidoRepository.Save();
                mensagem = "Pedido realizado com sucesso. Aguardando confirmação.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = $"Erro ao salvar o pedido: {ex.Message}";
                return false;
            }
        }
        public List<Pedido> ListarPedidos(string filtroStatus, string busca)
        {
            var pedidos = _pedidoRepository.GetAll();

            if (!string.IsNullOrEmpty(filtroStatus) && filtroStatus != "Todos")
            {
                pedidos = pedidos.Where(p => p.Status.Equals(filtroStatus, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(busca))
            {
                string termo = busca.Trim().ToLower();
                pedidos = pedidos.Where(p =>
                    p.Id.ToString().Contains(termo) ||
                    (p.Usuario != null && p.Usuario.Nome.ToLower().Contains(termo))
                ).ToList();
            }

            return pedidos;
        }

        public List<Pedido> ListarHistoricoCliente(int usuarioId)
        {
            var todosPedidos = _pedidoRepository.GetAll();
            return todosPedidos.Where(p => p.UsuarioId == usuarioId).ToList();
        }

        public bool AtualizarStatusPedido(int pedidoId, string novoStatus, string motivoCancelamento, out string mensagem)
        {
            Pedido pedido = _pedidoRepository.GetById(pedidoId);

            if (pedido == null)
            {
                mensagem = "Pedido não encontrado.";
                return false;
            }

            if (!STATUS_VALIDOS.Contains(novoStatus))
            {
                mensagem = "Status inválido. Use Pendente, Em Preparo, Entregue ou Cancelado.";
                return false;
            }

            if (pedido.Status == "Entregue" || pedido.Status == "Cancelado")
            {
                mensagem = $"Não é possível alterar o status de um pedido que já está '{pedido.Status}'.";
                return false;
            }

            if (novoStatus == "Cancelado" && string.IsNullOrEmpty(motivoCancelamento))
            {
                mensagem = "O motivo do cancelamento é obrigatório.";
                return false;
            }

            pedido.Status = novoStatus;
            pedido.MotivoCancelamento = motivoCancelamento;

            try
            {
                _pedidoRepository.Update(pedido);
                _pedidoRepository.Save();
                mensagem = "Status atualizado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = $"Erro de persistência ao atualizar status: {ex.Message}";
                return false;
            }
        }


        public void Dispose()
        {
            _pedidoRepository?.Dispose();
            _produtoBLL?.Dispose(); 
            GC.SuppressFinalize(this);
        }

        internal bool CriarPedido(int usuarioId, Dictionary<int, int> carrinho, string observacoes, out string mensagem)
        {
            if (carrinho == null || !carrinho.Any())
            {
                mensagem = "O pedido não pode ser vazio.";
                return false;
            }

            try
            {

                decimal totalCalculado = 0;
                var produtosParaAtualizar = new List<Produto>();
                var itensDoPedido = new List<ItemPedido>();

                foreach (var item in carrinho)
                {
                    int produtoId = item.Key;
                    int quantidade = item.Value;

                    Produto produto = _produtoBLL.ObterProdutoPorId(produtoId);

                    if (produto == null)
                    {
                        mensagem = $"O produto ID {produtoId} não foi encontrado.";
                        return false;
                    }
                    if (!produto.Disponivel)
                    {
                        mensagem = $"O produto '{produto.Nome}' não está mais disponível.";
                        return false;
                    }
                    if (produto.Estoque < quantidade)
                    {
                        mensagem = $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.Estoque}.";
                        return false;
                    }

                   
                    produto.Estoque -= quantidade;
                    produtosParaAtualizar.Add(produto);

                  
                    itensDoPedido.Add(new ItemPedido
                    {
                        ProdutoId = produtoId,
                        Quantidade = quantidade,
                        PrecoUnitario = produto.Preco 
                    });

                    totalCalculado += (produto.Preco * quantidade);
                }


                Pedido novoPedido = new Pedido
                {
                    UsuarioId = usuarioId,
                    DataPedido = DateTime.Now,
                    Status = "Pendente",
                    Observacoes = observacoes,
                    ValorTotal = totalCalculado,
                    Itens = itensDoPedido 
                };

                _pedidoRepository.Add(novoPedido);
                _pedidoRepository.Save();

                string msgEstoque;
                foreach (var produto in produtosParaAtualizar)
                {
                    _produtoBLL.AtualizarProduto(produto, out msgEstoque);
                }

                mensagem = "Pedido realizado com sucesso. Aguardando confirmação.";
                return true;
            }
            catch (Exception ex)
            {
                mensagem = $"Erro ao salvar o pedido: {ex.Message}";
                return false;
            }
        }
    }
}