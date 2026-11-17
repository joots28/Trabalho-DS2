# ProjetoDS2 — Sistema de Gestão de Seguros (SistemaSeguro)

Sistema corporativo de gestão de seguradora desenvolvido para a disciplina de Desenvolvimento de Sistemas 2 (DS2). O sistema conta com uma aplicação Desktop rica em Windows Forms e uma **Web API RESTful de alta performance em ASP.NET Core (.NET 9)** integrada a banco de dados relacional **MySQL**.

---

## 1. Visão Geral e Arquitetura

O sistema é estruturado em camadas desacopladas (Clean Architecture / Service-Repository Pattern):

```
ProjetoDS2/
├── Banco_de_dados/          # Scripts DDL, modelagem e diagramas do MySQL
│   ├── ProjetoDS.sql        # Criação do banco db_seguradora e tabelas
│   └── Modelo_Conceitual.png
├── DAO/                     # Camada de persistência clássica ADO.NET / MySQL
│   ├── Conexao.cs           # Gerenciador resiliente de conexão (App.config / Env)
│   ├── ClienteDAO.cs        # CRUD completo de Clientes
│   ├── AutomovelDAO.cs      # CRUD completo de Automóveis
│   ├── SeguroDAO.cs         # CRUD completo de Seguros
│   ├── AcidenteDAO.cs       # CRUD completo de Acidentes/Sinistros
│   └── PagamentoSeguroDAO.cs# CRUD completo de Pagamentos/Indenizações
├── Model/                   # Entidades do Domínio
│   ├── Cliente.cs
│   ├── Automovel.cs
│   ├── Seguro.cs
│   ├── Acidente.cs
│   └── PagamentoSeguro.cs
├── Services/                # Camada de Regras de Negócio e Validações
│   ├── ClienteService.cs
│   ├── AutomovelService.cs
│   ├── SeguroService.cs
│   ├── AcidenteService.cs
│   └── PagamentoSeguroService.cs
├── Forms (Desktop UI)/      # Interface Gráfica Windows Forms (.NET 4.7.2)
│   ├── FormMenu.cs          # Painel e Menu de Navegação Principal
│   ├── Form1.cs             # Gestão de Clientes
│   ├── FormAutomovel.cs     # Gestão de Automóveis
│   ├── FormAcidente.cs      # Gestão de Acidentes
│   └── FormPagamento.cs     # Gestão de Pagamentos
├── SistemaSeguro.Api/       # Web API RESTful ASP.NET Core (.NET 9)
│   ├── Controllers/         # Endpoints REST (Clientes, Autos, Seguros, Acidentes, Pagamentos, Health)
│   ├── DTOs/                # DTOs de entrada e ApiResponse<T> padronizado
│   ├── Repositories/        # Repositórios assíncronos de dados
│   ├── Middlewares/         # Tratamento global de exceções
│   └── Program.cs           # Configuração de CORS, DI e Swagger OpenAPI
└── SistemaSeguro.Tests/     # Suíte de Testes Automatizados (xUnit)
    ├── ApiIntegrationTests.cs # Testes de ponta a ponta contra API e MySQL
    └── ServiceUnitTests.cs    # Testes unitários das regras de negócio
```

---

## 2. Tecnologias Utilizadas

- **C# / .NET:**
  - .NET 9.0 (Web API RESTful e Testes Automatizados)
  - .NET Framework 4.7.2 (Aplicação Windows Forms)
- **Banco de Dados:** MySQL 8.x / 9.x (`db_seguradora`)
- **Drivers de Acesso a Dados:** `MySqlConnector` e `MySql.Data`
- **Documentação de API:** OpenAPI / Swagger (`Swashbuckle.AspNetCore`)
- **Testes Automatizados:** `xUnit`, `Microsoft.AspNetCore.Mvc.Testing`

---

## 3. Configuração do Banco de Dados

1. Certifique-se de que o serviço do MySQL esteja em execução.
2. O script de inicialização está localizado em `Banco_de_dados/ProjetoDS.sql`.
3. Para criar o banco e as tabelas, execute via terminal ou MySQL Workbench:

```bash
mysql -u root -p < Banco_de_dados/ProjetoDS.sql
```

