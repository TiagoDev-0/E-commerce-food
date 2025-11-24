using System;
using System.Linq;
using E_commerce_food.Models;

namespace E_commerce_food.adm
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            // ?? Proteção: só Admin entra
            ValidarAcesso();

            if (!IsPostBack)
                AtualizarResumo();
        }

        private void ValidarAcesso()
        {
          
            // 2. Verifica se é Admin
            string tipo = Session["TipoUsuario"]?.ToString().ToLower() ?? "";

            if (tipo != "administrador" && tipo != "admin")
            {
                // Usuário comum tentando entrar no Admin
                Response.Redirect("~/Default.aspx");
                return;
            }
        }

        private void AtualizarResumo()
        {
            // Contadores principais
            lblProdutos.Text = db.Produtos.Count().ToString();
            lblPedidos.Text = db.Pedidos.Count().ToString();

            // Contagem de usuários por status
            lblUsuariosAtivos.Text = db.Usuarios.Count(u => !u.Bloqueado).ToString();
            lblUsuariosBloqueados.Text = db.Usuarios.Count(u => u.Bloqueado).ToString();
        }
    }
}
