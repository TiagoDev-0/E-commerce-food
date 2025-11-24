<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmProdutos.aspx.cs" Inherits="E_commerce_food.adm.Produtos" %>

<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gerenciar Produtos - Admin</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="../css/admin.css" rel="stylesheet" />
    
    <script type="text/javascript">
        // Script para pré-visualizar a imagem antes de enviar
        function previewFile() {
            var preview = document.querySelector('#imgPreviewModal');
            var file = document.querySelector('#<%= fuImagem.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
                preview.style.display = 'block';
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
                preview.style.display = 'none';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <!-- HEADER -->
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
                        <a href="Dashboard.aspx" class="btn btn-light me-2"><i class="bi bi-arrow-left"></i> Voltar</a>
                        <a href="FrmPedidos.aspx" class="btn btn-outline-light"><i class="bi bi-receipt"></i> Pedidos</a>
                    </div>
                </div>
            </div>
        </header>

        <div class="container pb-5">
            
            <!-- ESTATÍSTICAS -->
            <div class="row stats-cards g-3 mb-4">
                <div class="col-md-3">
                    <div class="card p-3 shadow-sm border-0 bg-primary text-white">
                        <h3><span id="countTotal" runat="server">0</span></h3>
                        <small>Total de Produtos</small>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card p-3 shadow-sm border-0 bg-success text-white">
                        <h3><span id="countDisponiveis" runat="server">0</span></h3>
                        <small>Disponíveis</small>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card p-3 shadow-sm border-0 bg-danger text-white">
                        <h3><span id="countIndisponiveis" runat="server">0</span></h3>
                        <small>Indisponíveis</small>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card p-3 shadow-sm border-0 bg-info text-white">
                        <h3><span id="countCategorias" runat="server">0</span></h3>
                        <small>Categorias</small>
                    </div>
                </div>
            </div>

            <!-- BARRA DE FERRAMENTAS -->
            <div class="card p-3 mb-4 shadow-sm border-0">
                <div class="row g-3">
                    <div class="col-md-5">
                        <div class="input-group">
                            <span class="input-group-text"><i class="bi bi-search"></i></span>
                            <asp:TextBox ID="txtBusca" runat="server" CssClass="form-control" 
                                placeholder="Buscar produto..." AutoPostBack="true" OnTextChanged="txtBusca_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <!-- FILTRO NO SINGULAR (Pizza e Acompanhamento removidos) -->
                        <asp:DropDownList ID="ddlFiltroCategoria" runat="server" CssClass="form-select" 
                            AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroCategoria_SelectedIndexChanged">
                            <asp:ListItem Value="">Todas as Categorias</asp:ListItem>
                            <asp:ListItem Value="Lanche">Lanches</asp:ListItem>
                            <asp:ListItem Value="Bebida">Bebidas</asp:ListItem>
                            <asp:ListItem Value="Sobremesa">Sobremesas</asp:ListItem>
                            <asp:ListItem Value="Combo">Combos</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3 text-end">
                        <button type="button" class="btn btn-success w-100" data-bs-toggle="modal" data-bs-target="#modalNovoProduto">
                            <i class="bi bi-plus-lg"></i> Novo Produto
                        </button>
                    </div>
                </div>
            </div>

            <!-- TABELA -->
            <div class="card shadow-sm border-0 overflow-hidden">
                <asp:GridView ID="gvProdutos" runat="server" 
                    AutoGenerateColumns="False" 
                    DataKeyNames="Id"
                    CssClass="table table-hover mb-0 align-middle"
                    OnRowEditing="gvProdutos_RowEditing"
                    OnRowDeleting="gvProdutos_RowDeleting"
                    OnRowDataBound="gvProdutos_RowDataBound"
                    EmptyDataText="Nenhum produto encontrado."
                    GridLines="None">
                    
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="#" HeaderStyle-Width="50px" />
                        
                        <asp:TemplateField HeaderText="Produto">
                            <ItemTemplate>
                                <div class="d-flex align-items-center">
                                    <!-- IMAGEM DA PASTA UPLOADS -->
                                    <img src='<%# ResolveUrl("~/uploads/" + Eval("ImagemUrl")) %>' alt="" 
                                         style="width:50px; height:50px; object-fit:cover; border-radius:8px; margin-right:15px;" 
                                         onerror="this.src='https://placehold.co/50x50?text=Img';"/>
                                    <div>
                                        <div class="fw-bold"><%# Eval("Nome") %></div>
                                        <small class="text-muted"><%# Eval("Categoria") %></small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Preco" HeaderText="Preço" DataFormatString="{0:C}" HeaderStyle-Width="100px" />
                        
                        <asp:TemplateField HeaderText="Estoque" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <span class='badge bg-<%# GetEstoqueClass((int)Eval("Estoque")) %>'>
                                    <%# Eval("Estoque") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <div class="form-check form-switch">
                                    <input class="form-check-input" type="checkbox" checked='<%# Eval("Disponivel") %>' disabled />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ações" HeaderStyle-Width="150px" ItemStyle-CssClass="text-end">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-primary me-1" 
                                    CommandName="Edit" ToolTip="Editar">
                                    <i class="bi bi-pencil"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExcluir" runat="server" CssClass="btn btn-sm btn-outline-danger" 
                                    CommandName="Delete" OnClientClick="return confirm('Tem certeza que deseja excluir este produto?');" ToolTip="Excluir">
                                    <i class="bi bi-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>

        <!-- MODAL NOVO PRODUTO -->
        <div class="modal fade" id="modalNovoProduto" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title"><i class="bi bi-box-seam"></i> Cadastrar Novo Produto</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Nome</label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" placeholder="Ex: X-Bacon"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Categoria</label>
                                <!-- CATEGORIAS NO SINGULAR (Removidos Pizza e Acompanhamento) -->
                                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">Selecione...</asp:ListItem>
                                    <asp:ListItem Value="Lanche">Lanche</asp:ListItem>
                                    <asp:ListItem Value="Bebida">Bebida</asp:ListItem>
                                    <asp:ListItem Value="Sobremesa">Sobremesa</asp:ListItem>
                                    <asp:ListItem Value="Combo">Combo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-bold">Descrição</label>
                                <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Preço (R$)</label>
                                <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Estoque</label>
                                <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                            </div>
                            
                            <!-- UPLOAD DE IMAGEM -->
                            <div class="col-md-12">
                                <label class="form-label fw-bold">Imagem do Produto</label>
                                <div class="input-group">
                                    <asp:FileUpload ID="fuImagem" runat="server" CssClass="form-control" accept=".png,.jpg,.jpeg,.gif" onchange="previewFile()" />
                                </div>
                                <div class="mt-2 text-center">
                                    <img id="imgPreviewModal" src="" alt="Pré-visualização" style="max-height: 150px; display: none;" class="img-thumbnail" />
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="form-check form-switch">
                                    <asp:CheckBox ID="chkDisponivel" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label">Produto Disponível</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnAdicionar" runat="server" Text="Salvar Produto" CssClass="btn btn-success" OnClick="btnAdicionar_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- MENSAGEM FLUTUANTE -->
        <asp:Panel ID="pnlMensagem" runat="server" Visible="false" role="alert">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </asp:Panel>

    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>