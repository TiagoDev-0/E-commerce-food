using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.IO; // Necessário para salvar o arquivo
using System.Linq;
using System.Web.UI.WebControls;

namespace E_commerce_food.adm
{
    public partial class Produtos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidarAcesso();

            if (!IsPostBack)
            {
                CarregarProdutos();
                AtualizarEstatisticas();
            }
        }

        // ==============================
        // SEGURANÇA
        // ==============================
        private void ValidarAcesso()
        {
            string tipo = Session["TipoUsuario"]?.ToString().ToLower() ?? "";

            if (tipo != "administrador" && tipo != "admin")
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        // ==============================
        // LISTAGEM E CARGA
        // ==============================
        private void CarregarProdutos(string filtroCategoria = "", string busca = "")
        {
            try
            {
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    List<Produto> produtos = produtoBLL.ListarProdutos(
                        ddlFiltroCategoria.SelectedValue,
                        txtBusca.Text
                    );

                    gvProdutos.DataSource = produtos;
                    gvProdutos.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar produtos: {ex.Message}", "danger");
            }
        }

        // ==============================
        // ATUALIZAR ESTATÍSTICAS (LÓGICA DE ESTOQUE)
        // ==============================
        private void AtualizarEstatisticas()
        {
            try
            {
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    List<Produto> produtos = produtoBLL.ListarProdutos();

                    countTotal.InnerText = produtos.Count.ToString();

                    // Disponível: Checkbox marcado E Estoque positivo
                    countDisponiveis.InnerText = produtos.Count(p => p.Disponivel && p.Estoque > 0).ToString();

                    // Indisponível: Checkbox desmarcado OU Estoque zerado/negativo
                    countIndisponiveis.InnerText = produtos.Count(p => !p.Disponivel || p.Estoque <= 0).ToString();

                    countCategorias.InnerText = produtos.Select(p => p.Categoria).Distinct().Count().ToString();
                }
            }
            catch (Exception)
            {
                countTotal.InnerText = "0";
                countDisponiveis.InnerText = "0";
                countIndisponiveis.InnerText = "0";
                countCategorias.InnerText = "0";
            }
        }

        // ==============================
        // CADASTRO COM UPLOAD
        // ==============================
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    ExibirMensagem("O nome do produto é obrigatório.", "warning");
                    return;
                }
                if (string.IsNullOrWhiteSpace(ddlCategoria.SelectedValue))
                {
                    ExibirMensagem("Selecione uma categoria.", "warning");
                    return;
                }
                if (!decimal.TryParse(txtPreco.Text, out decimal preco) || preco <= 0)
                {
                    ExibirMensagem("Preço inválido.", "warning");
                    return;
                }
                int.TryParse(txtEstoque.Text, out int estoque);

                // Upload
                string nomeArquivoImagem = "sem-foto.png";

                if (fuImagem.HasFile)
                {
                    try
                    {
                        string extensao = Path.GetExtension(fuImagem.FileName).ToLower();
                        string[] extensoesValidas = { ".jpg", ".jpeg", ".png", ".gif" };

                        if (!extensoesValidas.Contains(extensao))
                        {
                            ExibirMensagem("Formato inválido. Use JPG, PNG ou GIF.", "warning");
                            return;
                        }

                        if (fuImagem.PostedFile.ContentLength > 2097152) // 2MB
                        {
                            ExibirMensagem("A imagem deve ter no máximo 2MB.", "warning");
                            return;
                        }

                        string novoNome = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString().Substring(0, 5) + extensao;
                        string caminhoPasta = Server.MapPath("~/uploads/");

                        if (!Directory.Exists(caminhoPasta))
                        {
                            Directory.CreateDirectory(caminhoPasta);
                        }

                        fuImagem.SaveAs(caminhoPasta + novoNome);
                        nomeArquivoImagem = novoNome;
                    }
                    catch (Exception ex)
                    {
                        ExibirMensagem("Erro no upload: " + ex.Message, "danger");
                        return;
                    }
                }

                // Objeto Produto
                Produto novoProduto = new Produto
                {
                    Nome = txtNome.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    Categoria = ddlCategoria.SelectedValue,
                    Preco = preco,
                    Estoque = estoque,
                    ImagemUrl = nomeArquivoImagem,
                    Disponivel = chkDisponivel.Checked
                };

                // Salvar
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    string mensagem;
                    bool sucesso = produtoBLL.CadastrarProduto(novoProduto, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem("Produto cadastrado com sucesso!", "success");
                        LimparCampos();
                        CarregarProdutos();
                        AtualizarEstatisticas();
                        // Evita reenvio ao recarregar a página
                        Response.Redirect(Request.RawUrl, false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                    else
                    {
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro crítico: {ex.Message}", "danger");
            }
        }

        // ==============================
        // EVENTOS DE GRIDVIEW
        // ==============================
        protected void gvProdutos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int produtoId = Convert.ToInt32(gvProdutos.DataKeys[e.NewEditIndex].Value);
            Response.Redirect($"ProdutosEditar.aspx?id={produtoId}");
        }

        protected void gvProdutos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int produtoId = Convert.ToInt32(gvProdutos.DataKeys[e.RowIndex].Value);
            try
            {
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    string mensagem;
                    bool sucesso = produtoBLL.ExcluirProduto(produtoId, out mensagem);
                    ExibirMensagem(mensagem, sucesso ? "success" : "danger");
                }
                CarregarProdutos();
                AtualizarEstatisticas();
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao excluir: {ex.Message}", "danger");
            }
        }

        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Lógica extra de linha, se necessária
            }
        }

        // ==============================
        // FILTROS
        // ==============================
        protected void txtBusca_TextChanged(object sender, EventArgs e)
        {
            CarregarProdutos(ddlFiltroCategoria.SelectedValue, txtBusca.Text);
        }

        protected void ddlFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarProdutos(ddlFiltroCategoria.SelectedValue, txtBusca.Text);
        }

        // ==============================
        // AUXILIARES
        // ==============================
        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-custom alert-{tipo} fixed-bottom m-3";
            lblMsg.Text = mensagem;
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtDescricao.Text = "";
            ddlCategoria.SelectedIndex = 0;
            txtPreco.Text = "";
            txtEstoque.Text = "";
            chkDisponivel.Checked = true;
        }

        protected string GetEstoqueClass(int estoque)
        {
            if (estoque >= 30) return "success";
            else if (estoque >= 10) return "warning";
            else return "danger";
        }
    }
}