using E_commerce_food.Models;
using System;
using System.Web.UI;

namespace E_commerce_food.adm
{
    public partial class FrmLoginAdm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Se o admin já estiver logado, redireciona pro dashboard
            if (Session["UsuarioTipo"] != null && Session["UsuarioTipo"].ToString() == "admin")
            {
                Response.Redirect("~/adm/Dashboard.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ExibirMensagem("Preencha todos os campos.", "danger");
                return;
            }

            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                string mensagem;
                Usuario usuario = usuarioBLL.RealizarLogin(email, senha, out mensagem);

                if (usuario != null && usuario.TipoUsuario.ToLower() == "admin")
                {
                    // Cria sessão do admin
                    Session["UsuarioId"] = usuario.Id;
                    Session["UsuarioNome"] = usuario.Nome;
                    Session["UsuarioEmail"] = usuario.Email;
                    Session["UsuarioTipo"] = usuario.TipoUsuario;

                    Response.Redirect("~/adm/Dashboard.aspx");
                }
                else if (usuario != null)
                {
                    ExibirMensagem("Acesso negado! Apenas administradores podem entrar aqui.", "danger");
                }
                else
                {
                    ExibirMensagem(mensagem, "danger");
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem($"Erro: {ex.Message}", "danger");
            }
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-{tipo}";
            lblMensagem.Text = mensagem;
        }
    }
}
