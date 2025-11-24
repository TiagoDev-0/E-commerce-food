<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLoginAdm.aspx.cs" Inherits="E_commerce_food.adm.FrmLoginAdm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
    <link href="../css/bootstrap.css" rel="stylesheet" />
    <link href="../css/FrmLogin.css" rel="stylesheet" />
     <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
</head>
<body>
      <form id="form1" runat="server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-md-5">
                    <div class="card shadow">
                        <div class="card-body p-5">
                            <div class="text-center mb-4">
                                <i class="bi bi-person-circle text-primary" style="font-size: 4rem;"></i>
                                <h2 class="mt-3">Bem-vindo!</h2>
                                <p class="text-muted">Entre com sua conta</p>
                            </div>

                            <!-- Painel de Mensagem -->
                            <asp:Panel ID="pnlMensagem" runat="server" Visible="false" CssClass="alert" role="alert">
                                <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                            </asp:Panel>

                            <!-- Campo de Email -->
                            <div class="mb-3">
                                <label for="txtEmail" class="form-label">
                                    <i class="bi bi-envelope"></i> Email
                                </label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                    TextMode="Email" placeholder="seu@email.com"></asp:TextBox>
                            </div>

                            <!-- Campo de Senha -->
                            <div class="mb-3">
                                <label for="txtSenha" class="form-label">
                                    <i class="bi bi-lock"></i> Senha
                                </label>
                                <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control"
                                    TextMode="Password" placeholder="••••••••"></asp:TextBox>
                            </div>

                            <div class="mb-3 form-check">
                                <input type="checkbox" class="form-check-input" id="chkLembrar" />
                                <label class="form-check-label" for="chkLembrar">
                                    Lembrar-me
                                </label>
                            </div>

                            <!-- Botão Entrar -->
                            <div class="d-grid gap-2">
                                <asp:Button ID="btnLogin" runat="server" Text="Entrar"
                                    CssClass="btn btn-primary btn-lg" OnClick="btnLogin_Click" />
                            </div>

                            <hr class="my-4" />

                            <div class="text-center">
                                <p class="mb-0">Não tem uma conta?</p>
                                <a href="Cadastro.aspx" class="btn btn-outline-primary mt-2">
                                    Cadastre-se agora
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
