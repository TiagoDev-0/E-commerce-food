<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="E_commerce_food.adm.Dashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Dashboard - Administrador</title>
    <link href="../css-adm/admin.css" rel="stylesheet" />
    <link href="../css/bootstrap.css" rel="stylesheet" />
     <!-- Ícones e Gráficos -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1>📊 Painel Administrativo</h1>
            <div>
                <a href="FrmProdutos.aspx" class="btn btn-nav"><i class="fa-solid fa-box"></i> Produtos</a>
                <a href="FrmPedidos.aspx" class="btn btn-nav"><i class="fa-solid fa-receipt"></i> Pedidos</a>
                <a href="FrmUsuarios.aspx" class="btn btn-nav"><i class="fa-solid fa-users"></i> Usuários</a>
                <a href="../FrmLogin.aspx" class="btn btn-danger"><i class="fa-solid fa-right-from-bracket"></i> Sair</a>
            </div>
        </header>
        <main>
               <div class="container mt-4">
       <h2 class="text-center mb-4">Resumo do Sistema</h2>

       <div class="dashboard-cards">
           <div class="card text-center p-4">
               <i class="fa-solid fa-box"></i>
               <h4 class="mt-3">Total de Produtos</h4>
               <asp:Label ID="lblProdutos" runat="server" CssClass="fs-4 fw-bold text-primary"></asp:Label>
           </div>

           <div class="card text-center p-4">
               <i class="fa-solid fa-cart-shopping"></i>
               <h4 class="mt-3">Total de Pedidos</h4>
               <asp:Label ID="lblPedidos" runat="server" CssClass="fs-4 fw-bold text-success"></asp:Label>
           </div>

           <div class="card text-center p-4">
               <i class="fa-solid fa-user-check"></i>
               <h4 class="mt-3">Usuários Ativos</h4>
               <asp:Label ID="lblUsuariosAtivos" runat="server" CssClass="fs-4 fw-bold text-info"></asp:Label>
           </div>

           <div class="card text-center p-4">
               <i class="fa-solid fa-user-lock"></i>
               <h4 class="mt-3">Usuários Bloqueados</h4>
               <asp:Label ID="lblUsuariosBloqueados" runat="server" CssClass="fs-4 fw-bold text-danger"></asp:Label>
           </div>
       </div>

       <div class="chart-container mt-5">
           <h4 class="text-center mb-3">📈 Pedidos por Mês</h4>
           <canvas id="chartPedidos"></canvas>
       </div>
   </div>
        </main>
     

        <footer>
            Sistema de Administração © <%= DateTime.Now.Year %> - E-commerce Food
        </footer>
    </form>

    <!-- Scripts -->
    <script src="../js/bootstrap.bundle.min.js"></script>
    <script>
        const ctx = document.getElementById('chartPedidos').getContext('2d');
        const chartPedidos = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                datasets: [{
                    label: 'Pedidos',
                    data: [5, 8, 12, 9, 15, 18, 25, 22, 19, 30, 28, 35],
                    borderColor: '#0d6efd',
                    backgroundColor: 'rgba(13,110,253,0.2)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }]
            },
            options: {
                scales: {
                    y: { beginAtZero: true }
                },
                plugins: {
                    legend: { display: false }
                }
            }
        });
    </script>
</body>
</html>
