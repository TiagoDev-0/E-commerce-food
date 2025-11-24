<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="E_commerce_food.Admin.Clientes" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Gerenciar Clientes</title>
    <link rel="stylesheet" href="admin.css" />
</head>
<body>
    <header>
        <h1>Painel Administrativo - Clientes</h1>
    </header>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Lista de Clientes</h2>

            <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                OnRowCommand="gvClientes_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:BoundField DataField="Nome" HeaderText="Nome" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Telefone" HeaderText="Telefone" />
                    <asp:CheckBoxField DataField="Ativo" HeaderText="Ativo" />
                    <asp:TemplateField HeaderText="Ações">
                        <ItemTemplate>
                            <asp:Button ID="btnBloquear" runat="server" CssClass="btn" 
                                Text='<%# (bool)Eval("Ativo") ? "Bloquear" : "Desbloquear" %>' 
                                CommandName="Bloquear" CommandArgument='<%# Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
