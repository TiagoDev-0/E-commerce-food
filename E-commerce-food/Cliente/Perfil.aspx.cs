using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Web.Security;
using System.Web.UI;

namespace E_commerce_food.Cliente
{
    public partial class Perfil : System.Web.UI.Page
    {
        private int _usuarioId = 0; // Armazena o ID do usuário logado

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Validação de Segurança
            if (Session["UsuarioId"] == null)
            {
                // O web.config já deve ter barrado, mas esta é uma segurança dupla.
                // Redireciona para o login com a URL atual como retorno.
                string returnUrl = Server.UrlEncode(Request.RawUrl);
                FormsAuthentication.RedirectToLoginPage("returnUrl=" + returnUrl);
                return;
            }

            // Garante que o ID foi pego da sessão
            _usuarioId = (int)Session["UsuarioId"];

            // 2. Segurança: Se um Admin acessar esta página, redireciona para o Dashboard
            string tipo = Session["TipoUsuario"]?.ToString().ToLower() ?? "";
            if (tipo == "admin" || tipo == "administrador")
            {
                Response.Redirect("~/adm/Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // 3. Carrega os dados do usuário no formulário
                CarregarDadosUsuario();
            }
        }

        // Carrega os dados do usuário logado
        private void CarregarDadosUsuario()
        {
            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    // Usa o método que você confirmou que existe na BLL
                    Usuario usuario = bll.ObterUsuarioPorId(_usuarioId);

                    if (usuario != null)
                    {
                        // Preenche os campos
                        txtNome.Text = usuario.Nome;
                        txtEmail.Text = usuario.Email;
                        txtTelefone.Text = usuario.Telefone;
                        txtEndereco.Text = usuario.Endereco;
                    }
                    else
                    {
                        ExibirMensagem("Não foi possível carregar seus dados. Tente sair e entrar novamente.", "danger");
                        btnSalvarDados.Enabled = false;
                        btnAlterarSenha.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao carregar dados: {ex.Message}", "danger");
            }
        }

        // Ação: Salva as alterações de dados (Nome, Telefone, Endereço)
        protected void btnSalvarDados_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) // Verifica os validadores (como o rfvNome)
            {
                return;
            }

            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    // Cria o objeto 'Usuario' com os dados atualizados
                    Usuario dadosAtualizados = new Usuario
                    {
                        Id = _usuarioId, // ID pego da Sessão
                        Nome = txtNome.Text.Trim(),
                        Telefone = txtTelefone.Text.Trim(),
                        Endereco = txtEndereco.Text.Trim()
                        // Email e Senha não são atualizados aqui
                    };

                    string mensagem;
                    // Chama o método da BLL que você já tem
                    bool sucesso = bll.AtualizarPerfil(dadosAtualizados, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem(mensagem, "success");
                        // Atualiza o nome na sessão, caso tenha mudado
                        Session["NomeUsuario"] = dadosAtualizados.Nome;
                    }
                    else
                    {
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro ao salvar perfil: {ex.Message}", "danger");
            }
        }

        // Ação: Altera a senha
        protected void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            // Validação simples
            if (string.IsNullOrWhiteSpace(txtSenhaAtual.Text) ||
                string.IsNullOrWhiteSpace(txtNovaSenha.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmarNovaSenha.Text))
            {
                ExibirMensagem("Preencha todos os campos de senha.", "warning");
                return;
            }

            if (txtNovaSenha.Text != txtConfirmarNovaSenha.Text)
            {
                ExibirMensagem("A nova senha e a confirmação não conferem.", "warning");
                return;
            }

            // =====================================================================
            // 🔹 CORREÇÃO AQUI: Chamando a lógica real da BLL
            // =====================================================================
            try
            {
                using (UsuarioBLL bll = new UsuarioBLL())
                {
                    string mensagem;
                    bool sucesso = bll.AlterarSenha(_usuarioId, txtSenhaAtual.Text, txtNovaSenha.Text, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem(mensagem, "success");
                        // Limpa os campos após o sucesso
                        txtSenhaAtual.Text = "";
                        txtNovaSenha.Text = "";
                        txtConfirmarNovaSenha.Text = "";
                    }
                    else
                    {
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro no processo: {ex.Message}", "danger");
            }
        }

        // Ação: Logout
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/FrmLogin.aspx");
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