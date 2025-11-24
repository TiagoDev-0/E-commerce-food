<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsuarioDetalhes.aspx.cs" Inherits="E_commerce_food.adm.UsuarioDetalhes" %>

<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Editar Usuário - Admin</title>

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
                        <h1><i class="bi bi-person-fill-gear"></i>Editar Usuário</h1>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="Dashboard.aspx">Dashboard</a></li>
                                <li class="breadcrumb-item"><a href="FrmUsuarios.aspx">Usuários</a></li>
                                <li class="breadcrumb-item active">Editar</li>
                            </ol>
                        </nav>
                    </div>
                    <div>
                        <a href="FrmUsuarios.aspx" class="btn btn-light">
                            <i class="bi bi-arrow-left"></i>Voltar para Lista
                        </a>
                    </div>
                </div>
            </div>
        </header>

        <!-- CONTAINER PRINCIPAL -->
        <div class="container pb-5 mt-4">

            <!-- MENSAGEM DE FEEDBACK -->
            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </asp:Panel>

            <div class="card card-custom shadow-sm">
                <div class="card-body p-4 p-md-5">

                    <div class="row g-4">
                        <!-- Coluna 1: Campos de Edição -->
                        <div class="col-lg-8">
                            <h4 class="mb-4">Informações Pessoais</h4>
                            
                            <!-- Nome -->
                            <div class="mb-3">
                                <label for="txtNome" class="form-label">Nome Completo</label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNome" runat="server"
                                    ControlToValidate="txtNome"
                                    ErrorMessage="O nome é obrigatório."
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <!-- Email (Não editável) -->
                            <div class="mb-3">
                                <label for="txtEmail" class="form-label">Email (Login)</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" ToolTip="O email não pode ser alterado."></asp:TextBox>
                            </div>

                            <!-- Telefone -->
                            <div class="mb-3">
                                <label for="txtTelefone" class="form-label">Telefone</label>
                                <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control" placeholder="(11) 98765-4321"></asp:TextBox>
                            </div>

                            <!-- Endereço -->
                            <div class="mb-3">
                                <label for="txtEndereco" class="form-label">Endereço</label>
                                <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"
                                    placeholder="Rua Exemplo, 123, Bairro, Cidade - SP"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Coluna 2: Status e Ações -->
                        <div class="col-lg-4">
                            <h4 class="mb-4">Status da Conta</h4>
                            
                            <div class="card bg-light border-0 mb-3">
                                <div class="card-body">
                                    <div class="mb-3">
                                        <p class="mb-1 text-muted">Data de Cadastro:</p>
                                        <h5>
                                            <i class="bi bi-calendar-check"></i>
                                            <asp:Label ID="lblDataCadastro" runat="server" Text="N/A"></asp:Label>
                                        </h5>
                                    </div>
                                    <div>
                                        <p class="mb-1 text-muted">Status Atual:</p>
                                        <h5>
                                            <asp:Label ID="lblStatus" runat="server" Text="N/A"></asp:Label>
                                        </h5>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="d-grid gap-2">
                                <asp:Button ID="btnBloquear" runat="server" 
                                    Text="Bloquear Usuário" 
                                    CssClass="btn btn-warning" 
                                    OnClick="btnBloquear_Click" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="card-footer bg-white p-3 text-end">
                    <a href="FrmUsuarios.aspx" class="btn btn-secondary me-2">Cancelar</a>
                    <asp:Button ID="btnSalvar" runat="server" 
                        Text="Salvar Alterações" 
                        CssClass="btn btn-primary" 
                        OnClick="btnSalvar_Click" />
                </div>
            </div>

        </div>
    </form>
</body>
</html>