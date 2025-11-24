<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdutosEditar.aspx.cs" Inherits="E_commerce_food.adm.ProdutosEditar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Editar Produto</title>
    <link href="../css/bootstrap.css" rel="stylesheet" />
    <!-- Bootstrap 5 (CDN para garantir estilos) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-5">

        <div class="card shadow p-4 mx-auto" style="max-width: 900px;">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h3 class="mb-0">Editar Produto</h3>
                <a href="FrmProdutos.aspx" class="btn btn-outline-secondary">Voltar</a>
            </div>

            <!-- Mensagens -->
            <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert"></asp:Panel>

            <div class="row">
                <!-- Coluna Esquerda: Dados -->
                <div class="col-md-8">
                    
                    <div class="mb-3">
                        <label class="form-label fw-bold">Nome</label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Descrição</label>
                        <asp:TextBox ID="txtDescricao" TextMode="MultiLine" Rows="4" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Categoria</label>
                        <!-- 
                             🔹 CORREÇÃO AQUI TAMBÉM 🔹
                             Lista idêntica ao cadastro para evitar erros na edição
                        -->
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Lanche">Lanche</asp:ListItem>
                            <asp:ListItem Value="Pizza">Pizza</asp:ListItem>
                            <asp:ListItem Value="Bebida">Bebida</asp:ListItem>
                            <asp:ListItem Value="Sobremesa">Sobremesa</asp:ListItem>
                            <asp:ListItem Value="Combo">Combo</asp:ListItem>
                            <asp:ListItem Value="Acompanhamento">Acompanhamento</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-bold">Preço</label>
                            <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-bold">Estoque</label>
                            <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <div class="form-check mb-3">
                        <asp:CheckBox ID="chkDisponivel" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label">Disponível para venda</label>
                    </div>

                </div>

                <!-- Coluna Direita: Imagem -->
                <div class="col-md-4 text-center">
                    <label class="form-label fw-bold">Imagem do Produto</label>
                    <asp:TextBox ID="txtImagemUrl" runat="server" CssClass="form-control mb-2" placeholder="URL da imagem" />
                    
                    <div class="border rounded p-2 bg-white">
                        <asp:Image ID="imgPreview" runat="server" CssClass="img-fluid" Height="200" style="object-fit:contain;" onerror="this.src='https://placehold.co/200x200?text=Sem+Imagem';" />
                    </div>

                    <script>
                        function atualizarPreview() {
                            var url = document.getElementById("<%= txtImagemUrl.ClientID %>").value;
                            document.getElementById("<%= imgPreview.ClientID %>").src = url;
                        }
                    </script>

                    <asp:Button ID="btnAtualizarPreview" runat="server" CssClass="btn btn-sm btn-secondary mt-2 w-100" Text="Testar Imagem" OnClientClick="atualizarPreview(); return false;" />
                </div>

            </div>

            <div class="mt-4 d-grid gap-2 d-md-flex justify-content-md-end">
                <asp:Button ID="btnSalvar" runat="server" CssClass="btn btn-success btn-lg px-5" Text="Salvar Alterações" OnClick="btnSalvar_Click" />
            </div>

        </div>

    </form>
</body>
</html>