using System;
using System.Linq;
using System.Data.Entity;          // <--- necessário para Include
using E_commerce_food.Models;

namespace E_commerce_food.Admin
{
    public partial class Clientes : System.Web.UI.Page
    {
        ApplicationDbContext db = new ApplicationDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarClientes();
        }

        private void CarregarClientes()
        {
            // Carrega Cliente com a entidade Usuario relacionada
            var lista = db.Clientes
                          .Include(c => c.Usuario)   // garante que Usuario não será null no Select
                          .Select(c => new
                          {
                              Id = c.Id,
                              Nome = c.Usuario != null ? c.Usuario.Nome : "",   // Nome vem de Usuario
                              Email = c.Usuario != null ? c.Usuario.Email : "",
                              Telefone = c.Telefone,
                              Ativo = c.Usuario != null ? c.Usuario.Ativo : false
                          })
                          .ToList();

            gvClientes.DataSource = lista;
            gvClientes.DataBind();
        }

        protected void gvClientes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Bloquear")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var cliente = db.Clientes.Include(c => c.Usuario).FirstOrDefault(c => c.Id == id);

                if (cliente != null && cliente.Usuario != null)
                {
                    // altera o campo Ativo dentro de Usuario
                    cliente.Usuario.Ativo = !cliente.Usuario.Ativo;
                    db.SaveChanges();

                    lblMsg.Text = cliente.Usuario.Ativo
                        ? "Cliente desbloqueado com sucesso!"
                        : "Cliente bloqueado com sucesso!";

                    CarregarClientes();
                }
                else
                {
                    lblMsg.Text = "Registro inválido ou usuário não encontrado.";
                }
            }
        }
    }
}
