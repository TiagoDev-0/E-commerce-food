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
    public partial class Sobremesas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AtualizarHeader();

            if (!IsPostBack)
            {
                CarregarSobremesas();
            }
        }

        protected void btnBuscarHeader_Click(object sender, EventArgs e)
        {
            string termo = txtBuscaHeader.Text.Trim();
            if (!string.IsNullOrEmpty(termo))
            {
                // Redireciona para a página de Busca com o termo na URL
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
                {
                    lblNomeUsuario.Text = Session["NomeUsuario"].ToString();
                }
                else
                {
                    lblNomeUsuario.Text = User.Identity.Name;
                }

                AtualizarContadorCarrinho();
            }
            else
            {
                pnlLogado.Visible = false;
                pnlVisitante.Visible = true;
            }
        }

        private void CarregarSobremesas()
        {
            try
            {
                using (ProdutoBLL bll = new ProdutoBLL())
                {
                    // Busca TODOS os produtos
                    var todosProdutos = bll.ListarProdutos("", "");

                    // Filtra por Sobremesa (ou Doce, caso tenha cadastrado diferente)
                    var doces = todosProdutos.Where(p =>
                        p.Categoria.Equals("Sobremesa", StringComparison.OrdinalIgnoreCase) ||
                        p.Categoria.Equals("Doce", StringComparison.OrdinalIgnoreCase)
                    ).ToList();

                    if (doces != null && doces.Any())
                    {
                        rptSobremesas.DataSource = doces;
                        rptSobremesas.DataBind();
                    }
                    else
                    {
                        pnlSemProdutos.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar sobremesas: {ex.Message}", "danger");
            }
        }

        protected void rptSobremesas_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                if (Session["UsuarioId"] == null)
                {
                    ExibirMensagem("Faça login para adoçar seu dia! <a href='../FrmLogin.aspx'>Entrar</a>", "warning");
                    return;
                }

                int produtoId = Convert.ToInt32(e.CommandArgument);

                var carrinho = Session["Carrinho"] as Dictionary<int, int>;
                if (carrinho == null)
                {
                    carrinho = new Dictionary<int, int>();
                }

                if (carrinho.ContainsKey(produtoId))
                {
                    carrinho[produtoId]++;
                }
                else
                {
                    carrinho[produtoId] = 1;
                }

                Session["Carrinho"] = carrinho;
                AtualizarContadorCarrinho();
                ExibirMensagem("Sobremesa adicionada ao carrinho!", "success");
            }
        }

        private void AtualizarContadorCarrinho()
        {
            var carrinho = Session["Carrinho"] as Dictionary<int, int>;
            if (carrinho != null)
            {
                lblCarrinhoCount.Text = carrinho.Values.Sum().ToString();
            }
            else
            {
                lblCarrinhoCount.Text = "0";
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/FrmLogin.aspx");
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-{tipo}";
            lblMsg.Text = mensagem;
        }
    }
}