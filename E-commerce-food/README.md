Feito por : Alex, e Tiago


# E-commerce Food

## 🍔 Funcionalidades Principais
- Cadastro e login de usuários  
- Catálogo de produtos com imagens  
- Carrinho de compras  
- Finalização de pedidos  
- Histórico de compras do cliente  
- Painel administrativo para gerenciar produtos e equipes  
- Upload de imagens para produtos  

## 🛠 Tecnologias Utilizadas
- ASP.NET WebForms (C#)  
- SQL Server  
- ADO.NET  
- Bootstrap  
- JavaScript  
- IIS / Visual Studio  

## ▶ Como Executar o Projeto
1. Abra o projeto no **Visual Studio**.  
2. Ajuste a string de conexão no arquivo `Web.config`.  
3. Restaure o banco de dados SQL Server.  
4. Execute o projeto via IIS Express.  
5. Acesse:  
   - `/Cliente` para a área do cliente  
   - `/adm` para administração  

## 🧠 Decisões Técnicas
- Separação clara entre camadas (**BLL**, **Models**, **UI**)  
- Páginas ASPX com CodeBehind para organização  
- Upload de imagens usando `FileUpload` e salvamento físico  
- Sessões para autenticação  
- Padronização de nomes e organização de pastas  

🔗 Repositório no GitHub

👉 https://github.com/TiagoSantos9/E-commerce-food
