<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Carrinho.aspx.cs" Inherits="E_commerce_food.Cliente.Carrinho" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Meu Carrinho - FoodExpress</title>
    <link href="../css/bootstrap.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="../css/Default.css" rel="stylesheet" />
    <link href="../css/Nav-footer.css" rel="stylesheet" />
    <link href="../css/Theme.css" rel="stylesheet" />
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
            <h2 class="mb-4">Meu Carrinho</h2>
            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Panel ID="pnlCarrinhoCheio" runat="server" Visible="false">
                <div class="row g-4">
                    <div class="col-lg-8">
                        <div class="card shadow-sm">
                            <div class="card-header bg-white py-3">
                                <h5 class="mb-0">Itens do Pedido</h5>
                            </div>
                            <div class="card-body p-0">
                                <asp:Repeater ID="rptCarrinho" runat="server" OnItemCommand="rptCarrinho_ItemCommand">
                                    <ItemTemplate>
                                        <div class="row g-0 p-3 border-bottom align-items-center">
                                            <div class="col-2 col-md-1">
                                                <asp:Image runat="server" ImageUrl='<%# Eval("Produto.ImagemUrl") %>' alt='<%# Eval("Produto.Nome") %>'
                                                    CssClass="img-fluid rounded"
                                                    onerror="this.src='https://placehold.co/100x100/EFEFEF/AAAAAA?text=Item';" />
                                            </div>
                                            <div class="col-10 col-md-5 ps-3">
                                                <h6 class="mb-0"><%# Eval("Produto.Nome") %></h6>
                                                <small class="text-muted"><%# Eval("Produto.Categoria") %></small>
                                            </div>
                                            <div class="col-5 col-md-3 mt-2 mt-md-0">
                                                <div class="input-group input-group-sm" style="max-width: 120px;">
                                                    <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control text-center"
                                                        Text='<%# Eval("Quantidade") %>'></asp:TextBox>
                                                    <asp:Button ID="btnAtualizarQtd" runat="server" Text="OK" CssClass="btn btn-outline-secondary"
                                                        CommandName="UpdateQty" CommandArgument='<%# Eval("ProdutoId") %>' />
                                                </div>
                                            </div>
                                            <div class="col-4 col-md-2 mt-2 mt-md-0 text-end">
                                                <h6 class="mb-0"><%# Eval("Subtotal", "{0:C}") %></h6>
                                                <small class="text-muted">(<%# Eval("PrecoUnitario", "{0:C}") %> un)</small>
                                            </div>
                                            <div class="col-3 col-md-1 d-flex align-items-center justify-content-end">
                                                <asp:LinkButton ID="btnRemover"
                                                    runat="server"
                                                    CssClass="btn-remove"
                                                    CommandName="Remove"
                                                    CommandArgument='<%# Eval("ProdutoId") %>'
                                                    ToolTip="Remover item">
                                                    <span class="material-symbols-outlined" style="color: red;">delete</span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="card shadow-sm">
                            <div class="card-header bg-white py-3">
                                <h5 class="mb-0">Resumo</h5>
                            </div>
                            <div class="card-body">
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item d-flex justify-content-between">
                                        <span>Subtotal</span>
                                        <strong>
                                            <asp:Label ID="lblSubtotal" runat="server" Text="R$ 0,00"></asp:Label>
                                        </strong>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between bg-light">
                                        <h5 class="mb-0">Total</h5>
                                        <h5 class="mb-0 text-success">
                                            <asp:Label ID="lblTotal" runat="server" Text="R$ 0,00"></asp:Label>
                                        </h5>
                                    </li>
                                </ul>
                                <hr />
                                <div class="mb-3">
                                    <label for="txtObservacoes" class="form-label">Observações do Pedido</label>
                                    <asp:TextBox ID="txtObservacoes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"
                                        placeholder="Ex: Tirar a cebola, ponto da carne, etc."></asp:TextBox>
                                </div>
                            </div>
                            <div class="card-footer p-3 d-grid">
                                <asp:Button ID="btnFinalizarPedido" runat="server"
                                    Text="Finalizar Pedido"
                                    CssClass="btn btn-success btn-lg"
                                    OnClick="btnFinalizarPedido_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCarrinhoVazio" runat="server" Visible="true" CssClass="text-center py-5">
                <i class="bi bi-cart-x" style="font-size: 4rem;"></i>
                <h4 class="mt-3">Seu carrinho está vazio</h4>
                <p class="text-muted">Adicione itens do cardápio para continuar.</p>
                <a href="Comidas.aspx" class="btn btn-primary">Ver Cardápio</a>
            </asp:Panel>
        </main>
    </form>

    <script src="../js/bootstrap.bundle.min.js"></script>
    <script src="../js/Página.js"></script>
</body>
</html>