A base conterá as tabelas:
- `cliente`: Cadastro e dados de contato do cliente.
- `automovel`: Veículos associados a um proprietário (`ON DELETE CASCADE`).
- `seguro`: Apólices de seguro vinculadas ao cliente e automóvel.
- `acidente`: Sinistros e acidentes vinculados ao veículo envolvido.
- `pagamento`: Indenizações pagas ao cliente referentes ao sinistro.

---

## 4. Como Executar a Web API RESTful

1. Navegue até a pasta da API:
```bash
cd SistemaSeguro.Api
```

2. Restaure e inicie a API:
```bash
dotnet run
```

3. A API será iniciada nos endereços:
- Swagger UI (Documentação Interativa): `http://localhost:5000` ou `https://localhost:5001`
- Health Check: `GET http://localhost:5000/api/health`

---

## 5. Endpoints da API RESTful

Todas as respostas seguem o padrão JSON uniforme:
```json
{
  "success": true,
  "message": "Operação realizada com sucesso.",
  "data": { ... },
  "errors": null,
  "timestamp": "2026-11-17T19:00:00Z"
}
```

### Módulos e Rotas:

| Módulo | Método | Rota | Descrição |
|---|---|---|---|
| **Health** | `GET` | `/api/health` | Diagnóstico de integridade da API e MySQL |
| **Clientes** | `GET` | `/api/clientes?busca={termo}` | Listagem com filtro opcional |
| | `GET` | `/api/clientes/{id}` | Busca cliente por ID |
| | `POST` | `/api/clientes` | Cadastro de novo cliente |
| | `PUT` | `/api/clientes/{id}` | Atualização de cliente |
| | `DELETE` | `/api/clientes/{id}` | Exclusão de cliente |
| **Automóveis** | `GET` | `/api/automoveis?idCliente={id}` | Listagem geral ou por cliente |
| | `GET` | `/api/automoveis/{id}` | Busca automóvel por ID |
| | `POST` | `/api/automoveis` | Cadastro com validação de placa Mercosul/antiga |
| | `PUT` | `/api/automoveis/{id}` | Atualização de veículo |
| | `DELETE` | `/api/automoveis/{id}` | Exclusão de veículo |
| **Seguros** | `GET` | `/api/seguros?idCliente={id}` | Listagem de apólices ativas |
| | `GET` | `/api/seguros/{id}` | Busca apólice por ID |
| | `POST` | `/api/seguros` | Cadastro de nova apólice |
| | `PUT` | `/api/seguros/{id}` | Atualização de apólice |
| | `DELETE` | `/api/seguros/{id}` | Exclusão de apólice |
| **Acidentes** | `GET` | `/api/acidentes?idAutomovel={id}` | Listagem de sinistros registrados |
| | `GET` | `/api/acidentes/{id}` | Busca sinistro por ID |
| | `POST` | `/api/acidentes` | Registro de novo acidente |
| | `PUT` | `/api/acidentes/{id}` | Atualização de sinistro |
| | `DELETE` | `/api/acidentes/{id}` | Exclusão de sinistro |
| **Pagamentos** | `GET` | `/api/pagamentos?idCliente={id}` | Listagem de indenizações |
| | `GET` | `/api/pagamentos/{id}` | Busca pagamento por ID |
| | `POST` | `/api/pagamentos` | Registro de pagamento/indenização |
| | `PUT` | `/api/pagamentos/{id}` | Atualização de pagamento |
| | `DELETE` | `/api/pagamentos/{id}` | Exclusão de pagamento |

---

## 6. Como Executar os Testes Automatizados

Para executar toda a suíte de testes (Integração e Unitários):

```bash
dotnet test SistemaSeguro.Tests/SistemaSeguro.Tests.csproj
```

Resultado verificado:
- **Total:** 8 testes
- **Aprovados:** 8 (100%)
- **Falhas:** 0

---

## 7. Como Executar a Aplicação Desktop (Windows Forms)

Para compilar e executar o aplicativo Windows Forms:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" SistemaSeguro.csproj /t:Build /p:Configuration=Debug
& ".\bin\Debug\SistemaSeguro.exe"
```
Ou simplesmente abra a solução `SistemaSeguro.sln` no Visual Studio e pressione `F5`.
