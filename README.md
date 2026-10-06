# 🍔 E-commerce Food

Sistema web de e-commerce desenvolvido em **C# com ASP.NET Web Forms**, voltado para a venda de alimentos e bebidas.

O projeto permite que clientes naveguem pelo catálogo, adicionem produtos ao carrinho, realizem pedidos e acompanhem seu histórico de compras. Também possui uma área administrativa para gerenciamento dos produtos e usuários.

> **Status:** Projeto acadêmico/concluído

## 📋 Sobre o projeto

O **E-commerce Food** foi desenvolvido como um sistema de comércio eletrônico para simular o funcionamento de uma plataforma de pedidos de alimentos.

A aplicação possui duas áreas principais:

- **Área do cliente:** navegação pelo catálogo, busca de produtos, carrinho, perfil e pedidos.
- **Área administrativa:** gerenciamento dos produtos e usuários do sistema.

O projeto também utiliza uma estrutura organizada em camadas, separando regras de negócio, acesso aos dados e modelos da aplicação.

## ✨ Funcionalidades

### 👤 Cliente

- [x] Cadastro de usuário
- [x] Login
- [x] Perfil do usuário
- [x] Catálogo de produtos
- [x] Produtos separados por categorias
- [x] Busca de produtos
- [x] Visualização de produtos
- [x] Carrinho de compras
- [x] Finalização de pedidos
- [x] Histórico de pedidos

### 🔐 Administração

- [x] Área administrativa
- [x] Gerenciamento de produtos
- [x] Gerenciamento de usuários
- [x] Cadastro de produtos
- [x] Upload de imagens
- [x] Organização dos produtos por categorias

## 🛠️ Tecnologias utilizadas

### Backend

- **C#**
- **ASP.NET Web Forms**
- **ADO.NET**
- **.NET Framework**

### Banco de dados

- **SQL Server**
- SQL
- Entity Framework / Migrations

### Frontend

- **HTML**
- **CSS**
- **Bootstrap**
- **JavaScript**
- **ASP.NET Web Forms / ASPX**

### Ambiente

- **Visual Studio**
- **IIS Express**
- **Git**
- **GitHub**

## 🏗️ Arquitetura

O projeto utiliza uma organização em camadas para separar as principais responsabilidades da aplicação:

```text
┌──────────────────────────────┐
│             UI               │
│      ASP.NET Web Forms       │
│        Pages / ASPX          │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│             BLL              │
│        Business Logic        │
│       Regras de negócio      │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│             DAL              │
│       Data Access Layer      │
│       Acesso aos dados       │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         SQL Server           │
│         Banco de dados       │
└──────────────────────────────┘
```

Essa separação facilita a organização do código e permite que a interface, as regras de negócio e o acesso aos dados tenham responsabilidades distintas.

## 📂 Estrutura do projeto

```text
E-commerce-food/
│
├── App_Data/
│   └── dbo.Table.sql
│
├── BLL/
│   ├── ClienteBLL.cs
│   ├── PedidosBLL.cs
│   ├── ProdutosBLL.cs
│   └── UsuarioBLL.cs
│
├── Cliente/
│   ├── Bebidas.aspx
│   ├── Busca.aspx
│   ├── Carrinho.aspx
│   ├── Combos.aspx
│   ├── Comidas.aspx
│   ├── Default.aspx
│   ├── MeusPedidos.aspx
│   ├── Perfil.aspx
│   └── Sobremesas.aspx
│
├── DAL/
│   ├── ClienteRepository.cs
│   ├── PedidosRepository.cs
│   ├── ProdutosRepository.cs
│   └── UsuarioRepository.cs
│
├── Images/
│   └── produtos/
│
├── Migrations/
│
├── Models/
│   ├── Cliente.cs
│   ├── ItemPedido.cs
│   ├── Pedido.cs
│   ├── Produto.cs
│   ├── Usuarios.cs
│   └── ...
│
├── Uploads/
│
├── FrmCadastro.aspx
├── FrmLogin.aspx
├── Web.config
└── E-commerce-food.sln
```

## 🗄️ Banco de dados

O sistema utiliza **SQL Server** para armazenamento dos dados da aplicação.

Entre as principais entidades utilizadas estão:

- Usuários;
- Clientes;
- Produtos;
- Pedidos;
- Itens dos pedidos.

O projeto possui arquivos relacionados à criação e migração do banco de dados, incluindo scripts SQL e migrations.

## 🖼️ Imagens e uploads

O sistema possui armazenamento de imagens relacionadas aos produtos.

As imagens utilizadas pelo projeto são organizadas em diretórios específicos:

```text
Images/
└── produtos/

Uploads/
```

O sistema utiliza recursos do ASP.NET para realizar o upload e armazenamento dos arquivos.

## ▶️ Como executar

### Pré-requisitos

Para executar o projeto, é necessário possuir:

- Windows;
- Visual Studio;
- .NET Framework compatível com o projeto;
- SQL Server;
- SQL Server Management Studio (recomendado).

### 1. Clonar o repositório

```bash
git clone https://github.com/TiagoSantos9/E-commerce-food.git
cd E-commerce-food
```

### 2. Abrir o projeto

Abra a solução:

```text
E-commerce-food.sln
```

no **Visual Studio**.

### 3. Configurar o banco de dados

Configure o SQL Server e restaure/crie o banco utilizando os scripts e migrations disponíveis no projeto.

Verifique também a configuração da conexão no:

```text
Web.config
```

### 4. Executar

No Visual Studio:

1. Selecione o projeto como projeto de inicialização;
2. Execute utilizando **IIS Express**;
3. Aguarde o navegador abrir a aplicação.

## 🧠 Decisões técnicas

Durante o desenvolvimento foram adotadas algumas decisões para manter o projeto organizado:

- Separação entre **UI, BLL e DAL**;
- Utilização de **Models** para representação das entidades;
- Repositories para acesso aos dados;
- CodeBehind nas páginas ASPX;
- Utilização de sessões para controle de usuário;
- Banco de dados relacional;
- Separação entre área do cliente e área administrativa;
- Organização dos produtos por categorias;
- Upload de imagens para produtos.

## 📚 Conceitos praticados

O projeto permitiu praticar conceitos importantes de desenvolvimento de sistemas, como:

- Programação Orientada a Objetos;
- Desenvolvimento Web;
- C#;
- ASP.NET Web Forms;
- SQL;
- Banco de dados relacionais;
- CRUD;
- Arquitetura em camadas;
- Padrão Repository;
- Regras de negócio;
- Autenticação e sessões;
- Manipulação de arquivos;
- Desenvolvimento de aplicações com banco de dados.

## 🎯 Objetivo acadêmico

O projeto foi desenvolvido com o objetivo de aplicar conhecimentos de **desenvolvimento web, programação orientada a objetos, banco de dados e arquitetura de software** em uma aplicação prática.

## 🔗 Repositório

[GitHub — E-commerce Food](https://github.com/TiagoSantos9/E-commerce-food)
