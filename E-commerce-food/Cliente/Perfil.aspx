<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="E_commerce_food.Cliente.Perfil" %>

<!DOCTYPE html>
<html lang="pt-BR">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Meu Perfil - E-commerce Food</title>

    <link href="../css/bootstrap.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="../css/Nav&footer.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        
      
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <div class="container">
                <a class="navbar-brand" href="Default.aspx">E-Food</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="Comidas.aspx">Cardápio</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Carrinho.aspx"><i class="bi bi-cart"></i> Carrinho</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link active" href="Perfil.aspx"><i class="bi bi-person"></i> Perfil</a>
                        </li>
                        <li class="nav-item">
                            <asp:LinkButton ID="btnSair" runat="server" CssClass="nav-link" OnClick="btnSair_Click"><i class="bi bi-box-arrow-right"></i> Sair</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- CONTAINER PRINCIPAL -->
        <div class="container my-5">
            <div class="row justify-content-center">
                <div class="col-lg-10">

                    <!-- Mensagem de Feedback -->
                    <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </asp:Panel>

                    <!-- Card 1: Meus Dados -->
                    <div class="card shadow-sm mb-4">
                        <div class="card-header bg-white py-3">
                            <h4 class="mb-0"><i class="bi bi-person-lines-fill me-2"></i>Meus Dados</h4>
                        </div>
                        <div class="card-body p-4">
                            <div class="row g-3">
                                <!-- Nome -->
                                <div class="col-md-6">
                                    <label for="txtNome" class="form-label">Nome Completo</label>
                                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNome" runat="server"
                                        ControlToValidate="txtNome" ErrorMessage="O nome é obrigatório."
                                        CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <!-- Email (Não editável) -->
                                <div class="col-md-6">
                                    <label for="txtEmail" class="form-label">Email (Login)</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" ToolTip="Seu email de login não pode ser alterado."></asp:TextBox>
                                </div>
                                <!-- Telefone -->
                                <div class="col-md-6">
                                    <label for="txtTelefone" class="form-label">Telefone / WhatsApp</label>
                                    <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control" placeholder="(11) 98765-4321"></asp:TextBox>
                                </div>
                                <!-- Endereço -->
                                <div class="col-12">
                                    <label for="txtEndereco" class="form-label">Endereço Principal (para entregas)</label>
                                    <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"
                                        placeholder="Rua Exemplo, 123, Bairro, Cidade - SP"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer bg-white text-end p-3">
                            <asp:Button ID="btnSalvarDados" runat="server" 
                                Text="Salvar Alterações" 
                                CssClass="btn btn-primary" 
                                OnClick="btnSalvarDados_Click" />
                        </div>
                    </div>

                    <!-- Card 2: Alterar Senha -->
                    <div class="card shadow-sm">
                        <div class="card-header bg-white py-3">
                            <h4 class="mb-0"><i class="bi bi-key-fill me-2"></i>Alterar Senha</h4>
                        </div>
                        <div class="card-body p-4">
                             <div class="row g-3">
                                <!-- Senha Atual -->
                                <div class="col-md-4">
                                    <label for="txtSenhaAtual" class="form-label">Senha Atual</label>
                                    <asp:TextBox ID="txtSenhaAtual" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                 <!-- Nova Senha -->
                                <div class="col-md-4">
                                    <label for="txtNovaSenha" class="form-label">Nova Senha</label>
                                    <asp:TextBox ID="txtNovaSenha" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                 <!-- Confirmar Nova Senha -->
                                <div class="col-md-4">
                                    <label for="txtConfirmarNovaSenha" class="form-label">Confirmar Nova Senha</label>
                                    <asp:TextBox ID="txtConfirmarNovaSenha" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer bg-white text-end p-3">
                            <asp:Button ID="btnAlterarSenha" runat="server" 
                                Text="Alterar Senha" 
                                CssClass="btn btn-secondary" 
                                OnClick="btnAlterarSenha_Click" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>