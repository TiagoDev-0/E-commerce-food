using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Web.Security; // Essencial
using System.Web.UI;

namespace E_commerce_food
{
    public partial class FrmLogin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlMensagem.Visible = false;

            // Se o usuário já está logado e acessa o login,
            // redireciona para a página correta (evita loop)
            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                // Verifica se há um ReturnUrl (ex: /adm/Usuarios.aspx)
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    // Se não houver, vai para o painel padrão
                    RedirecionarPorTipo(Session["TipoUsuario"]?.ToString() ?? "");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text.Trim();

            try
            {
                using (UsuarioBLL usuarioBLL = new UsuarioBLL())
                {
                    Usuario usuario = usuarioBLL.Autenticar(email, senha);

                    if (usuario != null)
                    {
                        // ==============================
                        // 🔹 1. Salva dados do usuário na Session
                        // ==============================
                        Session["UsuarioId"] = usuario.Id;
                        Session["TipoUsuario"] = usuario.TipoUsuario;
                        Session["NomeUsuario"] = usuario.Nome;

                       
                        FormsAuthentication.SetAuthCookie(usuario.Email, false);
                        bool manterConectado = chkLembrar.Checked;
                        FormsAuthentication.SetAuthCookie(usuario.Email, manterConectado);
                        string returnUrl = Request.QueryString["ReturnUrl"];

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            Response.Redirect(returnUrl, false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            RedirecionarPorTipo(usuario.TipoUsuario);
                        }
                    }
                    else
                    {
                        Usuario usuarioExistente = usuarioBLL.ObterUsuarioPorEmail(email);
                        if (usuarioExistente != null && usuarioExistente.Bloqueado)
                        {
                            ExibirMensagem("Sua conta está bloqueada. Contate o suporte.", "danger");
                        }
                        else
                        {
                            ExibirMensagem("Email ou senha incorretos.", "warning");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro ao processar o login: " + ex.Message, "danger");
            }
        }

        /// Redireciona o usuário para a página padrão com base no seu tipo.
      
        private void RedirecionarPorTipo(string tipo)
        {
            string urlDestino;
            switch (tipo.ToLower())
            {
                case "admin":
                case "administrador":
                    urlDestino = "~/adm/Dashboard.aspx";
                    break;
                default:
                    urlDestino = "~/Cliente/Default.aspx";
                    break;
            }
            Response.Redirect(urlDestino, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            pnlMensagem.CssClass = $"alert alert-{tipo}";
            lblMensagem.Text = mensagem;
        }
    }
}