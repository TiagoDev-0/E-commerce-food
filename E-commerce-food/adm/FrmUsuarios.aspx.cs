using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic; // Necessário para List<T>
using E_commerce_food.BLL;      // Necessário para UsuarioBLL
using E_commerce_food.Models;   // Necessário para Usuario

namespace E_commerce_food.adm
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Validação de acesso (Correta)
            ValidarAcesso();

            if (!IsPostBack)
            {
                CarregarUsuarios();
                AtualizarEstatisticas();
            }
        }

        // ========================================
        // VALIDAÇÃO DE ACESSO (CORRETA)
        // ========================================
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

        // ========================================
        // CARREGAR USUÁRIOS (AGORA COM DADOS REAIS)
        // ========================================
        private void CarregarUsuarios(string filtroStatus = "", string busca = "")
        {
            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    // 1. Busca os clientes reais do BLL
                    List<Usuario> usuarios = bll.ListarClientes();

                    // 2. Aplicar filtro de status (usando LINQ)
                    if (filtroStatus == "Ativo")
                    {
                        usuarios = usuarios.Where(u => !u.Bloqueado).ToList();
                    }
                    else if (filtroStatus == "Bloqueado")
                    {
                        usuarios = usuarios.Where(u => u.Bloqueado).ToList();
                    }

                    // 3. Aplicar busca (usando LINQ)
                    if (!string.IsNullOrEmpty(busca))
                    {
                        busca = busca.ToLower();
                        usuarios = usuarios.Where(u =>
                            (u.Nome != null && u.Nome.ToLower().Contains(busca)) ||
                            (u.Email != null && u.Email.ToLower().Contains(busca))
                        ).ToList();
                    }

                    gvUsuarios.DataSource = usuarios;
                    gvUsuarios.DataBind();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar usuários: {ex.Message}", "danger");
            }
        }

        // ========================================
        // ATUALIZAR ESTATÍSTICAS (AGORA COM DADOS REAIS)
        // ========================================
        private void AtualizarEstatisticas()
        {
            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    // Busca todos os clientes para estatísticas
                    List<Usuario> usuarios = bll.ListarClientes();

                    // Total de usuários
                    countTotal.InnerText = usuarios.Count.ToString();

                    // Usuários ativos
                    int ativos = usuarios.Count(row => !row.Bloqueado);
                    countAtivos.InnerText = ativos.ToString();

                    // Usuários bloqueados
                    int bloqueados = usuarios.Count(row => row.Bloqueado);
                    countBloqueados.InnerText = bloqueados.ToString();

                    // Novos usuários (últimos 7 dias)
                    DateTime seteDiasAtras = DateTime.Now.AddDays(-7);
                    int novos = usuarios.Count(row => row.DataCadastro >= seteDiasAtras);
                    countNovos.InnerText = novos.ToString();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar estatísticas: {ex.Message}", "warning");
            }
        }

        // ========================================
        // EVENTOS DO GRIDVIEW
        // ========================================
        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int usuarioId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Bloquear":
                    BloquearDesbloquearUsuario(usuarioId);
                    break;

                // ==============================
                // 🔹 ATUALIZAÇÃO AQUI
                // ==============================
                // Reativa o redirecionamento para a página de detalhes
                case "VerDetalhes":
                    Response.Redirect($"UsuarioDetalhes.aspx?id={usuarioId}");
                    break;
                // ==============================

                case "Excluir":
                    ExcluirUsuario(usuarioId);
                    break;
            }
        }

        // ========================================
        // BLOQUEAR/DESBLOQUEAR (AGORA COM LÓGICA REAL)
        // ========================================
        private void BloquearDesbloquearUsuario(int usuarioId)
        {
            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    string mensagem;
                    // Chama o método real da BLL
                    bool sucesso = bll.AlternarBloqueioCliente(usuarioId, out mensagem);

                    ExibirMensagem(mensagem, sucesso ? "success" : "danger");
                }

                // Recarrega os dados após a ação
                CarregarUsuarios(ddlFiltroStatus.SelectedValue, txtBusca.Text);
                AtualizarEstatisticas();
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao alterar status: {ex.Message}", "danger");
            }
        }

        // ========================================
        // EXCLUIR USUÁRIO
        // ========================================
        private void ExcluirUsuario(int usuarioId)
        {
            try
            {
                // TODO: A BLL (UsuarioBLL.cs) que você enviou não possui um método ExcluirCliente.
                // Você precisará criar esse método na BLL e no Repository.

                ExibirMensagem("Função 'Excluir' ainda não implementada na BLL.", "info");

                // CarregarUsuarios(ddlFiltroStatus.SelectedValue, txtBusca.Text);
                // AtualizarEstatisticas();
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao excluir usuário: {ex.Message}", "danger");
            }
        }

        // ========================================
        // MÉTODOS RESTANTES (Filtros, Auxiliares, etc.)
        // ========================================

        protected string FormatEndereco(object valor)
        {
            string texto = valor == null ? "" : valor.ToString();
            return string.IsNullOrWhiteSpace(texto) ? "Não informado" : texto;
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Customizações adicionais se necessário
            }
        }

        protected void txtBusca_TextChanged(object sender, EventArgs e)
        {
            CarregarUsuarios(ddlFiltroStatus.SelectedValue, txtBusca.Text);
            // AtualizarEstatisticas(); // Opcional: Atualizar estatísticas na busca
        }

        protected void ddlFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarUsuarios(ddlFiltroStatus.SelectedValue, txtBusca.Text);
            // AtualizarEstatisticas(); // Opcional: Atualizar estatísticas no filtro
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            // TODO: Esta função ainda usa ObterUsuariosSimulados().
            // Você precisará alterá-la para usar 'bll.ListarClientes()' como fiz em CarregarUsuarios.

            ExibirMensagem("Exportação precisa ser atualizada para dados reais.", "info");
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-custom alert-{tipo}";
            lblMsg.Text = mensagem;
        }
    }
}