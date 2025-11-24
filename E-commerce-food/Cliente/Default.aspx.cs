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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AtualizarHeader();

            if (!IsPostBack)
            {
                CarregarProdutosDestaque();
            }
        }

        // ========================================
        // 🔹 LÓGICA DA BUSCA (ESSENCIAL) 🔹
        // ========================================
        // Este método é chamado quando o usuário aperta Enter na busca
        protected void btnBuscarHeader_Click(object sender, EventArgs e)
        {
            string termo = txtBuscaHeader.Text.Trim();
            if (!string.IsNullOrEmpty(termo))
            {
                // Redireciona para a página de Busca com o termo na URL
                Response.Redirect($"Busca.aspx?q={Server.UrlEncode(termo)}");
            }
        }

        // ========================================
        // CONTROLE DO CABEÇALHO
        // ========================================
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

        // ========================================
        // CARREGAR PRODUTOS (DESTAQUES)
        // ========================================
        private void CarregarProdutosDestaque()
        {
            try
            {
                using (ProdutoBLL bll = new ProdutoBLL())
                {
                    var todosProdutos = bll.ListarProdutos("", "");

                    if (todosProdutos != null && todosProdutos.Any())
                    {
                        // Pega os primeiros 8 produtos disponíveis
                        var destaques = todosProdutos
                                            .Where(p => p.Disponivel)
                                            .Take(8)
                                            .ToList();

                        rptProdutosDestaque.DataSource = destaques;
                        rptProdutosDestaque.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar destaques: {ex.Message}", "danger");
            }
        }

        // ========================================
        // ADICIONAR AO CARRINHO
        // ========================================
        protected void rptProdutosDestaque_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                if (Session["UsuarioId"] == null)
                {
                    ExibirMensagem("Você precisa estar logado para adicionar itens ao carrinho. <a href='../FrmLogin.aspx'>Fazer Login</a>", "warning");
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
                ExibirMensagem("Item adicionado ao carrinho com sucesso!", "success");
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
            pnlMensagem.CssClass = $"container alert alert-{tipo} mt-3";
            lblMsg.Text = mensagem;
        }
    }
}