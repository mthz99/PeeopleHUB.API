# PeopleHub API

API REST desenvolvida em .NET 8 seguindo os princípios da Arquitetura Limpa e padrão CQRS para gerenciamento de pessoas.

## 🏗️ Arquitetura

O projeto está estruturado em camadas seguindo a Arquitetura Limpa:

- **PeopleHub.API**: Camada de apresentação (Controllers, Program.cs)
- **PeopleHub.Application**: Camada de aplicação (CQRS - Commands, Queries, Handlers, DTOs)
- **PeopleHub.Domain**: Camada de domínio (Entidades, Interfaces)
- **PeopleHub.Infrastructure**: Camada de infraestrutura (Repositórios, DbContext, Serviços)
- **PeopleHub.Test**: Camada de testes unitários (Controllers, Helpers, Test Data Factory)

## 🚀 Tecnologias

- .NET 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- MediatR (CQRS)
- BCrypt.Net (Hash de senhas)
- Swagger/OpenAPI
- API Versioning

### 🧪 Tecnologias de Teste

- **xUnit**: Framework de teste principal
- **Moq**: Biblioteca para mocking/simulação
- **FluentAssertions**: Assertions mais legíveis e expressivas
- **Microsoft.AspNetCore.Mvc.Testing**: Testes de integração para ASP.NET Core

## 📋 Funcionalidades

### 🔐 Autenticação e Registro de Usuários
- ✅ **Registro de novo usuário**: Cria conta com acesso ao sistema (pessoa + usuário)
- ✅ **Login**: Autenticação via CPF e senha
- ✅ **JWT**: Tokens de acesso para endpoints protegidos

### 👥 Gerenciamento de Pessoas (CRUD Administrativo)
- ✅ **Cadastro**: Inserir novos registros de pessoas (somente dados pessoais)
- ✅ **Consulta**: Buscar registros de pessoas (todos ou por ID)
- ✅ **Alteração**: Atualizar informações de registros existentes
- ✅ **Remoção**: Excluir registros de pessoas

### Campos da Pessoa
- **Nome**: obrigatório
- **Sexo**: opcional
- **E-mail**: opcional, validado se preenchido
- **Data de Nascimento**: obrigatória, validada
- **Naturalidade**: opcional
- **Nacionalidade**: opcional
- **CPF**: obrigatório, validado (formato e unicidade)
- **Endereço**: obrigatório apenas na versão 2

### Versionamento
- **v1.0**: Versão original sem endereço obrigatório
- **v2.0**: Versão com endereço obrigatório

## 🧪 Testes Unitários

O projeto inclui uma suíte abrangente de testes unitários organizados para garantir a qualidade e confiabilidade do código.

### 📁 Estrutura dos Testes

```
PeopleHub.Test/
├── Controllers/
│   ├── AuthControllerTests.cs           # Testes do controller de autenticação
│   ├── Version1/
│   │   └── PeopleHubControllerV1Tests.cs # Testes do controller V1
│   └── Version2/
│       └── PeopleHubControllerV2Tests.cs # Testes do controller V2
└── Helpers/
    └── TestDataFactory.cs               # Factory para dados de teste
```

### 🎯 Cobertura de Testes

#### AuthController
- ✅ **Login Tests**:
  - Login com credenciais válidas
  - Login com username/password vazios
  - Login com credenciais inválidas
  - Tratamento de exceções (ArgumentException, UnauthorizedAccessException, Exception)
  
- ✅ **Register Tests**:
  - Registro com dados válidos
  - Registro com CPF duplicado
  - Registro com dados inválidos
  - Tratamento de exceções

#### PeopleHubController V1
- ✅ **GetAll Tests**:
  - Buscar todas as pessoas (com e sem dados)
  - Tratamento de exceções
  
- ✅ **GetById Tests**:
  - Buscar pessoa existente
  - Buscar pessoa inexistente
  - Tratamento de exceções
  
- ✅ **Create Tests**:
  - Criar pessoa com dados válidos
  - Criar pessoa com dados inválidos
  - Criar pessoa com CPF duplicado
  
- ✅ **Update Tests**:
  - Atualizar pessoa existente
  - Atualizar pessoa inexistente
  
