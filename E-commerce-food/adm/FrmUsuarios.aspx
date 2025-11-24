<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUsuarios.aspx.cs" Inherits="E_commerce_food.adm.Usuarios" %>

<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gerenciar Usuários - Admin</title>

    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />

    <!-- CSS Customizado -->
    </head>
<body>
    <form id="form1" runat="server">

        <!-- HEADER ADMIN -->
        <header class="admin-header">
            <div class="container">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h1><i class="bi bi-people"></i>Gerenciar Usuários</h1>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="Dashboard.aspx">Dashboard</a></li>
                                <li class="breadcrumb-item active">Usuários</li>
                            </ol>
                        </nav>
                    </div>
                    <div>
                        <a href="Dashboard.aspx" class="btn btn-light me-2">
                            <i class="bi bi-arrow-left"></i>Voltar
                        </a>
                        <a href="FrmProdutos.aspx" class="btn btn-outline-light">
                            <i class="bi bi-box"></i>Produtos
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
                    <div class="stat-card total">
                        <i class="bi bi-people text-primary"></i>
                        <h3 class="stat-number text-primary" id="countTotal" runat="server">0</h3>
                        <p class="stat-label">Total de Usuários</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card ativos">
                        <i class="bi bi-person-check text-success"></i>
                        <h3 class="stat-number text-success" id="countAtivos" runat="server">0</h3>
                        <p class="stat-label">Ativos</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card bloqueados">
                        <i class="bi bi-person-lock text-danger"></i>
                        <h3 class="stat-number text-danger" id="countBloqueados" runat="server">0</h3>
                        <p class="stat-label">Bloqueados</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card novos">
                        <i class="bi bi-person-plus text-info"></i>
                        <h3 class="stat-number text-info" id="countNovos" runat="server">0</h3>
                        <p class="stat-label">Novos (7 dias)</p>
                    </div>
                </div>
            </div>

            <!-- MENSAGEM DE FEEDBACK -->
            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert alert-custom" role="alert">
                <i class="bi bi-check-circle-fill me-2"></i>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <!-- BARRA DE BUSCA E FILTROS -->
            <div class="search-section">
                <div class="row align-items-center">
                    <div class="col-md-6">
                        <div class="search-box">
                            <i class="bi bi-search"></i>
                            <asp:TextBox ID="txtBusca" runat="server" CssClass="form-control"
                                placeholder="Buscar por nome ou email..."
                                AutoPostBack="true"
                                OnTextChanged="txtBusca_TextChanged">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlFiltroStatus" runat="server" CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlFiltroStatus_SelectedIndexChanged">
                            <asp:ListItem Value="">Todos os Status</asp:ListItem>
                            <asp:ListItem Value="Ativo">Ativos</asp:ListItem>
                            <asp:ListItem Value="Bloqueado">Bloqueados</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3 text-end">
                        <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-outline-primary" OnClick="btnExportar_Click">
    <i class="bi bi-download"></i> Exportar CSV
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <!-- TABELA DE USUÁRIOS -->
            <div class="card-custom">
                <div class="card-header-custom">
                    <h4 class="mb-0">
                        <i class="bi bi-list-ul"></i>Lista de Usuários
                    </h4>
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvUsuarios" runat="server"
                        AutoGenerateColumns="False"
                        DataKeyNames="Id"
                        CssClass="table table-custom mb-0"
                        OnRowCommand="gvUsuarios_RowCommand"
                        OnRowDataBound="gvUsuarios_RowDataBound"
                        EmptyDataText="Nenhum usuário encontrado">

                        <EmptyDataTemplate>
                            <div class="empty-state">
                                <i class="bi bi-inbox"></i>
                                <h4>Nenhum usuário encontrado</h4>
                                <p class="text-muted">Não há usuários cadastrados no sistema.</p>
                            </div>
                        </EmptyDataTemplate>

                        <Columns>
                            <asp:TemplateField HeaderText="ID">
                                <ItemTemplate>
                                    <span class="user-id">#<%# Eval("Id") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Usuário">
                                <ItemTemplate>
                                    <div class="user-info">
                                        <div class="user-avatar">
                                            <i class="bi bi-person-circle"></i>
                                        </div>
                                        <div class="user-details">
                                            <strong><%# Eval("Nome") %></strong>
                                            <small class="text-muted d-block">
                                                <i class="bi bi-envelope"></i><%# Eval("Email") %>
                                            </small>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Contato">
                                <ItemTemplate>
                                    <div class="contact-info">
                                        <div>
                                            <i class="bi bi-telephone text-muted"></i>
                                            <span><%# Eval("Telefone") ?? "Não informado" %></span>
                                        </div>
                                        <div>
                                            <i class="bi bi-geo-alt text-muted"></i>
                                            <span>
                                                <span><%# FormatEndereco(Eval("Endereco")) %></span>
                                            </span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Data Cadastro">
                                <ItemTemplate>
                                    <i class="bi bi-calendar3 text-muted me-1"></i>
                                    <%# Eval("DataCadastro", "{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='badge badge-status badge-<%# (bool)Eval("Bloqueado") ? "bloqueado" : "ativo" %>'>
                                        <i class='bi bi-<%# (bool)Eval("Bloqueado") ? "lock" : "check-circle" %>'></i>
                                        <%# (bool)Eval("Bloqueado") ? "Bloqueado" : "Ativo" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ações">
                                <ItemTemplate>
                                    <div class="btn-group" role="group">
                                        <asp:Button ID="btnVerDetalhes" runat="server"
                                            Text="Detalhes"
                                            CssClass="btn btn-info btn-sm btn-action"
                                            CommandName="VerDetalhes"
                                            CommandArgument='<%# Eval("Id") %>'
                                            ToolTip="Ver Detalhes" />

                                        <asp:Button ID="btnBloquear" runat="server"
                                            Text='<%# (bool)Eval("Bloqueado") ? "Desbloquear" : "Bloquear" %>'
                                            CssClass='<%# "btn btn-sm btn-action " + ((bool)Eval("Bloqueado") ? "btn-success" : "btn-warning") %>'
                                            CommandName="Bloquear"
                                            CommandArgument='<%# Eval("Id") %>'
                                            ToolTip='<%# (bool)Eval("Bloqueado") ? "Desbloquear Usuário" : "Bloquear Usuário" %>' />

                                        <asp:Button ID="btnExcluir" runat="server"
                                            Text="Excluir"
                                            CssClass="btn btn-danger btn-sm btn-action"
                                            CommandName="Excluir"
                                            CommandArgument='<%# Eval("Id") %>'
                                            OnClientClick="return confirm('Deseja realmente excluir este usuário? Esta ação não pode ser desfeita.');"
                                            ToolTip="Excluir Usuário" />
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
        setTimeout(function () {
            var alert = document.querySelector('.alert-custom');
            if (alert) {
                alert.style.transition = 'opacity 0.5s';
                alert.style.opacity = '0';
                setTimeout(function () {
                    alert.style.display = 'none';
                }, 500);
            }
        }, 3000);
    </script>
</body>
</html>
