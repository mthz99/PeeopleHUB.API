# ?? PeopleHub API

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge&logo=microsoft)](https://docs.microsoft.com/en-us/ef/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger)](https://swagger.io/)
[![xUnit](https://img.shields.io/badge/xUnit-Testing-512BD4?style=for-the-badge&logo=microsoft)](https://xunit.net/)

*API REST robusta para gerenciamento de pessoas seguindo Clean Architecture e padr�o CQRS*

</div>

## ?? Sobre o Projeto

A **PeopleHub API** � uma aplica��o moderna desenvolvida em **.NET 8** que implementa um sistema completo de gerenciamento de pessoas com autentica��o JWT, versionamento de API e arquitetura limpa. O projeto demonstra boas pr�ticas de desenvolvimento, incluindo testes unit�rios abrangentes e documenta��o OpenAPI.

### ? Principais Caracter�sticas

- ??? **Clean Architecture** com separa��o clara de responsabilidades
- ?? **CQRS Pattern** usando MediatR para separa��o de comandos e consultas
- ?? **Autentica��o JWT** robusta com hash BCrypt para senhas
- ?? **API Versioning** (v1.0 e v2.0) mantendo compatibilidade
- ?? **CORS** configurado para aceitar qualquer origem
- ?? **Swagger/OpenAPI** para documenta��o interativa
- ?? **Testes Unit�rios** completos com alta cobertura
- ? **Valida��o** robusta de CPF com d�gitos verificadores

## ??? Arquitetura

```
?? PeopleHub.API/
??? ?? PeopleHub.API/              # Apresenta��o (Controllers, Program.cs)
??? ?? PeopleHub.Application/      # Aplica��o (CQRS, DTOs, Handlers)
??? ??? PeopleHub.Domain/           # Dom�nio (Entidades, Interfaces)
??? ?? PeopleHub.Infrastructure/   # Infraestrutura (Reposit�rios, DbContext)
??? ?? PeopleHub.Test/             # Testes Unit�rios (Controllers, Helpers)
```

### Camadas da Aplica��o

- **API**: Controllers, middlewares, configura��o
- **Application**: Commands, Queries, Handlers, DTOs, valida��es
- **Domain**: Entidades de neg�cio, interfaces, regras de dom�nio  
- **Infrastructure**: Implementa��es de reposit�rios, servi�os, contexto de dados
- **Test**: Testes unit�rios com mocking e assertions fluentes

## ?? Tecnologias

### Core
- ![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4) **Framework principal**
- ![Entity Framework](https://img.shields.io/badge/EF%20Core-9.0-512BD4) **ORM para acesso a dados**
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927) **Banco de dados**

### Arquitetura & Padr�es
- ![MediatR](https://img.shields.io/badge/MediatR-12.2-FF6B6B) **CQRS Pattern**
- ![JWT](https://img.shields.io/badge/JWT-Authentication-000000) **Autentica��o stateless**
- ![BCrypt](https://img.shields.io/badge/BCrypt-Hashing-4ECDC4) **Hash seguro de senhas**

### Documenta��o & API
- ![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D) **Documenta��o interativa**
- ![Versioning](https://img.shields.io/badge/API-Versioning-45B7D1) **M�ltiplas vers�es**

### Testes & Qualidade
- ![xUnit](https://img.shields.io/badge/xUnit-Framework-512BD4) **Framework de testes**
- ![Moq](https://img.shields.io/badge/Moq-Mocking-96CEB4) **Simula��o de depend�ncias**
- ![FluentAssertions](https://img.shields.io/badge/FluentAssertions-Readable-FFEAA7) **Assertions expressivas**

## ?? Funcionalidades

### ?? Sistema de Autentica��o
| Funcionalidade | Endpoint | Descri��o |
|---|---|---|
| ?? **Registro** | `POST /api/Auth/register` | Criar conta com dados pessoais + credenciais |
| ?? **Login** | `POST /api/Auth/login` | Autentica��o via CPF e senha |
| ?? **JWT Token** | - | Tokens com expira��o configur�vel |

### ?? Gerenciamento de Pessoas

#### Vers�o 1.0 (B�sica)
| A��o | Endpoint | Descri��o |
|---|---|---|
| ?? **Listar** | `GET /api/v1.0/PeopleHub` | Buscar todas as pessoas |
| ?? **Buscar** | `GET /api/v1.0/PeopleHub/{id}` | Buscar pessoa por ID |
| ? **Criar** | `POST /api/v1.0/PeopleHub` | Adicionar nova pessoa |
| ?? **Editar** | `PUT /api/v1.0/PeopleHub/{id}` | Atualizar dados |
| ??? **Excluir** | `DELETE /api/v1.0/PeopleHub/{id}` | Remover pessoa |

#### Vers�o 2.0 (Com Endere�o)
| A��o | Endpoint | Descri��o |
|---|---|---|
| ?? **Listar** | `GET /api/v2.0/PeopleHub` | Buscar pessoas + endere�os |
| ?? **Buscar** | `GET /api/v2.0/PeopleHub/{id}` | Buscar pessoa + endere�o |
| ? **Criar** | `POST /api/v2.0/PeopleHub` | Criar com endere�o obrigat�rio |
| ?? **Editar** | `PUT /api/v2.0/PeopleHub/{id}` | Atualizar + endere�o |
| ??? **Excluir** | `DELETE /api/v2.0/PeopleHub/{id}` | Remover pessoa |

### ?? Campos da Pessoa

| Campo | Tipo | V1 | V2 | Valida��o |
|---|---|:---:|:---:|---|
| **Nome** | `string` | ? | ? | Obrigat�rio, max 200 chars |
| **Sexo** | `string` | ? | ? | Opcional, M ou F |
| **Email** | `string` | ? | ? | Opcional, formato v�lido |
| **Data Nascimento** | `DateTime` | ? | ? | Obrigat�rio, anterior a hoje |
| **Naturalidade** | `string` | ? | ? | Opcional, max 100 chars |
| **Nacionalidade** | `string` | ? | ? | Opcional, max 100 chars |
| **CPF** | `string` | ? | ? | Obrigat�rio, valida��o completa |
| **Endere�o** | `string` | ? | ? | Obrigat�rio apenas na V2 |

## ?? Testes Unit�rios

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

### ?? Cen�rios Testados

| Controller | Cen�rios | Status |
|---|---|:---:|
| **AuthController** | Login v�lido/inv�lido, registro, exce��es | ? 100% |
| **PeopleHub V1** | CRUD completo, valida��es, exce��es | ? 100% |
| **PeopleHub V2** | CRUD + endere�o, valida��es espec�ficas | ? 100% |

### ????? Executar Testes

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Espec�ficos
dotnet test --filter "AuthControllerTests"
dotnet test --filter "PeopleHubController"
```

## ? Quick Start

### ?? Pr�-requisitos

- ![.NET 8 SDK](https://img.shields.io/badge/.NET-8.0%20SDK-512BD4)
- ![SQL Server](https://img.shields.io/badge/SQL-Server-CC2927) (LocalDB ou inst�ncia completa)
- ![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-5C2D91) / ![VS Code](https://img.shields.io/badge/VS%20Code-Editor-007ACC) (opcional)

### ?? Instala��o

1. **Clone o reposit�rio**
   ```bash
   git clone https://github.com/seu-usuario/peoplehub-api.git
   cd peoplehub-api
   ```

2. **Restaure as depend�ncias**
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

5. **Execute a aplica��o**
   ```bash
   dotnet run --project PeopleHub.API
   ```

6. **Acesse o Swagger**
   ```
   https://localhost:7xxx/swagger
   ```

## ?? Configura��o

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

### ?? Configura��o JWT

| Par�metro | Descri��o | Exemplo |
|---|---|---|
| **SecretKey** | Chave para assinar tokens (min. 32 chars) | `"MinhaChaveSuperSecreta123456789012"` |
| **Issuer** | Emissor do token | `"PeopleHubAPI"` |
| **Audience** | Audi�ncia do token | `"PeopleHubUsers"` |
| **Expiration** | Tempo de vida | `1 hora` (configur�vel) |

## ?? Exemplos de Uso

### ?? Autentica��o

<details>
<summary><b>1. Registro de Usu�rio</b></summary>

```bash
curl -X POST "https://localhost:7xxx/api/Auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Jo�o Silva",
    "sexo": "M",
    "email": "joao@email.com",
    "dataNascimento": "1990-05-15",
    "naturalidade": "S�o Paulo",
    "nacionalidade": "Brasileira",
    "cpf": "12345678901",
    "password": "minhasenha123"
  }'
```

**Resposta:**
```json
{
  "message": "Usu�rio registrado com sucesso",
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
<summary><b>4. Criar Pessoa (V2 - Com Endere�o)</b></summary>

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

## ?? Valida��es Implementadas

### ? CPF
- ? Formato correto (11 d�gitos num�ricos)
- ? Valida��o completa dos d�gitos verificadores
- ? Unicidade no banco de dados

### ? Outros Campos
- ?? **Data de Nascimento**: Deve ser anterior � data atual
- ?? **Email**: Formato v�lido quando preenchido
- ?? **Nome**: Obrigat�rio, m�ximo 200 caracteres
- ?? **Senha**: M�nimo 6 caracteres (registro)

## ?? Padr�es e Boas Pr�ticas

### ??? Arquiteturais
- ? **Clean Architecture** - Separa��o clara de responsabilidades
- ? **CQRS** - Commands separados de Queries
- ? **Repository Pattern** - Abstra��o do acesso a dados
- ? **Dependency Injection** - Invers�o de depend�ncias

### ?? C�digo
- ? **SOLID Principles** - C�digo extens�vel e manuten�vel
- ? **Domain-Driven Design** - Foco no dom�nio da aplica��o
- ? **API Versioning** - Evolu��o sem quebrar compatibilidade
- ? **Error Handling** - Tratamento padronizado de exce��es

### ?? Testes
- ? **AAA Pattern** - Arrange, Act, Assert
- ? **Mocking** - Isolamento de depend�ncias
- ? **Test Data Builders** - Factory pattern para dados
- ? **Fluent Assertions** - Assertions leg�veis

## ?? Contribuindo

1. **Fork** o projeto
2. **Clone** sua fork: `git clone https://github.com/seu-usuario/peoplehub-api.git`
3. **Branch** para sua feature: `git checkout -b feature/nova-funcionalidade`
4. **Commit** suas mudan�as: `git commit -m 'Add: nova funcionalidade'`
5. **Push** para a branch: `git push origin feature/nova-funcionalidade`
6. **Pull Request** para a branch main

### ?? Diretrizes

- ? Siga os padr�es de c�digo existentes
- ? Adicione testes para novas funcionalidades
- ? Documente mudan�as na API
- ? Execute todos os testes antes do PR

## ?? Licen�a

Este projeto est� sob a licen�a MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ????? Autor

**Seu Nome**
- GitHub: [@seu-usuario](https://github.com/seu-usuario)
- LinkedIn: [Seu Perfil](https://linkedin.com/in/seu-perfil)
- Email: seu.email@exemplo.com

---

<div align="center">

? **Se este projeto te ajudou, deixe uma estrela!** ?

</div>