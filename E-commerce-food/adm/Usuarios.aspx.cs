using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace E_commerce_food.Admin
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar se é admin
            if (Session["AdminId"] == null)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CarregarUsuarios();
                AtualizarEstatisticas();
            }
        }

        // ========================================
        // CARREGAR USUÁRIOS
        // ========================================
        private void CarregarUsuarios(string filtroStatus = "", string busca = "")
        {
            // TODO: Substituir por chamada ao ClienteBLL real
            DataTable dt = ObterUsuariosSimulados();

            // Aplicar filtro de status
            if (!string.IsNullOrEmpty(filtroStatus))
            {
                DataView dv = dt.DefaultView;
                if (filtroStatus == "Ativo")
                {
                    dv.RowFilter = "Bloqueado = false";
                }
                else if (filtroStatus == "Bloqueado")
                {
                    dv.RowFilter = "Bloqueado = true";
                }
                dt = dv.ToTable();
            }

            // Aplicar busca
            if (!string.IsNullOrEmpty(busca))
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"Nome LIKE '%{busca}%' OR Email LIKE '%{busca}%'";
                dt = dv.ToTable();
            }

            gvUsuarios.DataSource = dt;
            gvUsuarios.DataBind();
        }

        // ========================================
        // DADOS SIMULADOS
        // ========================================
        private DataTable ObterUsuariosSimulados()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Telefone", typeof(string));
            dt.Columns.Add("Endereco", typeof(string));
            dt.Columns.Add("Bloqueado", typeof(bool));
            dt.Columns.Add("DataCadastro", typeof(DateTime));

            // Dados de exemplo
            dt.Rows.Add(1, "João Silva", "joao@email.com", "(11) 98765-4321", "Rua A, 123", false, DateTime.Now.AddDays(-30));
            dt.Rows.Add(2, "Maria Santos", "maria@email.com", "(11) 91234-5678", "Av. B, 456", false, DateTime.Now.AddDays(-15));
            dt.Rows.Add(3, "Pedro Oliveira", "pedro@email.com", "(11) 99876-5432", "Rua C, 789", true, DateTime.Now.AddDays(-60));
            dt.Rows.Add(4, "Ana Costa", "ana@email.com", "(11) 98888-7777", "Av. D, 321", false, DateTime.Now.AddDays(-5));
            dt.Rows.Add(5, "Carlos Lima", "carlos@email.com", "(11) 97777-6666", "Rua E, 654", false, DateTime.Now.AddDays(-2));
            dt.Rows.Add(6, "Julia Ferreira", "julia@email.com", "(11) 96666-5555", "", true, DateTime.Now.AddDays(-90));
            dt.Rows.Add(7, "Roberto Alves", "roberto@email.com", "", "Rua F, 987", false, DateTime.Now.AddDays(-1));

            return dt;
        }

        // ========================================
        // ATUALIZAR ESTATÍSTICAS
        // ========================================
        private void AtualizarEstatisticas()
        {
            DataTable dt = ObterUsuariosSimulados();

            // Total de usuários
            countTotal.InnerText = dt.Rows.Count.ToString();

            // Usuários ativos
            int ativos = dt.AsEnumerable().Count(row => !row.Field<bool>("Bloqueado"));
            countAtivos.InnerText = ativos.ToString();

            // Usuários bloqueados
            int bloqueados = dt.AsEnumerable().Count(row => row.Field<bool>("Bloqueado"));
            countBloqueados.InnerText = bloqueados.ToString();

            // Novos usuários (últimos 7 dias)
            DateTime seteDiasAtras = DateTime.Now.AddDays(-7);
            int novos = dt.AsEnumerable().Count(row => row.Field<DateTime>("DataCadastro") >= seteDiasAtras);
            countNovos.InnerText = novos.ToString();
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

                case "VerDetalhes":
                    Response.Redirect($"UsuarioDetalhes.aspx?id={usuarioId}");
                    break;

                case "Excluir":
                    ExcluirUsuario(usuarioId);
                    break;
            }
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Customizações adicionais se necessário
            }
        }

        // ========================================
        // BLOQUEAR/DESBLOQUEAR USUÁRIO
        // ========================================
        private void BloquearDesbloquearUsuario(int usuarioId)
        {
            try
            {
                // TODO: Implementar lógica real com ClienteBLL
                /*
                ClienteBLL clienteBLL = new ClienteBLL();
                var usuario = clienteBLL.BuscarPorId(usuarioId);
                
                if (usuario != null)
                {
                    bool novoBloqueio = !usuario.Bloqueado;
                    clienteBLL.AlterarBloqueio(usuarioId, novoBloqueio);
                    
                    string acao = novoBloqueio ? "bloqueado" : "desbloqueado";
                    ExibirMensagem($"Usuário {acao} com sucesso!", "success");
                }
                */

                // Simulação

                    ExibirMensagem("Status do usuário alterado com sucesso!", "success");
                CarregarUsuarios();
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
                // TODO: Implementar exclusão real com ClienteBLL
                /*
                ClienteBLL clienteBLL = new ClienteBLL();
                clienteBLL.ExcluirCliente(usuarioId);
                */

                ExibirMensagem("Usuário excluído com sucesso!", "success");
                CarregarUsuarios();
                AtualizarEstatisticas();
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao excluir usuário: {ex.Message}", "danger");
            }
        }

        // ========================================
        // FILTROS E BUSCA
        // ========================================
        protected void txtBusca_TextChanged(object sender, EventArgs e)
        {
            CarregarUsuarios(ddlFiltroStatus.SelectedValue, txtBusca.Text);
            AtualizarEstatisticas();
        }

        protected void ddlFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarUsuarios(ddlFiltroStatus.SelectedValue, txtBusca.Text);
            AtualizarEstatisticas();
        }

        // ========================================
        // EXPORTAR CSV
        // ========================================
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ObterUsuariosSimulados();

                // Criar CSV
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                // Cabeçalhos
                sb.AppendLine("ID,Nome,Email,Telefone,Endereco,Status,Data Cadastro");

                // Dados
                foreach (DataRow row in dt.Rows)
                {
                    sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                        row["Id"],
                        row["Nome"],
                        row["Email"],
                        row["Telefone"],
                        row["Endereco"],
                        (bool)row["Bloqueado"] ? "Bloqueado" : "Ativo",
                        Convert.ToDateTime(row["DataCadastro"]).ToString("dd/MM/yyyy")
                    ));
                }

                // Download do arquivo
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", $"attachment;filename=usuarios_{DateTime.Now:yyyyMMdd}.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao exportar: {ex.Message}", "danger");
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
    }
}