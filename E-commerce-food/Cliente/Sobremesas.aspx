<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sobremesas.aspx.cs" Inherits="E_commerce_food.Cliente.Sobremesas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>FoodExpress - Sobremesas</title>
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
            <h2 class="mb-4 text-pink">Sobremesas & Doces</h2>

            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Repeater ID="rptSobremesas" runat="server" OnItemCommand="rptSobremesas_OnItemCommand">
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
                                onerror="this.src='https://placehold.co/400x300/FFE4E1/DB7093?text=Doce';" />

                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title"><%# Eval("Nome") %></h5>
                                <p class="card-text text-muted flex-grow-1"><%# Eval("Descricao") %></p>
                                <h4 class="card-text text-success mb-3"><%# Eval("Preco", "{0:C}") %></h4>

                                <asp:Button ID="btnAddCarrinho" runat="server"
                                    Text="Adicionar"
                                    CssClass="btn btn-outline-danger w-100 mt-auto"
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
                <span class="material-symbols-outlined" style="font-size: 4rem; color: #ccc;">cookie</span>
                <h4 class="mt-3">Nenhuma sobremesa encontrada</h4>
                <p class="text-muted">Estamos preparando novos doces para você!</p>
            </asp:Panel>

        </main>
    </form>
    <footer class="py-5 footer">
        <div class="container">
            <div class="row">

                <div class="col-md-4 mb-4 d-flex flex-column flex-md-row gap-4">

                    <div class="d-flex justify-content-center">
                        <img src="../img/Logo-FoodExpress.png" alt="Logo" class="logo-footer" />
                    </div>

                    <div class="d-flex gap-5 justify-content-center">
                        <div>
                            <h5>Sobre nós</h5>
                            <ul class="nav flex-column">
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Quem somos</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Nossa história</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Trabalhe conosco</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Contato</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Política de privacidade</a></li>
                            </ul>
                        </div>

                        <div>
                            <h5>Ajuda</h5>
                            <ul class="nav flex-column">
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Central de atendimento</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">FAQ</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Entregas</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Reembolsos</a></li>
                                <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Suporte</a></li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-4">
                    <h5>Cardápio</h5>
                    <ul class="nav flex-column">
                        <li class="nav-item mb-2"><a href="Comidas.aspx" class="nav-link p-0 text-body-secondary">Lanches</a></li>
                        <li class="nav-item mb-2"><a href="Bebidas.aspx" class="nav-link p-0 text-body-secondary">Bebidas</a></li>
                        <li class="nav-item mb-2"><a href="Sobremesas.aspx" class="nav-link p-0 text-body-secondary">Sobremesas</a></li>
                        <li class="nav-item mb-2"><a href="Combos.aspx" class="nav-link p-0 text-body-secondary">Combos</a></li>
                        <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-body-secondary">Promoções</a></li>
                    </ul>
                </div>

                <div class="col-md-4 mb-3">
                    <form>
                        <h5>Receba novidades</h5>
                        <p>Promoções, descontos e novidades fresquinhas do nosso cardápio.</p>
                        <div class="d-flex flex-column flex-sm-row w-100 gap-2">
                            <input id="newsletter1" type="email" class="form-control" placeholder="Seu e-mail" />
                            <button class="btn btn-primary" type="button">Inscrever-se</button>
                        </div>
                    </form>
                </div>
            </div>

            <div class="d-flex flex-column flex-sm-row justify-content-between py-4 my-4 border-top">
                <p class="mb-0">© 2025 FoodExpress. Todos os direitos reservados.</p>

                <ul class="list-unstyled d-flex">
                    <li class="ms-3"><a href="#" class="link-body-emphasis"><i class="bi bi-instagram" style="font-size: 1.4rem;"></i></a></li>
                    <li class="ms-3"><a href="#" class="link-body-emphasis"><i class="bi bi-facebook" style="font-size: 1.4rem;"></i></a></li>
                </ul>
            </div>
        </div>
    </footer>

    <script src="../js/Página.js"></script>

    <script src="../js/bootstrap.bundle.min.js"></script>
</body>
</html>
