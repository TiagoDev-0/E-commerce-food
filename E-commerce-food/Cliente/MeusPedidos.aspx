<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MeusPedidos.aspx.cs" Inherits="E_commerce_food.Cliente.MeusPedidos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Meus Pedidos - FoodExpress</title>
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="../css/bootstrap.css" rel="stylesheet" />
    <link href="../css/Nav-footer.css" rel="stylesheet" />
    <link href="../css/Cards.css" rel="stylesheet" />
    <link href="../css/Theme.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <header class="p-3 text-white navbar-custom">
            <div class="container">
                <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                    <a href="Default.aspx" class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none me-3">
                        <img src="../img/Logo-FoodExpress.png" alt="Logo" style="height:40px; width:auto;" onerror="this.style.display='none'"/>
                    </a>
                    <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
                        <li><a href="Default.aspx" class="nav-link px-2 text-white">Início</a></li>
                        <li><a href="Comidas.aspx" class="nav-link px-2 text-white">Comidas</a></li>
                        <li><a href="Bebidas.aspx" class="nav-link px-2 text-white">Bebidas</a></li>
                        <li><a href="Sobremesas.aspx" class="nav-link px-2 text-white">Sobremesas</a></li>
                        <li><a href="Combos.aspx" class="nav-link px-2 text-white">Combos</a></li>
                    </ul>
                    <div class="text-end d-flex align-items-center">
                        <a href="Perfil.aspx" class="nav-link text-white me-3">
                            <span class="material-symbols-outlined" style="vertical-align: middle;">person</span>
                            Perfil
                        </a>
                        <a href="Carrinho.aspx" class="btn btn-warning position-relative me-3">
                            <span class="material-symbols-outlined">shopping_cart</span>
                            <asp:Label ID="lblCarrinhoCount" runat="server" Text="0" 
                                CssClass="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" />
                        </a>
                        <asp:LinkButton ID="btnSair" runat="server" CssClass="btn btn-outline-light btn-sm" OnClick="btnSair_Click">
                            <span class="material-symbols-outlined">logout</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </header>

        <main class="container my-5">
            <h2 class="mb-4"><i class="bi bi-clock-history"></i> Meus Pedidos</h2>

            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Panel ID="pnlSemPedidos" runat="server" Visible="false" CssClass="text-center py-5">
                <span class="material-symbols-outlined" style="font-size: 4rem; color: #ccc;">receipt_long</span>
                <h4 class="mt-3">Você ainda não fez nenhum pedido.</h4>
                <a href="Comidas.aspx" class="btn btn-primary mt-3">Fazer meu primeiro pedido</a>
            </asp:Panel>

            <div class="row g-4">
                <asp:Repeater ID="rptPedidos" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-lg-4">
                            <div class="card h-100 shadow-sm order-card status-<%# Eval("Status").ToString().ToLower().Replace(" ", "") %>">
                                <div class="card-header bg-white d-flex justify-content-between align-items-center">
                                    <span class="fw-bold">Pedido #<%# Eval("Id") %></span>
                                    <small class="text-muted"><%# Eval("DataPedido", "{0:dd/MM/yyyy HH:mm}") %></small>
                                </div>
                                <div class="card-body">
                                    <h5 class="card-title mb-3">
                                        <span class='badge rounded-pill bg-<%# GetStatusColor(Eval("Status").ToString()) %>'>
                                            <%# Eval("Status") %>
                                        </span>
                                    </h5>
                                    
                                    <p class="card-text text-muted mb-2">
                                        <strong>Itens:</strong><br/>
                                        <%# GetResumoItens(Eval("Itens")) %>
                                    </p>

                                    <h4 class="text-success mt-3"><%# Eval("ValorTotal", "{0:C}") %></h4>
                                </div>
                                <div class="card-footer bg-white text-end">
                                     <small class="text-muted">Obrigado pela preferência!</small>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </main>
    </form>
    <script src="../js/bootstrap.bundle.min.js"></script>
</body>
</html>
