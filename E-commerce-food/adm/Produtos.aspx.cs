using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace E_commerce_food.Admin
{
    // A camada de Apresentação (UI) agora interage exclusivamente com a BLL.
    public partial class Produtos : System.Web.UI.Page
    {
        // Usamos using/try-catch para garantir que o Dispose() seja chamado no BLL
        // liberando o DbContext.

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar se é admin (adicione sua lógica de autenticação)
            if (Session["AdminId"] == null)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Garante que a primeira carga use o BLL
                CarregarProdutos();
                AtualizarEstatisticas();
            }
        }

        // ========================================
        // CARREGAR PRODUTOS (LEITURA - READ)
        // ========================================
        private void CarregarProdutos(string filtroCategoria = "", string busca = "")
        {
            try
            {
                // TODO: Usar 'using' para garantir o Dispose
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    // A BLL aplica os filtros, trazendo apenas os dados reais do banco
                    List<Produto> produtos = produtoBLL.ListarProdutos(
                        ddlFiltroCategoria.SelectedValue,
                        txtBusca.Text
                    );

                    // Bind do GridView com a lista de entidades do Models
                    gvProdutos.DataSource = produtos;
                    gvProdutos.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Em caso de falha na conexão com o BD
                ExibirMensagem($"Erro ao carregar produtos do banco de dados: {ex.Message}", "danger");
                // Limpa o GridView em caso de erro
                gvProdutos.DataSource = null;
                gvProdutos.DataBind();
            }
        }

        // ========================================
        // ATUALIZAR ESTATÍSTICAS
        // ========================================
        private void AtualizarEstatisticas()
        {
            try
            {
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    // Busca todos os produtos para cálculo das estatísticas
                    List<Produto> produtos = produtoBLL.ListarProdutos();

                    // Total de produtos
                    countTotal.InnerText = produtos.Count.ToString();

                    // Produtos disponíveis
                    int disponiveis = produtos.Count(p => p.Disponivel);
                    countDisponiveis.InnerText = disponiveis.ToString();

                    // Produtos indisponíveis
                    int indisponiveis = produtos.Count(p => !p.Disponivel);
                    countIndisponiveis.InnerText = indisponiveis.ToString();

                    // Quantidade de categorias únicas
                    int categorias = produtos.Select(p => p.Categoria).Distinct().Count();
                    countCategorias.InnerText = categorias.ToString();
                }
            }
            catch (Exception)
            {
                // Mantém estatísticas zeradas ou ignora erro para não quebrar a UI
                countTotal.InnerText = "N/A";
                countDisponiveis.InnerText = "N/A";
                countIndisponiveis.InnerText = "N/A";
                countCategorias.InnerText = "N/A";
            }
        }

        // ========================================
        // EVENTOS DO GRIDVIEW (EDIÇÃO E EXCLUSÃO)
        // ========================================
        protected void gvProdutos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int produtoId = Convert.ToInt32(gvProdutos.DataKeys[e.NewEditIndex].Value);

            // Redireciona para página de edição
            Response.Redirect($"ProdutosEditar.aspx?id={produtoId}");

            e.Cancel = true; // Cancelar modo de edição inline
        }

        protected void gvProdutos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Usando o try/catch para a chamada BLL, conforme o requisito de tratamento de exceções.
            int produtoId = Convert.ToInt32(gvProdutos.DataKeys[e.RowIndex].Value);

            try
            {
                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    string mensagem;
                    bool sucesso = produtoBLL.ExcluirProduto(produtoId, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem(mensagem, "success");
                    }
                    else
                    {
                        // Mensagem de erro de negócio (ex: produto com pedidos pendentes)
                        ExibirMensagem(mensagem, "danger");
                    }
                }

                CarregarProdutos();
                AtualizarEstatisticas();
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao tentar excluir produto: {ex.Message}", "danger");
            }
        }

        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Lógica de DataBound
            }
        }

        // ========================================
        // FILTROS E BUSCA
        // ========================================
        protected void txtBusca_TextChanged(object sender, EventArgs e)
        {
            // Ambos os métodos CarregarProdutos e AtualizarEstatisticas
            // agora dependem dos valores dos filtros.
            CarregarProdutos(ddlFiltroCategoria.SelectedValue, txtBusca.Text);
            AtualizarEstatisticas();
        }

        protected void ddlFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarProdutos(ddlFiltroCategoria.SelectedValue, txtBusca.Text);
            AtualizarEstatisticas();
        }

        // ========================================
        // ADICIONAR NOVO PRODUTO (CRIAÇÃO - CREATE)
        // ========================================
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validações client-side (para evitar chamadas desnecessárias)
                // As validações server-side mais complexas ficam na BLL.
                if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtPreco.Text))
                {
                    ExibirMensagem("Preencha Nome e Preço.", "warning");
                    return;
                }

                // Tenta fazer o parse antes de chamar o BLL (para não passar valores inválidos)
                if (!decimal.TryParse(txtPreco.Text, out decimal preco) || preco <= 0)
                {
                    ExibirMensagem("Preço inválido.", "warning");
                    return;
                }

                int estoque = 0;
                int.TryParse(txtEstoque.Text, out estoque); // Se falhar, estoque fica 0

                // Cria o objeto Produto do Models, preenchendo as propriedades
                Produto novoProduto = new Produto
                {
                    Nome = txtNome.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    Categoria = ddlCategoria.SelectedValue,
                    Preco = preco,
                    Estoque = estoque,
                    ImagemUrl = txtImagemUrl.Text.Trim(),
                    Disponivel = chkDisponivel.Checked
                };

                using (ProdutoBLL produtoBLL = new ProdutoBLL())
                {
                    string mensagem;
                    // Chamar o método de Cadastro da BLL, que cuida das regras e persistência
                    bool sucesso = produtoBLL.CadastrarProduto(novoProduto, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem("Produto cadastrado com sucesso!", "success");
                        LimparCampos();
                        CarregarProdutos();
                        AtualizarEstatisticas();
                    }
                    else
                    {
                        // Mensagem retorna a regra de negócio violada
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao cadastrar produto: {ex.Message}", "danger");
            }
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

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtDescricao.Text = "";
            ddlCategoria.SelectedIndex = 0;
            txtPreco.Text = "";
            txtEstoque.Text = "";
            txtImagemUrl.Text = "";
            chkDisponivel.Checked = true;
        }

        // Método auxiliar para classe CSS do badge de estoque
        protected string GetEstoqueClass(int estoque)
        {
            if (estoque >= 30)
                return "alto";
            else if (estoque >= 10)
                return "medio";
            else
                return "baixo";
        }
    }
}