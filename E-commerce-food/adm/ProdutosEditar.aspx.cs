using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Web.UI; 
using System.Web.UI.WebControls;

namespace E_commerce_food.adm
{
    public partial class ProdutosEditar : System.Web.UI.Page
    {
        // Armazena o ID do produto sendo editado
        protected int ProdutoId;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            ValidarAcesso();



            // Tenta obter o ID do produto da URL em CADA carregamento
            if (!int.TryParse(Request.QueryString["id"], out ProdutoId))
            {
                ExibirMensagem("ID do produto inválido ou não fornecido.", "danger");
                // Desabilita o botão salvar se o ID for inválido
                btnSalvar.Enabled = false;
                return;
            }

            if (!IsPostBack)
            {
                // Só carrega os dados do banco na primeira vez
                CarregarProduto();
            }
        }

        // ==============================
        // 🔹 MÉTODO ADICIONADO
        // ==============================
        // Copiado do Dashboard.aspx.cs para padronizar a segurança
        private void ValidarAcesso()
        {
            // O web.config (Forms Auth) já garante que o usuário está logado.
            // Aqui verificamos se o usuário logado é um Administrador.

            string tipo = Session["TipoUsuario"]?.ToString().ToLower() ?? "";

            if (tipo != "administrador" && tipo != "admin")
            {
                // Usuário comum tentando entrar no Admin
                Response.Redirect("~/Default.aspx");
                return;
            }
        }
        // ==============================

        private void CarregarProduto()
        {
            try
            {
                using (ProdutoBLL bll = new ProdutoBLL())
                {
                    Produto p = bll.ObterProdutoPorId(ProdutoId);

                    if (p == null)
                    {
                        ExibirMensagem("Produto não encontrado.", "danger");
                        btnSalvar.Enabled = false; // Desabilita salvar se o produto não existe
                        return;
                    }

                    // Preencher campos
                    txtNome.Text = p.Nome;
                    txtDescricao.Text = p.Descricao;

                    // Define o valor do DropDownList
                    if (ddlCategoria.Items.FindByValue(p.Categoria) != null)
                    {
                        ddlCategoria.SelectedValue = p.Categoria;
                    }

                    txtPreco.Text = p.Preco.ToString("F2"); // Formata para 2 casas decimais
                    txtEstoque.Text = p.Estoque.ToString();
                    txtImagemUrl.Text = p.ImagemUrl;
                    chkDisponivel.Checked = p.Disponivel;

                    imgPreview.ImageUrl = p.ImagemUrl;
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro ao carregar produto: " + ex.Message, "danger");
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Re-valida o ID (essencial, pois o 'ProdutoId' é uma variável de classe)
                if (!int.TryParse(Request.QueryString["id"], out ProdutoId) || ProdutoId == 0)
                {
                    ExibirMensagem("ID do produto inválido. Não foi possível salvar.", "danger");
                    return;
                }

                if (!decimal.TryParse(txtPreco.Text, out decimal preco))
                {
                    ExibirMensagem("Preço inválido.", "warning");
                    return;
                }

                if (!int.TryParse(txtEstoque.Text, out int estoque))
                {
                    ExibirMensagem("Estoque inválido.", "warning");
                    return;
                }

                Produto p = new Produto
                {
                    Id = ProdutoId, // ID obtido da QueryString
                    Nome = txtNome.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    Categoria = ddlCategoria.SelectedValue,
                    Preco = preco,
                    Estoque = estoque,
                    ImagemUrl = txtImagemUrl.Text.Trim(),
                    Disponivel = chkDisponivel.Checked
                };

                using (ProdutoBLL bll = new ProdutoBLL())
                {
                    string msg;
                    bool ok = bll.AtualizarProduto(p, out msg);

                    if (ok)
                    {
                        ExibirMensagem("Produto atualizado com sucesso!", "success");
                    }
                    else
                    {
                        ExibirMensagem(msg, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro ao salvar: " + ex.Message, "danger");
            }
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMsg.Visible = true;
            pnlMsg.CssClass = $"alert alert-{tipo}";
            pnlMsg.Controls.Clear();
            pnlMsg.Controls.Add(new Literal { Text = mensagem });
        }
    }
}