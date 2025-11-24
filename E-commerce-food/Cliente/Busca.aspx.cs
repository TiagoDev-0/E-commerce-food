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
    public partial class Busca : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Atualiza o cabeçalho (Login/Visitante e Contador)
            AtualizarHeader();

            if (!IsPostBack)
            {
                // 1. Pega o termo da URL (ex: Busca.aspx?q=burguer)
                string termo = Request.QueryString["q"];

                if (!string.IsNullOrEmpty(termo))
                {
                    lblTermoBusca.Text = termo;
                    CarregarResultados(termo);
                }
                else
                {
                    lblTermoBusca.Text = "Todos os Produtos";
                    CarregarResultados(""); // Carrega tudo se não houver termo
                }
            }
        }

        // ========================================
        // 2. CARREGAR E FILTRAR RESULTADOS
        // ========================================
        private void CarregarResultados(string termo)
        {
            try
            {
                using (ProdutoBLL bll = new ProdutoBLL())
                {
                    // Usa o método ListarProdutos da BLL que já aceita filtro de busca
                    // Parâmetro 1: Categoria (Vazio = Todas)
                    // Parâmetro 2: Busca (Termo digitado)
                    var produtos = bll.ListarProdutos("", termo);

                    // Filtra apenas produtos disponíveis para o cliente
                    // (Admin vê tudo, Cliente só vê o que está ativo)
                    var produtosDisponiveis = produtos.Where(p => p.Disponivel).ToList();

                    if (produtosDisponiveis != null && produtosDisponiveis.Any())
                    {
                        rptResultados.DataSource = produtosDisponiveis;
                        rptResultados.DataBind();

                        // Controla visibilidade dos painéis
                        pnlSemResultados.Visible = false;
                    }
                    else
                    {
                        pnlSemResultados.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro ao buscar produtos: " + ex.Message, "danger");
            }
        }

        // ========================================
        // 3. ADICIONAR AO CARRINHO
        // ========================================
        protected void rptResultados_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                // Verifica se o usuário está logado
                if (Session["UsuarioId"] == null)
                {
                    ExibirMensagem("Você precisa estar logado para comprar. <a href='../FrmLogin.aspx'>Entrar</a>", "warning");
                    return;
                }

                int produtoId = Convert.ToInt32(e.CommandArgument);

                // Lógica do Carrinho na Sessão
                var carrinho = Session["Carrinho"] as Dictionary<int, int>;
                if (carrinho == null)
                {
                    carrinho = new Dictionary<int, int>();
                }

                // Adiciona ou incrementa
                if (carrinho.ContainsKey(produtoId))
                {
                    carrinho[produtoId]++;
                }
                else
                {
                    carrinho[produtoId] = 1;
                }

                // Salva de volta na sessão
                Session["Carrinho"] = carrinho;

                // Atualiza UI
                AtualizarContadorCarrinho();
                ExibirMensagem("Item adicionado ao carrinho com sucesso!", "success");
            }
        }

        // ========================================
        // 4. NOVA BUSCA (Pelo Header da própria página)
        // ========================================
        protected void btnBuscarHeader_Click(object sender, EventArgs e)
        {
            string termo = txtBuscaHeader.Text.Trim();
            if (!string.IsNullOrEmpty(termo))
            {
                // Redireciona para a mesma página com novo termo
                Response.Redirect($"Busca.aspx?q={Server.UrlEncode(termo)}");
            }
        }

        // ========================================
        // MÉTODOS AUXILIARES (Copiados do Default.aspx)
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