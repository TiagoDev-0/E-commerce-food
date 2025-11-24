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
    public partial class Carrinho : System.Web.UI.Page
    {
        public class ItemCarrinhoDisplay
        {
            public int ProdutoId { get; set; }
            public int Quantidade { get; set; }
            public decimal PrecoUnitario { get; set; }
            public Produto Produto { get; set; }
            public decimal Subtotal => Quantidade * PrecoUnitario;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // ============================================================
            // 🛡️ CORREÇÃO DO LOOP DE REDIRECIONAMENTO
            // ============================================================

            // 1. Se não tem Cookie, aí sim manda pro login.
            if (!User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage("returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            // 2. Se tem Cookie, mas a Sessão morreu (UsuarioId nulo), RECUPERA OS DADOS.
            if (Session["UsuarioId"] == null)
            {
                try
                {
                    // O User.Identity.Name contém o email salvo no cookie
                    string emailCookie = User.Identity.Name;

                    using (UsuarioBLL usuarioBLL = new UsuarioBLL())
                    {
                        var usuario = usuarioBLL.ObterUsuarioPorEmail(emailCookie);

                        if (usuario != null)
                        {
                            // Restaura a sessão
                            Session["UsuarioId"] = usuario.Id;
                            Session["TipoUsuario"] = usuario.TipoUsuario;
                            Session["NomeUsuario"] = usuario.Nome;

                            // Se o carrinho também morreu, cria um novo vazio
                            if (Session["Carrinho"] == null)
                            {
                                Session["Carrinho"] = new Dictionary<int, int>();
                            }
                        }
                        else
                        {
                            // Se o usuário do cookie não existe mais no banco, força logout
                            FormsAuthentication.SignOut();
                            Response.Redirect("~/FrmLogin.aspx");
                            return;
                        }
                    }
                }
                catch
                {
                    // Se der erro ao recuperar, manda pro login
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/FrmLogin.aspx");
                    return;
                }
            }
            // ============================================================

            if (!IsPostBack)
            {
                CarregarCarrinho();
            }
            AtualizarHeader(); // Nome do usuário e Logout
            AtualizarContadorCarrinho();
        }

        // ... (O restante dos métodos permanece IGUAL, incluí abaixo para garantir) ...

        protected void btnBuscarHeader_Click(object sender, EventArgs e)
        {
            string termo = txtBuscaHeader.Text.Trim();
            if (!string.IsNullOrEmpty(termo))
            {
                Response.Redirect($"Busca.aspx?q={Server.UrlEncode(termo)}");
            }
        }

        private void AtualizarHeader()
        {
            if (User.Identity.IsAuthenticated)
            {
                pnlLogado.Visible = true;
                pnlVisitante.Visible = false;

                if (Session["NomeUsuario"] != null)
                    lblNomeUsuario.Text = Session["NomeUsuario"].ToString();
                else
                    lblNomeUsuario.Text = User.Identity.Name;
            }
            else
            {
                pnlLogado.Visible = false;
                pnlVisitante.Visible = true;
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/FrmLogin.aspx");
        }

        private void CarregarCarrinho()
        {
            var carrinho = Session["Carrinho"] as Dictionary<int, int>;

            if (carrinho == null || !carrinho.Any())
            {
                pnlCarrinhoVazio.Visible = true;
                pnlCarrinhoCheio.Visible = false;
                return;
            }

            pnlCarrinhoVazio.Visible = false;
            pnlCarrinhoCheio.Visible = true;

            List<ItemCarrinhoDisplay> itensParaExibicao = new List<ItemCarrinhoDisplay>();
            decimal subtotalCalculado = 0;

            try
            {
                using (ProdutoBLL bll = new ProdutoBLL())
                {
                    foreach (var item in carrinho)
                    {
                        int produtoId = item.Key;
                        int quantidade = item.Value;

                        Produto produto = bll.ObterProdutoPorId(produtoId);

                        if (produto != null)
                        {
                            itensParaExibicao.Add(new ItemCarrinhoDisplay
                            {
                                ProdutoId = produtoId,
                                Quantidade = quantidade,
                                PrecoUnitario = produto.Preco,
                                Produto = produto
                            });

                            subtotalCalculado += (produto.Preco * quantidade);
                        }
                        else
                        {
                            carrinho.Remove(produtoId);
                            Session["Carrinho"] = carrinho;
                        }
                    }
                }

                rptCarrinho.DataSource = itensParaExibicao;
                rptCarrinho.DataBind();

                lblSubtotal.Text = subtotalCalculado.ToString("C");
                lblTotal.Text = subtotalCalculado.ToString("C");
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar o carrinho: {ex.Message}", "danger");
            }
        }

        protected void rptCarrinho_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var carrinho = Session["Carrinho"] as Dictionary<int, int>;
            if (carrinho == null) return;

            int produtoId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Remove")
            {
                if (carrinho.ContainsKey(produtoId))
                {
                    carrinho.Remove(produtoId);
                    ExibirMensagem("Item removido.", "success");
                }
            }

            if (e.CommandName == "UpdateQty")
            {
                if (carrinho.ContainsKey(produtoId))
                {
                    TextBox txtQuantidade = (TextBox)e.Item.FindControl("txtQuantidade");
                    if (int.TryParse(txtQuantidade.Text, out int novaQtd))
                    {
                        if (novaQtd > 0 && novaQtd < 100)
                            carrinho[produtoId] = novaQtd;
                        else
                            carrinho.Remove(produtoId);

                        ExibirMensagem("Quantidade atualizada.", "success");
                    }
                }
            }

            Session["Carrinho"] = carrinho;
            CarregarCarrinho();
            AtualizarContadorCarrinho();
        }

        protected void btnFinalizarPedido_Click(object sender, EventArgs e)
        {
            var carrinho = Session["Carrinho"] as Dictionary<int, int>;
            int usuarioId = (int)Session["UsuarioId"];
            string observacoes = txtObservacoes.Text.Trim();

            if (carrinho == null || !carrinho.Any())
            {
                ExibirMensagem("Carrinho vazio.", "warning");
                return;
            }

            try
            {
                using (PedidoBLL bll = new PedidoBLL())
                {
                    string mensagem;
                    bool sucesso = bll.CriarPedido(usuarioId, carrinho, observacoes, out mensagem);

                    if (sucesso)
                    {
                        Session["Carrinho"] = null;
                        Response.Redirect("MeusPedidos.aspx?msg=" + Server.UrlEncode(mensagem));
                    }
                    else
                    {
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao finalizar: {ex.Message}", "danger");
            }

            CarregarCarrinho();
            AtualizarContadorCarrinho();
        }

        private void AtualizarContadorCarrinho()
        {
            var carrinho = Session["Carrinho"] as Dictionary<int, int>;
            lblCarrinhoCount.Text = (carrinho != null) ? carrinho.Values.Sum().ToString() : "0";
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-{tipo}";
            lblMsg.Text = mensagem;
        }
    }
}