- ✅ **Delete Tests**:
  - Deletar pessoa existente
  - Deletar pessoa inexistente
  - Tratamento de exceções

#### PeopleHubController V2
- ✅ **Todos os testes do V1** +
- ✅ **Testes específicos da V2**:
  - Validação de endereço obrigatório
  - Mapeamento correto para PersonDtoV2
  - Tratamento de erros específicos da V2

### 🔧 TestDataFactory

Helper class que fornece métodos para criar dados de teste consistentes:

- **PersonDtoV1/V2**: Criação de pessoas válidas e inválidas
- **LoginDto**: Credenciais de login para testes
- **RegisterDto**: Dados de registro para testes
- **Response DTOs**: Respostas esperadas dos endpoints
- **Constantes**: Valores comuns usados nos testes

### 🏃‍♂️ Executando os Testes

1. **Executar todos os testes**:
   ```bash
   dotnet test
   ```

2. **Executar testes com relatório de cobertura**:
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

3. **Executar testes específicos**:
   ```bash
   dotnet test --filter "AuthControllerTests"
   dotnet test --filter "PeopleHubControllerV1Tests"
   dotnet test --filter "PeopleHubControllerV2Tests"
   ```

4. **Executar testes no Visual Studio**:
   - Usar Test Explorer
   - Executar individualmente ou em grupo
   - Debug de testes específicos

### 📊 Padrões de Teste Implementados

- **AAA Pattern**: Arrange, Act, Assert
- **Mocking**: Uso de Moq para isolamento de dependências
- **Test Data Builders**: Factory pattern para criação de dados
- **Tipagem Forte**: Sem uso de `var` nos testes
- **Assertions Fluentes**: FluentAssertions para melhor legibilidade
- **Testes de Exceção**: Verificação de tratamento de erros
- **Verificação de Interações**: Verificação de chamadas para dependências

## 🔧 Configuração

### Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "workstation id=PeopleHub-Staged.mssql.somee.com;packet size=4096;user id=Matheusmarques_SQLLogin_1;pwd=u153bmkblh;data source=PeopleHub-Staged.mssql.somee.com;persist security info=False;initial catalog=PeopleHub-Staged;TrustServerCertificate=True"
  }
}
```

### JWT Settings
```json
{
  "Jwt": {
    "SecretKey": "MySecretKeyForJWTTokenGeneration123456789",
    "Issuer": "PeopleHubAPI",
    "Audience": "PeopleHubUsers"
  }
}
```

## 📚 Endpoints

### 🔐 Autenticação (Públicos)
```
POST /api/Auth/register   # Registro de novo usuário (cria pessoa + usuário)
POST /api/Auth/login      # Login do usuário
```

### 👥 Pessoas - Versão 1.0 (Protegidos - Requer Token)
```
GET    /api/v1.0/PeopleHub           # Listar todas as pessoas
GET    /api/v1.0/PeopleHub/{id}      # Buscar pessoa por ID
POST   /api/v1.0/PeopleHub           # Criar nova pessoa (somente dados pessoais)
PUT    /api/v1.0/PeopleHub/{id}      # Atualizar pessoa
DELETE /api/v1.0/PeopleHub/{id}      # Deletar pessoa
```

### 👥 Pessoas - Versão 2.0 (Protegidos - Requer Token)
```
GET    /api/v2.0/PeopleHub           # Listar todas as pessoas (com endereço)
GET    /api/v2.0/PeopleHub/{id}      # Buscar pessoa por ID (com endereço)
POST   /api/v2.0/PeopleHub           # Criar nova pessoa (endereço obrigatório)
PUT    /api/v2.0/PeopleHub/{id}      # Atualizar pessoa (endereço obrigatório)
DELETE /api/v2.0/PeopleHub/{id}      # Deletar pessoa
```

## 🎯 Fluxos de Uso

### 📝 **Fluxo 1: Registro de Novo Usuário (Tela "Criar Conta")**
```json
POST /api/Auth/register
{
  "nome": "João Silva",
  "sexo": "M",
  "email": "joao@email.com",
  "dataNascimento": "1990-05-15",
  "naturalidade": "São Paulo",
  "nacionalidade": "Brasileira",
  "cpf": "12345678901",
  "password": "minhasenha123"
}
```
**Resultado**: Cria registro na tabela `People` + `Users` (com senha)

### 🔑 **Fluxo 2: Login**
```json
POST /api/Auth/login
{
  "username": "12345678901",  // CPF
  "password": "minhasenha123"
}
```
**Resultado**: Retorna token JWT para acessar endpoints protegidos

### 👥 **Fluxo 3: CRUD Administrativo de Pessoas V1 (SEM senha)**
```json
POST /api/v1.0/PeopleHub
{
  "nome": "Maria Santos",
  "sexo": "F",
  "email": "maria@email.com",
  "dataNascimento": "1985-03-22",
  "naturalidade": "Rio de Janeiro",
  "nacionalidade": "Brasileira",
  "cpf": "98765432109"
}
```
**Resultado**: Cria APENAS registro na tabela `People` (sem usuário, sem senha)

### 🏠 **Fluxo 4: CRUD Administrativo de Pessoas V2 (COM endereço, SEM senha)**
```json
POST /api/v2.0/PeopleHub
{
  "nome": "Pedro Oliveira",
  "sexo": "M", 
  "email": "pedro@email.com",
  "dataNascimento": "1992-11-08",
  "naturalidade": "Belo Horizonte",
  "nacionalidade": "Brasileira",
  "cpf": "45678912345",
  "endereco": "Rua da Paz, 789, Belo Horizonte, MG"
}
```
**Resultado**: Cria APENAS registro na tabela `People` com endereço (sem usuário, sem senha)

## 🧪 Usuários de Teste

### Credenciais (todos com senha: "123456")
- **admin** / admin@peoplehub.com
- **user1** / user1@peoplehub.com  
- **testuser** / test@peoplehub.com

## 🔍 Validações Implementadas

### CPF
- Formato correto (11 dígitos)
- Validação dos dígitos verificadores
- Unicidade no banco de dados

### Data de Nascimento
- Deve ser anterior à data atual

### E-mail
- Formato válido (quando preenchido)

### Campos Obrigatórios
- **V1**: Nome, Data de Nascimento, CPF
- **V2**: Nome, Data de Nascimento, CPF, Endereço
- **Register**: Nome, Data de Nascimento, CPF, Password

## 🏃‍♂️ Como Executar

1. **Restaurar pacotes**:
   ```bash
   dotnet restore
   ```

2. **Compilar o projeto**:
   ```bash
   dotnet build
   ```

3. **Executar os testes**:
   ```bash
   dotnet test
   ```

4. **Executar a aplicação**:
   ```bash
   dotnet run --project PeopleHub.API
   ```

5. **Acessar o Swagger**:
   ```
   https://localhost:7xxx/swagger
   ```

## 🎯 Padrões Utilizados

- **CQRS**: Separação de Commands (escrita) e Queries (leitura)
- **Repository Pattern**: Abstração do acesso a dados
- **Dependency Injection**: Inversão de dependências
- **Clean Architecture**: Separação clara de responsabilidades
- **Domain-Driven Design**: Foco no domínio da aplicação
- **Unit Testing**: Testes unitários abrangentes com mocking
- **Test-Driven Development**: Testes como documentação viva

## 📝 Notas Importantes

- **Separação de Responsabilidades**: 
  - `/api/Auth/register` = Criar conta de usuário (pessoa + login)
  - `/api/v{version}/PeopleHub` = CRUD administrativo de pessoas (sem login)
- Todas as mensagens de retorno estão em português
- O código está em inglês seguindo boas práticas
- JWT expira em 1 hora
- CORS configurado para aceitar qualquer origem
- Validação robusta de CPF com dígitos verificadores
- Suporte a versionamento de API mantendo compatibilidade
- **Login por CPF**: O sistema usa o CPF como username para fazer login
- **Cobertura de Testes**: Testes unitários abrangentes para todos os controllers
- **Mocking**: Isolamento de dependências usando Moq
- **Assertions Fluentes**: Uso de FluentAssertions para melhor legibilidade dos testes
