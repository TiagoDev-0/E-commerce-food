using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_commerce_food.Admin
{
    // Esta página gerencia a visualização e atualização de status dos pedidos.
    public partial class Pedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO: Adicionar verificação de autenticação de administrador (TipoUsuario)
            if (Session["TipoUsuario"] == null || Session["TipoUsuario"].ToString().ToLower() != "administrador")
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CarregarPedidos();
                AtualizarEstatisticas();
            }
        }

        // ========================================
        // CARREGAR PEDIDOS (LEITURA - READ)
        // ========================================
        private void CarregarPedidos(string filtroStatus = "Todos", string busca = "")
        {
            try
            {
                using (PedidoBLL pedidoBLL = new PedidoBLL())
                {
                    // A BLL aplica os filtros, trazendo os dados reais
                    List<Pedido> pedidos = pedidoBLL.ListarPedidos(
                        ddlFiltroStatus.SelectedValue,
                        txtBusca.Text.Trim()
                    );

                    // Bind do GridView com a lista de entidades
                    gvPedidos.DataSource = pedidos;
                    gvPedidos.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar pedidos do banco de dados: {ex.Message}", "danger");
                gvPedidos.DataSource = null;
                gvPedidos.DataBind();
            }
        }

        // ========================================
        // ATUALIZAR ESTATÍSTICAS
        // ========================================
        private void AtualizarEstatisticas()
        {
            try
            {
                using (PedidoBLL pedidoBLL = new PedidoBLL())
                {
                    // Obtém todos os pedidos (sem filtro) para contagem
                    List<Pedido> pedidos = pedidoBLL.ListarPedidos("Todos", string.Empty);

                    // Contagens por Status (Regra de Negócio: Status Válidos)
                    countPendente.InnerText = pedidos.Count(p => p.Status == "Pendente").ToString();
                    countPreparo.InnerText = pedidos.Count(p => p.Status == "Em Preparo").ToString();
                    countEntregue.InnerText = pedidos.Count(p => p.Status == "Entregue").ToString();
                    countCancelado.InnerText = pedidos.Count(p => p.Status == "Cancelado").ToString();
                }
            }
            catch (Exception)
            {
                // Mantém estatísticas zeradas ou ignora erro para não quebrar a UI
                countPendente.InnerText = "N/A";
                countPreparo.InnerText = "N/A";
                countEntregue.InnerText = "N/A";
                countCancelado.InnerText = "N/A";
            }
        }

        // ========================================
        // EVENTOS DO GRIDVIEW (COMANDOS DO ADMIN)
        // ========================================
        protected void gvPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pedidoId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Detalhes")
            {
                // O pedidoId é usado (não declarado novamente)
                Response.Redirect($"PedidoDetalhes.aspx?id={pedidoId}");
                return;
            }

            // Define o novo status baseado no CommandName
            string novoStatus = e.CommandName == "Preparar" ? "Em Preparo" :
                                e.CommandName == "Entregar" ? "Entregue" :
                                e.CommandName == "Cancelar" ? "Cancelado" : string.Empty;

            if (!string.IsNullOrEmpty(novoStatus))
            {
                // O pedidoId é usado (não declarado novamente)
                string motivo = novoStatus == "Cancelar" ? "Cancelado pelo administrador" : string.Empty;
                AtualizarStatus(pedidoId, novoStatus, motivo);
            }

            CarregarPedidos();
            AtualizarEstatisticas();
        }

        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Como o DataSource agora é List<Pedido>, fazemos o DataBinder para acessar as propriedades
                Pedido pedido = (Pedido)e.Row.DataItem;
                string status = pedido.Status;

                // Encontrando os botões
                Button btnPreparar = (Button)e.Row.FindControl("btnPreparar");
                Button btnEntregar = (Button)e.Row.FindControl("btnEntregar");
                Button btnCancelar = (Button)e.Row.FindControl("btnCancelar");

                // Regras de Habilitação/Desabilitação dos Botões:
                if (status == "Entregue" || status == "Cancelado")
                {
                    // Pedidos concluídos não podem ser alterados
                    if (btnPreparar != null) btnPreparar.Enabled = false;
                    if (btnEntregar != null) btnEntregar.Enabled = false;
                    if (btnCancelar != null) btnCancelar.Enabled = false;
                }
                else if (status == "Em Preparo")
                {
                    // Pedido em preparo não pode voltar a Pendente, apenas Entregue ou Cancelado
                    if (btnPreparar != null) btnPreparar.Enabled = false;
                }
            }
        }

        // ========================================
        // ATUALIZAR STATUS (UPDATE)
        // ========================================
        private void AtualizarStatus(int pedidoId, string novoStatus, string motivoCancelamento)
        {
            try
            {
                using (PedidoBLL pedidoBLL = new PedidoBLL())
                {
                    string mensagem;
                    bool sucesso = pedidoBLL.AtualizarStatusPedido(
                        pedidoId,
                        novoStatus,
                        motivoCancelamento,
                        out mensagem
                    );

                    if (sucesso)
                    {
                        ExibirMensagem($"Status do Pedido #{pedidoId} alterado para '{novoStatus}' com sucesso!", "success");
                    }
                    else
                    {
                        // Mensagem de erro de negócio (ex: status inválido, pedido já cancelado)
                        ExibirMensagem($"Falha ao alterar status do Pedido #{pedidoId}: {mensagem}", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro interno ao processar a mudança de status: {ex.Message}", "danger");
            }
        }

        // ========================================
        // FILTROS E BUSCA
        // ========================================
        protected void ddlFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarPedidos(ddlFiltroStatus.SelectedValue, txtBusca.Text.Trim());
            AtualizarEstatisticas();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CarregarPedidos(ddlFiltroStatus.SelectedValue, txtBusca.Text.Trim());
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtBusca.Text = "";
            ddlFiltroStatus.SelectedIndex = 0;
            CarregarPedidos();
            AtualizarEstatisticas();
        }

        // ========================================
        // MÉTODOS AUXILIARES
        // ========================================
        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-custom alert-{tipo}";
            lblMsg.Text = mensagem;
        }

        protected string GetStatusClass(string status)
        {
            // Usado no ASPX para colorir as linhas ou badges
            return status.ToLower().Replace(" ", "");
        }
    }
}