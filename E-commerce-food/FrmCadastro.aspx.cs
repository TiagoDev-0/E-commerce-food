using E_commerce_food.BLL;
using E_commerce_food.Models;
using System;
using System.Web.UI;

namespace E_commerce_food.adm
{
    public partial class FrmCadastro : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 Se já estiver logado, redireciona para o painel
            if (Session["UsuarioId"] != null)
            {
                // Verifica o tipo de usuário antes de redirecionar
                if (Session["TipoUsuario"] != null && Session["TipoUsuario"].ToString().ToLower() == "Administrador")
                {
                    Response.Redirect("~/admin/Dashboard.aspx");
                }
                else
                {
                    Response.Redirect("~/FrmLogin.aspx"); // Redireciona para área de cliente
                }
                return;
            }

            pnlMensagem.Visible = false; // Garante que o painel comece invisível
        }

        private void ExibirMensagem(string mensagem, string tipo)
        {
            pnlMensagem.Visible = true;
            // Usando o formato alert-custom para melhor estilo, se existir no CSS
            pnlMensagem.CssClass = $"alert alert-custom alert-{tipo}";
            lblMensagem.Text = mensagem;
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            txtEndereco.Text = "";
            txtSenha.Text = "";
            txtConfirmarSenha.Text = "";
        }

        protected void btnCadastrar_Click(object sender, EventArgs e) // Ajustado nome do método
        {
            try
            {
                string nome = txtNome.Text.Trim();
                string email = txtEmail.Text.Trim();
                string telefone = txtTelefone.Text.Trim();
                string endereco = txtEndereco.Text.Trim();
                string senha = txtSenha.Text;
                string confirmarSenha = txtConfirmarSenha.Text;

                // --- 1. Validações de Interface (Server-Side) ---
                if (string.IsNullOrWhiteSpace(nome) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(senha))
                {
                    ExibirMensagem("Preencha todos os campos obrigatórios.", "warning");
                    return;
                }

                if (senha != confirmarSenha)
                {
                    ExibirMensagem("As senhas não coincidem.", "danger");
                    return;
                }

                if (senha.Length < 6)
                {
                    ExibirMensagem("A senha deve ter no mínimo 6 caracteres.", "danger");
                    return;
                }

                // --- 2. Criação da Entidade ---
                Usuario novoCliente = new Usuario
                {
                    Nome = nome,
                    Email = email,
                    Telefone = telefone,
                    Endereco = endereco,
                    DataCadastro = DateTime.Now
                    // Outras propriedades são definidas no BLL (TipoUsuario, Bloqueado)
                };

                // --- 3. Chamada da Camada de Negócio (BLL) ---
                using (UsuarioBLL usuarioBLL = new UsuarioBLL())
                {
                    string mensagem;

                    // O BLL cuida da validação de email duplicado e HASHING da senha.
                    bool sucesso = usuarioBLL.CadastrarCliente(novoCliente, senha, out mensagem);

                    if (sucesso)
                    {
                        ExibirMensagem(mensagem + " Redirecionando para o login...", "success");
                        LimparCampos();

                        // Redireciona automaticamente após 2 segundos
                        Response.AddHeader("REFRESH", "2;URL=FrmLogin.aspx");
                    }
                    else
                    {
                        // Exibe mensagem de erro (gerada pela regra de negócio, ex: "Este email já está cadastrado.")
                        ExibirMensagem(mensagem, "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                // 🔥 Captura erros inesperados
                ExibirMensagem("Ocorreu um erro ao cadastrar: " + ex.Message, "danger");
            }
        }
    }
}