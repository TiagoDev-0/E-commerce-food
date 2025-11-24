<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPedidos.aspx.cs" Inherits="E_commerce_food.adm.Pedidos" %>

<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gerenciar Pedidos - Admin</title>
    
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    
    <!-- CSS Customizado -->
    <link href="../css-adm/Pedidos.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        
        <!-- HEADER ADMIN -->
        <header class="admin-header">
            <div class="container">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h1><i class="bi bi-receipt-cutoff"></i> Gerenciar Pedidos</h1>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="Dashboard.aspx">Dashboard</a></li>
                                <li class="breadcrumb-item active">Pedidos</li>
                            </ol>
                        </nav>
                    </div>
                    <div>
                        <a href="Dashboard.aspx" class="btn btn-light">
                            <i class="bi bi-arrow-left"></i> Voltar
                        </a>
                    </div>
                </div>
            </div>
        </header>

        <!-- CONTAINER PRINCIPAL -->
        <div class="container pb-5">
            
            <!-- CARDS DE ESTATÍSTICAS -->
            <div class="row stats-cards g-3 mb-4">
                <div class="col-md-3">
                    <div class="stat-card pendente">
                        <i class="bi bi-clock-history text-warning"></i>
                        <h3 class="stat-number text-warning" id="countPendente" runat="server">0</h3>
                        <p class="stat-label">Pendentes</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card preparo">
                        <i class="bi bi-fire text-info"></i>
                        <h3 class="stat-number text-info" id="countPreparo" runat="server">0</h3>
                        <p class="stat-label">Em Preparo</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card entregue">
                        <i class="bi bi-check-circle text-success"></i>
                        <h3 class="stat-number text-success" id="countEntregue" runat="server">0</h3>
                        <p class="stat-label">Entregues</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card cancelado">
                        <i class="bi bi-x-circle text-danger"></i>
                        <h3 class="stat-number text-danger" id="countCancelado" runat="server">0</h3>
                        <p class="stat-label">Cancelados</p>
                    </div>
                </div>
            </div>

            <!-- MENSAGEM DE FEEDBACK -->
            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert alert-custom" role="alert">
                <i class="bi bi-check-circle-fill me-2"></i>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <!-- FILTROS -->
            <div class="filter-section">
                <div class="row align-items-center">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">
                            <i class="bi bi-funnel"></i> Filtrar por Status:
                        </label>
                        <asp:DropDownList ID="ddlFiltroStatus" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroStatus_SelectedIndexChanged">
                            <asp:ListItem Value="">Todos</asp:ListItem>
                            <asp:ListItem Value="Pendente">Pendente</asp:ListItem>
                            <asp:ListItem Value="Em Preparo">Em Preparo</asp:ListItem>
                            <asp:ListItem Value="Entregue">Entregue</asp:ListItem>
                            <asp:ListItem Value="Cancelado">Cancelado</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">
                            <i class="bi bi-search"></i> Buscar Pedido:
                        </label>
                        <asp:TextBox ID="txtBusca" runat="server" CssClass="form-control" placeholder="ID ou Nome do cliente"></asp:TextBox>
                    </div>
                    <div class="col-md-4 align-self-end">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary me-2" OnClick="btnBuscar_Click" />
                        <asp:Button ID="btnLimpar" runat="server" Text="Limpar" CssClass="btn btn-secondary" OnClick="btnLimpar_Click" />
                    </div>
                </div>
            </div>

            <!-- TABELA DE PEDIDOS -->
            <div class="card-custom">
                <div class="card-header-custom">
                    <h4 class="mb-0">
                        <i class="bi bi-list-ul"></i> Lista de Pedidos
                    </h4>
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvPedidos" runat="server" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="Id"
                        CssClass="table table-custom mb-0"
                        OnRowCommand="gvPedidos_RowCommand"
                        OnRowDataBound="gvPedidos_RowDataBound"
                        EmptyDataText="Nenhum pedido encontrado">
                        
                        <EmptyDataTemplate>
                            <div class="empty-state">
                                <i class="bi bi-inbox"></i>
                                <h4>Nenhum pedido encontrado</h4>
                                <p class="text-muted">Os pedidos aparecerão aqui quando forem realizados.</p>
                            </div>
                        </EmptyDataTemplate>

                        <Columns>
                            <asp:TemplateField HeaderText="ID">
                                <ItemTemplate>
                                    <span class="pedido-id">#<%# Eval("Id") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Usuario" HeaderText="Cliente" />
                            
                            <asp:TemplateField HeaderText="Data/Hora">
                                <ItemTemplate>
                                    <i class="bi bi-calendar3 text-muted me-1"></i>
                                    <%# Eval("DataPedido", "{0:dd/MM/yyyy}") %><br/>
                                    <small class="text-muted">
                                        <i class="bi bi-clock me-1"></i>
                                        <%# Eval("DataPedido", "{0:HH:mm}") %>
                                    </small>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='badge badge-status badge-<%# GetStatusClass(Eval("Status").ToString()) %>'>
                                        <%# Eval("Status") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Valor Total">
                                <ItemTemplate>
                                    <span class="valor-total">
                                        R$ <%# Eval("ValorTotal", "{0:N2}") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ações">
                                <ItemTemplate>
                                    <div class="btn-group" role="group">
                                        <asp:Button ID="btnPreparar" runat="server" 
                                            Text="Preparar" 
                                            CssClass="btn btn-warning btn-sm btn-action"
                                            CommandName="Preparar" 
                                            CommandArgument='<%# Eval("Id") %>'
                                            ToolTip="Marcar como Em Preparo" />
                                        
                                        <asp:Button ID="btnEntregar" runat="server" 
                                            Text="Entregar" 
                                            CssClass="btn btn-success btn-sm btn-action"
                                            CommandName="Entregar" 
                                            CommandArgument='<%# Eval("Id") %>'
                                            ToolTip="Marcar como Entregue" />
                                        
                                        <asp:Button ID="btnCancelar" runat="server" 
                                            Text="Cancelar" 
                                            CssClass="btn btn-danger btn-sm btn-action"
                                            CommandName="Cancelar" 
                                            CommandArgument='<%# Eval("Id") %>'
                                            OnClientClick="return confirm('Deseja realmente cancelar este pedido?');"
                                            ToolTip="Cancelar Pedido" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <script>
        // Auto-hide mensagem após 3 segundos
        setTimeout(function() {
            var alert = document.querySelector('.alert-custom');
            if (alert) {
                alert.style.transition = 'opacity 0.5s';
                alert.style.opacity = '0';
                setTimeout(function() {
                    alert.style.display = 'none';
                }, 500);
            }
        }, 3000);
    </script>
</body>
</html>