using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_commerce_food.Cliente
{
    public partial class MeusPedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            AtualizarHeader();

            if (!IsPostBack)
            {
                // Verifica se veio uma mensagem do Carrinho (Sucesso)
                string msg = Request.QueryString["msg"];
                if (!string.IsNullOrEmpty(msg))
                {
                    lblMsg.Text = msg;
                    pnlMensagem.CssClass = "alert alert-success";
                    pnlMensagem.Visible = true;
                }

                CarregarPedidos();
            }
        }

        private void CarregarPedidos()
        {
            try
            {
                int usuarioId = (int)Session["UsuarioId"];

                using (PedidoBLL bll = new PedidoBLL())
                {
                    // Chama o método que já existe na sua BLL
                    List<Pedido> pedidos = bll.ListarHistoricoCliente(usuarioId);

                    if (pedidos != null && pedidos.Any())
                    {
                        rptPedidos.DataSource = pedidos;
                        rptPedidos.DataBind();
                        pnlSemPedidos.Visible = false;
                    }
                    else
                    {
                        pnlSemPedidos.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Erro ao carregar pedidos: " + ex.Message;
                pnlMensagem.CssClass = "alert alert-danger";
                pnlMensagem.Visible = true;
            }
        }

        // Helper para formatar a lista de itens no HTML
        protected string GetResumoItens(object itensObj)
        {
            var itens = itensObj as ICollection<ItemPedido>;
            if (itens == null || !itens.Any()) return "Nenhum item.";

            // Ex: "2x X-Burger, 1x Coca-Cola"
            var resumo = itens.Select(i => $"{i.Quantidade}x {i.Produto?.Nome ?? "Item"}").ToArray();
            return string.Join(", ", resumo);
        }

        // Helper para cor do badge
        protected string GetStatusColor(string status)
        {
            switch (status)
            {
                case "Pendente": return "warning text-dark";
                case "Em Preparo": return "info text-dark";
                case "Entregue": return "success";
                case "Cancelado": return "danger";
                default: return "secondary";
            }
        }

        // --- Métodos padrão do Header ---
        private void AtualizarHeader()
        {
            var carrinho = Session["Carrinho"] as Dictionary<int, int>;
            lblCarrinhoCount.Text = (carrinho != null) ? carrinho.Values.Sum().ToString() : "0";
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/FrmLogin.aspx");
        }
    }
}