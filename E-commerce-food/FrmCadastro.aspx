<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCadastro.aspx.cs" Inherits="E_commerce_food.adm.FrmCadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Cadastro</title>
    <link href="../css/bootstrap.css" rel="stylesheet" />
    <link href="../css/FrmCadastro.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card shadow">
                        <div class="card-body p-5">
                            <div class="text-center mb-4">
                                <i class="bi bi-person-plus-fill text-primary" style="font-size: 4rem;"></i>
                                <h2 class="mt-3">Criar Conta</h2>
                                <p class="text-muted">Cadastre-se para fazer pedidos</p>
                            </div>

                            <!-- Painel de Mensagem -->
                            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                                <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                            </asp:Panel>

                            <!-- Campo Nome -->
                            <div class="mb-3">
                                <label for="txtNome" class="form-label">
                                    <i class="bi bi-person"></i> Nome Completo
                                </label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" 
                                    placeholder="Seu nome completo" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNome" runat="server" 
                                    ControlToValidate="txtNome" 
                                    ErrorMessage="Nome é obrigatório" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Campo Email -->
                            <div class="mb-3">
                                <label for="txtEmail" class="form-label">
                                    <i class="bi bi-envelope"></i> Email
                                </label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                    TextMode="Email" placeholder="seu@email.com" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail" 
                                    ErrorMessage="Email é obrigatório" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                    ControlToValidate="txtEmail" 
                                    ErrorMessage="Email inválido" 
                                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic">
                                </asp:RegularExpressionValidator>
                            </div>

                            <!-- Campo Telefone -->
                            <div class="mb-3">
                                <label for="txtTelefone" class="form-label">
                                    <i class="bi bi-telephone"></i> Telefone
                                </label>
                                <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control" 
                                    placeholder="(00) 00000-0000" MaxLength="15"></asp:TextBox>
                            </div>

                            <!-- Campo Endereço -->
                            <div class="mb-3">
                                <label for="txtEndereco" class="form-label">
                                    <i class="bi bi-geo-alt"></i> Endereço
                                </label>
                                <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control" 
                                    placeholder="Rua, número, bairro" MaxLength="200"></asp:TextBox>
                            </div>

                            <!-- Campo Senha -->
                            <div class="mb-3">
                                <label for="txtSenha" class="form-label">
                                    <i class="bi bi-lock"></i> Senha
                                </label>
                                <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" 
                                    TextMode="Password" placeholder="Mínimo 6 caracteres"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSenha" runat="server" 
                                    ControlToValidate="txtSenha" 
                                    ErrorMessage="Senha é obrigatória" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <!-- Campo Confirmar Senha -->
                            <div class="mb-3">
                                <label for="txtConfirmarSenha" class="form-label">
                                    <i class="bi bi-lock-fill"></i> Confirmar Senha
                                </label>
                                <asp:TextBox ID="txtConfirmarSenha" runat="server" CssClass="form-control" 
                                    TextMode="Password" placeholder="Digite a senha novamente"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConfirmarSenha" runat="server" 
                                    ControlToValidate="txtConfirmarSenha" 
                                    ErrorMessage="Confirmação de senha é obrigatória" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvSenhas" runat="server" 
                                    ControlToValidate="txtConfirmarSenha" 
                                    ControlToCompare="txtSenha" 
                                    ErrorMessage="As senhas não coincidem" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic">
                                </asp:CompareValidator>
                            </div>

                            <!-- Botão Cadastrar -->
                            <div class="d-grid gap-2">
                                <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" 
                                    CssClass="btn btn-primary btn-lg" OnClick="btnCadastrar_Click" />
                            </div>

                            <hr class="my-4" />

                            <!-- Link para Login -->
                            <div class="text-center">
                                <p class="mb-0">Já tem uma conta?</p>
                                <a href="FrmLogin.aspx" class="btn btn-outline-primary mt-2">
                                    Fazer Login
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js" integrity="sha384-ndDqU0Gzau9qJ1lfW4pNLlhNTkCfHzAVBReH9diLvGRem5+R9g2FzA8ZGN954O5Q" crossorigin="anonymous"></script>
    <script src="../js/Página.js"></script>
</body>
</html>