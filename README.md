# ?? PeopleHub API

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge&logo=microsoft)](https://docs.microsoft.com/en-us/ef/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger)](https://swagger.io/)
[![xUnit](https://img.shields.io/badge/xUnit-Testing-512BD4?style=for-the-badge&logo=microsoft)](https://xunit.net/)

*API REST robusta para gerenciamento de pessoas seguindo Clean Architecture e padrão CQRS*

</div>

## ?? Sobre o Projeto

A **PeopleHub API** é uma aplicação moderna desenvolvida em **.NET 8** que implementa um sistema completo de gerenciamento de pessoas com autenticação JWT, versionamento de API e arquitetura limpa. O projeto demonstra boas práticas de desenvolvimento, incluindo testes unitários abrangentes e documentação OpenAPI.

### ? Principais Características

- ??? **Clean Architecture** com separação clara de responsabilidades
- ?? **CQRS Pattern** usando MediatR para separação de comandos e consultas
- ?? **Autenticação JWT** robusta com hash BCrypt para senhas
- ?? **API Versioning** (v1.0 e v2.0) mantendo compatibilidade
- ?? **CORS** configurado para aceitar qualquer origem
- ?? **Swagger/OpenAPI** para documentação interativa
- ?? **Testes Unitários** completos com alta cobertura
- ? **Validação** robusta de CPF com dígitos verificadores

## ??? Arquitetura

```
?? PeopleHub.API/
??? ?? PeopleHub.API/              # Apresentação (Controllers, Program.cs)
??? ?? PeopleHub.Application/      # Aplicação (CQRS, DTOs, Handlers)
??? ??? PeopleHub.Domain/           # Domínio (Entidades, Interfaces)
??? ?? PeopleHub.Infrastructure/   # Infraestrutura (Repositórios, DbContext)
??? ?? PeopleHub.Test/             # Testes Unitários (Controllers, Helpers)
```

### Camadas da Aplicação

- **API**: Controllers, middlewares, configuração
- **Application**: Commands, Queries, Handlers, DTOs, validações
- **Domain**: Entidades de negócio, interfaces, regras de domínio  
- **Infrastructure**: Implementações de repositórios, serviços, contexto de dados
- **Test**: Testes unitários com mocking e assertions fluentes

## ?? Tecnologias

### Core
- ![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4) **Framework principal**
- ![Entity Framework](https://img.shields.io/badge/EF%20Core-9.0-512BD4) **ORM para acesso a dados**
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927) **Banco de dados**

### Arquitetura & Padrões
- ![MediatR](https://img.shields.io/badge/MediatR-12.2-FF6B6B) **CQRS Pattern**
- ![JWT](https://img.shields.io/badge/JWT-Authentication-000000) **Autenticação stateless**
- ![BCrypt](https://img.shields.io/badge/BCrypt-Hashing-4ECDC4) **Hash seguro de senhas**

### Documentação & API
- ![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D) **Documentação interativa**
- ![Versioning](https://img.shields.io/badge/API-Versioning-45B7D1) **Múltiplas versões**

### Testes & Qualidade
- ![xUnit](https://img.shields.io/badge/xUnit-Framework-512BD4) **Framework de testes**
- ![Moq](https://img.shields.io/badge/Moq-Mocking-96CEB4) **Simulação de dependências**
- ![FluentAssertions](https://img.shields.io/badge/FluentAssertions-Readable-FFEAA7) **Assertions expressivas**

## ?? Funcionalidades

### ?? Sistema de Autenticação
| Funcionalidade | Endpoint | Descrição |
|---|---|---|
| ?? **Registro** | `POST /api/Auth/register` | Criar conta com dados pessoais + credenciais |
| ?? **Login** | `POST /api/Auth/login` | Autenticação via CPF e senha |
| ?? **JWT Token** | - | Tokens com expiração configurável |

### ?? Gerenciamento de Pessoas

#### Versão 1.0 (Básica)
| Ação | Endpoint | Descrição |
|---|---|---|
| ?? **Listar** | `GET /api/v1.0/PeopleHub` | Buscar todas as pessoas |
| ?? **Buscar** | `GET /api/v1.0/PeopleHub/{id}` | Buscar pessoa por ID |
| ? **Criar** | `POST /api/v1.0/PeopleHub` | Adicionar nova pessoa |
| ?? **Editar** | `PUT /api/v1.0/PeopleHub/{id}` | Atualizar dados |
| ??? **Excluir** | `DELETE /api/v1.0/PeopleHub/{id}` | Remover pessoa |

#### Versão 2.0 (Com Endereço)
| Ação | Endpoint | Descrição |
|---|---|---|
| ?? **Listar** | `GET /api/v2.0/PeopleHub` | Buscar pessoas + endereços |
| ?? **Buscar** | `GET /api/v2.0/PeopleHub/{id}` | Buscar pessoa + endereço |
| ? **Criar** | `POST /api/v2.0/PeopleHub` | Criar com endereço obrigatório |
| ?? **Editar** | `PUT /api/v2.0/PeopleHub/{id}` | Atualizar + endereço |
| ??? **Excluir** | `DELETE /api/v2.0/PeopleHub/{id}` | Remover pessoa |

### ?? Campos da Pessoa

| Campo | Tipo | V1 | V2 | Validação |
|---|---|:---:|:---:|---|
| **Nome** | `string` | ? | ? | Obrigatório, max 200 chars |
| **Sexo** | `string` | ? | ? | Opcional, M ou F |
| **Email** | `string` | ? | ? | Opcional, formato válido |
| **Data Nascimento** | `DateTime` | ? | ? | Obrigatório, anterior a hoje |
| **Naturalidade** | `string` | ? | ? | Opcional, max 100 chars |
| **Nacionalidade** | `string` | ? | ? | Opcional, max 100 chars |
| **CPF** | `string` | ? | ? | Obrigatório, validação completa |
| **Endereço** | `string` | ? | ? | Obrigatório apenas na V2 |

## ?? Testes Unitários

### ?? Cobertura Abrangente

```
PeopleHub.Test/
??? ?? Controllers/
?   ??? ? AuthControllerTests.cs          # 100% cobertura auth
?   ??? ?? Version1/
?   ?   ??? ? PeopleHubControllerV1Tests.cs # CRUD completo V1
?   ??? ?? Version2/
?       ??? ? PeopleHubControllerV2Tests.cs # CRUD completo V2
??? ?? Helpers/
    ??? ?? TestDataFactory.cs              # Factory de dados
```

### ?? Cenários Testados

| Controller | Cenários | Status |
|---|---|:---:|
| **AuthController** | Login válido/inválido, registro, exceções | ? 100% |
| **PeopleHub V1** | CRUD completo, validações, exceções | ? 100% |
| **PeopleHub V2** | CRUD + endereço, validações específicas | ? 100% |

### ????? Executar Testes

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Específicos
dotnet test --filter "AuthControllerTests"
dotnet test --filter "PeopleHubController"
```

## ? Quick Start

### ?? Pré-requisitos

- ![.NET 8 SDK](https://img.shields.io/badge/.NET-8.0%20SDK-512BD4)
- ![SQL Server](https://img.shields.io/badge/SQL-Server-CC2927) (LocalDB ou instância completa)
- ![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-5C2D91) / ![VS Code](https://img.shields.io/badge/VS%20Code-Editor-007ACC) (opcional)

### ?? Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/peoplehub-api.git
   cd peoplehub-api
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Configure a connection string**
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PeopleHubDB;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

4. **Execute as migrations**
   ```bash
   dotnet ef database update --project PeopleHub.Infrastructure
   ```

5. **Execute a aplicação**
   ```bash
   dotnet run --project PeopleHub.API
   ```

6. **Acesse o Swagger**
   ```
   https://localhost:7xxx/swagger
   ```

## ?? Configuração

### ?? appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SUA_CONNECTION_STRING_AQUI"
  },
  "Jwt": {
    "SecretKey": "SUA_CHAVE_SECRETA_SUPER_SEGURA_AQUI_COM_PELO_MENOS_32_CARACTERES",
    "Issuer": "PeopleHubAPI",
    "Audience": "PeopleHubUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### ?? Configuração JWT

| Parâmetro | Descrição | Exemplo |
|---|---|---|
| **SecretKey** | Chave para assinar tokens (min. 32 chars) | `"MinhaChaveSuperSecreta123456789012"` |
| **Issuer** | Emissor do token | `"PeopleHubAPI"` |
| **Audience** | Audiência do token | `"PeopleHubUsers"` |
| **Expiration** | Tempo de vida | `1 hora` (configurável) |

## ?? Exemplos de Uso

### ?? Autenticação

<details>
<summary><b>1. Registro de Usuário</b></summary>

```bash
curl -X POST "https://localhost:7xxx/api/Auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "sexo": "M",
    "email": "joao@email.com",
    "dataNascimento": "1990-05-15",
    "naturalidade": "São Paulo",
    "nacionalidade": "Brasileira",
    "cpf": "12345678901",
    "password": "minhasenha123"
  }'
```

**Resposta:**
```json
{
  "message": "Usuário registrado com sucesso",
  "username": "12345678901",
  "email": "joao@email.com"
}
```
</details>

<details>
<summary><b>2. Login</b></summary>

```bash
curl -X POST "https://localhost:7xxx/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "12345678901",
    "password": "minhasenha123"
  }'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "12345678901",
  "expiresAt": "2024-01-01T15:30:00Z"
}
```
</details>

### ?? Gerenciamento de Pessoas

<details>
<summary><b>3. Criar Pessoa (V1)</b></summary>

```bash
curl -X POST "https://localhost:7xxx/api/v1.0/PeopleHub" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Santos",
    "sexo": "F",
    "email": "maria@email.com",
    "dataNascimento": "1985-03-22",
    "naturalidade": "Rio de Janeiro",
    "nacionalidade": "Brasileira",
    "cpf": "98765432109"
  }'
```
</details>

<details>
<summary><b>4. Criar Pessoa (V2 - Com Endereço)</b></summary>

```bash
curl -X POST "https://localhost:7xxx/api/v2.0/PeopleHub" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Pedro Oliveira",
    "sexo": "M",
    "email": "pedro@email.com",
    "dataNascimento": "1992-11-08",
    "naturalidade": "Belo Horizonte",
    "nacionalidade": "Brasileira",
    "cpf": "45678912345",
    "endereco": "Rua da Paz, 789, Belo Horizonte, MG"
  }'
```
</details>

## ?? Validações Implementadas

### ? CPF
- ? Formato correto (11 dígitos numéricos)
- ? Validação completa dos dígitos verificadores
- ? Unicidade no banco de dados

### ? Outros Campos
- ?? **Data de Nascimento**: Deve ser anterior à data atual
- ?? **Email**: Formato válido quando preenchido
- ?? **Nome**: Obrigatório, máximo 200 caracteres
- ?? **Senha**: Mínimo 6 caracteres (registro)

## ?? Padrões e Boas Práticas

### ??? Arquiteturais
- ? **Clean Architecture** - Separação clara de responsabilidades
- ? **CQRS** - Commands separados de Queries
- ? **Repository Pattern** - Abstração do acesso a dados
- ? **Dependency Injection** - Inversão de dependências

### ?? Código
- ? **SOLID Principles** - Código extensível e manutenível
- ? **Domain-Driven Design** - Foco no domínio da aplicação
- ? **API Versioning** - Evolução sem quebrar compatibilidade
- ? **Error Handling** - Tratamento padronizado de exceções

### ?? Testes
- ? **AAA Pattern** - Arrange, Act, Assert
- ? **Mocking** - Isolamento de dependências
- ? **Test Data Builders** - Factory pattern para dados
- ? **Fluent Assertions** - Assertions legíveis

## ?? Contribuindo

1. **Fork** o projeto
2. **Clone** sua fork: `git clone https://github.com/seu-usuario/peoplehub-api.git`
3. **Branch** para sua feature: `git checkout -b feature/nova-funcionalidade`
4. **Commit** suas mudanças: `git commit -m 'Add: nova funcionalidade'`
5. **Push** para a branch: `git push origin feature/nova-funcionalidade`
6. **Pull Request** para a branch main

### ?? Diretrizes

- ? Siga os padrões de código existentes
- ? Adicione testes para novas funcionalidades
- ? Documente mudanças na API
- ? Execute todos os testes antes do PR

## ?? Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ????? Autor

**Seu Nome**
- GitHub: [@seu-usuario](https://github.com/seu-usuario)
- LinkedIn: [Seu Perfil](https://linkedin.com/in/seu-perfil)
- Email: seu.email@exemplo.com

---

<div align="center">

? **Se este projeto te ajudou, deixe uma estrela!** ?

</div>