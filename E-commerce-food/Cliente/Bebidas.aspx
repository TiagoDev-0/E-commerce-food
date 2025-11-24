<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bebidas.aspx.cs" Inherits="E_commerce_food.Cliente.Bebidas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>FoodExpress - Bebidas</title>
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="../css/Cards.css" rel="stylesheet" />
    <link href="../css/Nav-footer.css" rel="stylesheet" />
    <link href="../css/Theme.css" rel="stylesheet" />
    <link href="../css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         <header class="p-3 text-white navbar-custom">
     <div class="container">
         <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
             <a href="Default.aspx" class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none me-3">
                 <img src="../img/Logo-FoodExpress.png" alt="Logo" style="height: 40px; width: auto;" onerror="this.style.display='none'" />
             </a>

             <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                 <li><a href="Default.aspx" class="nav-link px-2 text-white">Início</a></li>
                 <li><a href="Comidas.aspx" class="nav-link px-2 text-white">Comidas</a></li>
                 <li><a href="Bebidas.aspx" class="nav-link px-2 text-white">Bebidas</a></li>
                 <li><a href="Sobremesas.aspx" class="nav-link px-2 text-white">Sobremesas</a></li>
                 <li><a href="Combos.aspx" class="nav-link px-2 text-white">Combos</a></li>
             </ul>

             <asp:Panel ID="pnlBuscaHeader" runat="server" DefaultButton="btnBuscarHeader"
                 CssClass="col-12 col-lg-auto mb-3 mb-lg-0 me-lg-3 d-flex">
                 <asp:TextBox ID="txtBuscaHeader" runat="server" CssClass="form-control form-control-dark text-bg-dark" placeholder="Buscar..." />
                 <asp:Button ID="btnBuscarHeader" runat="server" Text="🔍"
                     CssClass="btn btn-secondary ms-2"
                     OnClick="btnBuscarHeader_Click"
                     Style="width: 0; height: 0; padding: 0; margin: 0; opacity: 0; border: 0;" />
             </asp:Panel>

             <div class="text-end d-flex align-items-center">
                 <button id="theme-toggle" type="button" class="btn btn-outline-light me-3">
                     <span class="material-symbols-outlined">dark_mode</span>
                 </button>

                 <asp:Panel ID="pnlVisitante" runat="server" Visible="false">
                     <a href="../FrmLogin.aspx" class="btn btn-outline-light me-2">Login</a>
                     <a href="FrmCadastro.aspx" class="btn btn-warning">Cadastre-se</a>
                 </asp:Panel>

                 <asp:Panel ID="pnlLogado" runat="server" Visible="false" CssClass="d-flex align-items-center">
                     <a href="Perfil.aspx" class="nav-link text-white me-3">
                         <span class="material-symbols-outlined">person</span>
                         Olá,
                         <asp:Label ID="lblNomeUsuario" runat="server" Text="Cliente"></asp:Label>
                     </a>
                     <a href="MeusPedidos.aspx" class="btn btn-outline-light btn-sm me-3" title="Ver histórico de pedidos">
                         <span class="material-symbols-outlined" style="vertical-align: middle;">receipt_long</span>
                         Pedidos
                     </a>
                     <a href="Carrinho.aspx" class="btn btn-warning position-relative me-3">
                         <span class="material-symbols-outlined">shopping_cart</span>
                         <asp:Label ID="lblCarrinhoCount" runat="server"
                             CssClass="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
                             Text="0" />
                     </a>
                     <asp:LinkButton ID="btnSair" runat="server" CssClass="btn btn-outline-light btn-sm" OnClick="btnSair_Click">
                         <span class="material-symbols-outlined">logout</span>
                     </asp:LinkButton>
                 </asp:Panel>
             </div>
         </div>
     </div>
 </header>


        <main class="container my-5">
            <h2 class="mb-4">Bebidas Geladas</h2>

            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Repeater ID="rptBebidas" runat="server" OnItemCommand="rptBebidas_OnItemCommand">
                <HeaderTemplate>
                    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
                </HeaderTemplate>

                <ItemTemplate>
                    <div class="col">
                        <div class="card h-100 product-card">

                            <asp:Image ID="imgProduto" runat="server"
                                ImageUrl='<%# "~/uploads/" + Eval("ImagemUrl") %>'
                                CssClass="card-img-top product-img"
                                alt='<%# Eval("Nome") %>'
                                onerror="this.src='https://placehold.co/400x300/EFEFEF/AAAAAA?text=Sem+Foto';" />

                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title"><%# Eval("Nome") %></h5>
                                <p class="card-text text-muted flex-grow-1"><%# Eval("Descricao") %></p>
                                <h4 class="card-text text-success mb-3"><%# Eval("Preco", "{0:C}") %></h4>

                                <asp:Button ID="btnAddCarrinho" runat="server"
                                    Text="Adicionar"
                                    CssClass="btn btn-primary w-100 mt-auto"
                                    CommandName="AddToCart"
                                    CommandArgument='<%# Eval("Id") %>'
                                    Enabled='<%# (bool)Eval("Disponivel") %>'
                                    ToolTip='<%# (bool)Eval("Disponivel") ? "Adicionar ao carrinho" : "Produto indisponível" %>' />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>

                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlSemProdutos" runat="server" Visible="false" CssClass="text-center py-5">
                <span class="material-symbols-outlined" style="font-size: 4rem; color: #ccc;">local_drink</span>
                <h4 class="mt-3">Nenhuma bebida encontrada</h4>
                <p class="text-muted">Estamos reabastecendo nosso estoque.</p>
            </asp:Panel>

        </main>
    </form>

    <script src="../js/Página.js"></script>

    <script src="../js/bootstrap.bundle.min.js"></script>
</body>
</html>
