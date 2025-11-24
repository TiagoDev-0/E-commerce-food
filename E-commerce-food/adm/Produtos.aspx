<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Produtos.aspx.cs" Inherits="E_commerce_food.Admin.Produtos" %>

<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gerenciar Produtos - Admin</title>
    
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    
    <!-- CSS Customizado -->
    
    <link href="../css/produtos.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        
        <!-- HEADER ADMIN -->
        <header class="admin-header">
            <div class="container">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h1><i class="bi bi-box-seam"></i> Gerenciar Produtos</h1>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="Dashboard.aspx">Dashboard</a></li>
                                <li class="breadcrumb-item active">Produtos</li>
                            </ol>
                        </nav>
                    </div>
                    <div>
                        <a href="Dashboard.aspx" class="btn btn-light me-2">
                            <i class="bi bi-arrow-left"></i> Voltar
                        </a>
                        <a href="Pedidos.aspx" class="btn btn-outline-light">
                            <i class="bi bi-receipt"></i> Pedidos
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
                        <i class="bi bi-box text-primary"></i>
                        <h3 class="stat-number text-primary" id="countTotal" runat="server">0</h3>
                        <p class="stat-label">Total de Produtos</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card disponiveis">
                        <i class="bi bi-check-circle text-success"></i>
                        <h3 class="stat-number text-success" id="countDisponiveis" runat="server">0</h3>
                        <p class="stat-label">Disponíveis</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card indisponiveis">
                        <i class="bi bi-x-circle text-danger"></i>
                        <h3 class="stat-number text-danger" id="countIndisponiveis" runat="server">0</h3>
                        <p class="stat-label">Indisponíveis</p>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-card categorias">
                        <i class="bi bi-tags text-info"></i>
                        <h3 class="stat-number text-info" id="countCategorias" runat="server">0</h3>
                        <p class="stat-label">Categorias</p>
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
                                placeholder="Buscar por nome ou categoria..." 
                                AutoPostBack="true" 
                                OnTextChanged="txtBusca_TextChanged">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlFiltroCategoria" runat="server" CssClass="form-select" 
                            AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlFiltroCategoria_SelectedIndexChanged">
                            <asp:ListItem Value="">Todas as Categorias</asp:ListItem>
                            <asp:ListItem Value="Lanches">Lanches</asp:ListItem>
                            <asp:ListItem Value="Bebidas">Bebidas</asp:ListItem>
                            <asp:ListItem Value="Acompanhamentos">Acompanhamentos</asp:ListItem>
                            <asp:ListItem Value="Sobremesas">Sobremesas</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3 text-end">
                        <button type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#modalNovoProduto">
                            <i class="bi bi-plus-circle"></i> Novo Produto
                        </button>
                    </div>
                </div>
            </div>

            <!-- TABELA DE PRODUTOS -->
            <div class="card-custom">
                <div class="card-header-custom">
                    <h4 class="mb-0">
                        <i class="bi bi-list-ul"></i> Lista de Produtos
                    </h4>
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvProdutos" runat="server" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="Id"
                        CssClass="table table-custom mb-0"
                        OnRowEditing="gvProdutos_RowEditing"
                        OnRowDeleting="gvProdutos_RowDeleting"
                        OnRowDataBound="gvProdutos_RowDataBound"
                        EmptyDataText="Nenhum produto encontrado">
                        
                        <EmptyDataTemplate>
                            <div class="empty-state">
                                <i class="bi bi-inbox"></i>
                                <h4>Nenhum produto cadastrado</h4>
                                <p class="text-muted">Clique em "Novo Produto" para adicionar.</p>
                            </div>
                        </EmptyDataTemplate>

                        <Columns>
                            <asp:TemplateField HeaderText="ID">
                                <ItemTemplate>
                                    <span class="produto-id">#<%# Eval("Id") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Imagem">
                                <ItemTemplate>
                                    <div class="produto-thumb">
                                        <img src='<%# ResolveUrl("~/Images/produtos/" + Eval("ImagemUrl")) %>' 
                                             alt='<%# Eval("Nome") %>'
                                             onerror="this.src='https://via.placeholder.com/80x80?text=Sem+Imagem'" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Nome" HeaderText="Nome" />

                            <asp:TemplateField HeaderText="Categoria">
                                <ItemTemplate>
                                    <span class="badge badge-categoria">
                                        <i class="bi bi-tag-fill me-1"></i>
                                        <%# Eval("Categoria") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Preço">
                                <ItemTemplate>
                                    <span class="preco-valor">
                                        R$ <%# Eval("Preco", "{0:N2}") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Estoque">
                                <ItemTemplate>
                                    <span class='badge badge-estoque badge-estoque-<%# GetEstoqueClass((int)Eval("Estoque")) %>'>
                                        <%# Eval("Estoque") %> un.
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <div class="form-check form-switch">
                                        <input class="form-check-input" type="checkbox" 
                                               checked='<%# Eval("Disponivel") %>' 
                                               disabled />
                                        <label class="form-check-label">
                                            <%# (bool)Eval("Disponivel") ? "Disponível" : "Indisponível" %>
                                        </label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ações">
                                <ItemTemplate>
                                    <div class="btn-group" role="group">
                                        <asp:Button ID="btnEditar" runat="server" 
                                            Text="Editar" 
                                            CssClass="btn btn-primary btn-sm btn-action"
                                            CommandName="Edit"
                                            CommandArgument='<%# Eval("Id") %>'
                                            ToolTip="Editar Produto" />
                                        
                                        <asp:Button ID="btnExcluir" runat="server" 
                                            Text="Excluir" 
                                            CssClass="btn btn-danger btn-sm btn-action"
                                            CommandName="Delete"
                                            CommandArgument='<%# Eval("Id") %>'
                                            OnClientClick="return confirm('Deseja realmente excluir este produto?');"
                                            ToolTip="Excluir Produto" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- MODAL - NOVO PRODUTO -->
        <div class="modal fade" id="modalNovoProduto" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header modal-header-custom">
                        <h5 class="modal-title">
                            <i class="bi bi-plus-circle"></i> Cadastrar Novo Produto
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label fw-bold">
                                    <i class="bi bi-card-text"></i> Nome do Produto
                                </label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" 
                                    placeholder="Ex: X-Burger" MaxLength="100">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">
                                    <i class="bi bi-tag"></i> Categoria
                                </label>
                                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Selecione...</asp:ListItem>
                                    <asp:ListItem Value="Lanches">Lanches</asp:ListItem>
                                    <asp:ListItem Value="Bebidas">Bebidas</asp:ListItem>
                                    <asp:ListItem Value="Acompanhamentos">Acompanhamentos</asp:ListItem>
                                    <asp:ListItem Value="Sobremesas">Sobremesas</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                <label class="form-label fw-bold">
                                    <i class="bi bi-card-text"></i> Descrição
                                </label>
                                <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3"
                                    placeholder="Descrição do produto..." MaxLength="500">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-bold">
                                    <i class="bi bi-currency-dollar"></i> Preço (R$)
                                </label>
                                <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" 
                                    placeholder="0,00" TextMode="Number" step="0.01">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-bold">
                                    <i class="bi bi-box"></i> Estoque
                                </label>
                                <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" 
                                    placeholder="0" TextMode="Number">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-bold">
                                    <i class="bi bi-image"></i> URL da Imagem
                                </label>
                                <asp:TextBox ID="txtImagemUrl" runat="server" CssClass="form-control" 
                                    placeholder="produto.jpg" MaxLength="200">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                <div class="form-check form-switch">
                                    <asp:CheckBox ID="chkDisponivel" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label fw-bold">
                                        Produto Disponível para Venda
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                            <i class="bi bi-x-circle"></i> Cancelar
                        </button>
                        <asp:Button ID="btnAdicionar" runat="server" Text="Salvar Produto" 
                            CssClass="btn btn-success" OnClick="btnAdicionar_Click" />
                    </div>
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

        // Fechar modal após submit com sucesso
        window.addEventListener('load', function() {
            var modal = bootstrap.Modal.getInstance(document.getElementById('modalNovoProduto'));
            if (modal && document.querySelector('.alert-success')) {
                modal.hide();
            }
        });
    </script>
</body>
</html>