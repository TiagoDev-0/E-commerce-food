using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Linq; // Necessário para .FirstOrDefault()
using System.Web.UI;

namespace E_commerce_food.adm
{
    public partial class UsuarioDetalhes : System.Web.UI.Page
    {
        // Armazena o ID do usuário que estamos editando
        private int _usuarioId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Validação de Segurança
            ValidarAcesso();

            // 2. Tentar obter o ID da URL
            if (!int.TryParse(Request.QueryString["id"], out _usuarioId))
            {
                // Se o ID for inválido ou não existir, volta para a lista
                ExibirMensagem("Usuário não encontrado.", "danger");
                Response.Redirect("FrmUsuarios.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Armazena o ID para uso nos cliques de botão (Salvar, Bloquear)
                ViewState["UsuarioId"] = _usuarioId;

                // 3. Carregar os dados do usuário no formulário
                CarregarDadosUsuario(_usuarioId);
            }
        }

        // Método de segurança padrão (copiado do Dashboard)
        private void ValidarAcesso()
        {
            string tipo = Session["TipoUsuario"]?.ToString().ToLower() ?? "";
            if (tipo != "administrador" && tipo != "admin")
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        // Carrega os dados do usuário do BLL para os campos do formulário
        private void CarregarDadosUsuario(int id)
        {
            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    // IMPORTANTE: Este método 'ObterUsuarioPorId' precisa ser criado na sua BLL.
                    // (Veja a observação após este arquivo)
                    Usuario usuario = bll.ObterUsuarioPorId(id);

                    if (usuario != null)
                    {
                        // Preenche os campos
                        txtNome.Text = usuario.Nome;
                        txtEmail.Text = usuario.Email;
                        txtTelefone.Text = usuario.Telefone;
                        txtEndereco.Text = usuario.Endereco;

                        // Atualiza as informações de status
                        lblDataCadastro.Text = usuario.DataCadastro.ToString("dd/MM/yyyy");

                        if (usuario.Bloqueado)
                        {
                            lblStatus.Text = "<i class='bi bi-lock-fill'></i> Bloqueado";
                            lblStatus.CssClass = "badge bg-danger";
                            btnBloquear.Text = "Desbloquear Usuário";
                            btnBloquear.CssClass = "btn btn-success";
                        }
                        else
                        {
                            lblStatus.Text = "<i class='bi bi-check-circle-fill'></i> Ativo";
                            lblStatus.CssClass = "badge bg-success";
                            btnBloquear.Text = "Bloquear Usuário";
                            btnBloquear.CssClass = "btn btn-warning";
                        }
                    }
                    else
                    {
                        ExibirMensagem("Usuário não encontrado.", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar dados: {ex.Message}", "danger");
            }
        }

        // Ação: Salva as alterações de perfil (Nome, Telefone, Endereço)
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação simples
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                ExibirMensagem("O campo 'Nome' é obrigatório.", "warning");
                return;
            }

            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    // Cria o objeto 'Usuario' com os dados atualizados
                    Usuario dadosAtualizados = new Usuario
                    {
                        Id = (int)ViewState["UsuarioId"],
                        Nome = txtNome.Text.Trim(),
                        Telefone = txtTelefone.Text.Trim(),
                        Endereco = txtEndereco.Text.Trim()
                        // Email não é enviado pois é ReadOnly e não deve ser alterado
                    };

                    string mensagem;
                    // Chama o método da BLL que você já criou
                    bool sucesso = bll.AtualizarPerfil(dadosAtualizados, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem(mensagem, "success");
                    }
                    else
                    {
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao salvar: {ex.Message}", "danger");
            }
        }

        // Ação: Alterna o status de bloqueio
        protected void btnBloquear_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioId = (int)ViewState["UsuarioId"];
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    string mensagem;
                    // Chama o método da BLL que você já criou
                    bool sucesso = bll.AlternarBloqueioCliente(usuarioId, out mensagem);

                    ExibirMensagem(mensagem, sucesso ? "success" : "danger");

                    // Recarrega os dados para atualizar os labels de status
                    if (sucesso)
                    {
                        CarregarDadosUsuario(usuarioId);
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao alterar status: {ex.Message}", "danger");
            }
        }

        // Método auxiliar para exibir mensagens
        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-{tipo}";
            lblMsg.Text = mensagem;
        }
    }
}