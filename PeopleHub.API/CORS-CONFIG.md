# Configuração de CORS - PeopleHub API

## ?? Problema Resolvido

O frontend estava recebendo o erro:
```
Access to XMLHttpRequest at 'http://peoplehubapi.somee.com/api/Auth/login' from origin 'http://localhost:5173' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: Redirect is not allowed for a preflight request.
```

## ?? Soluções Implementadas

### 1. Configuração de CORS Permissiva
```csharp
// CORS - Configuração permissiva para API pública
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("*");
    });
    
    // Política adicional para desenvolvimento local
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:8080", "https://localhost:3000", "https://localhost:5173", "https://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

### 2. Ordem Correta dos Middlewares
```csharp
// CORS deve vir ANTES de qualquer middleware que possa causar redirecionamento
app.UseCors();

// Condicionar HTTPS redirect apenas para produção para evitar problemas de CORS
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
```

### 3. Configuração de Ambiente de Desenvolvimento
- **appsettings.Development.json**: Desabilita HTTPS forçado em desenvolvimento
- **Program.cs**: HTTPS redirect só em produção

### 4. Configuração de Kestrel
```json
"Kestrel": {
  "Endpoints": {
    "Http": {
      "Url": "http://0.0.0.0:5000"
    },
    "Https": {
      "Url": "https://0.0.0.0:5001"
    }
  }
}
```

## ?? Origens Permitidas

A API agora aceita requisições de:
- ? **Qualquer origem** (`*`) - Política padrão
- ? **localhost:5173** (Vite/Vue.js)
- ? **localhost:3000** (React/Next.js)
- ? **localhost:8080** (Vue CLI)
- ? **Qualquer domínio em produção**

## ?? Como Testar

### 1. Frontend Local (localhost:5173)
```javascript
// Requisição do frontend
fetch('http://peoplehubapi.somee.com/api/Auth/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    username: 'seu-cpf',
    password: 'sua-senha'
  })
})
```

### 2. Curl para Testar CORS
```bash
# Testar preflight request
curl -X OPTIONS \
  -H "Origin: http://localhost:5173" \
  -H "Access-Control-Request-Method: POST" \
  -H "Access-Control-Request-Headers: Content-Type" \
  http://peoplehubapi.somee.com/api/Auth/login

# Testar requisição real
curl -X POST \
  -H "Origin: http://localhost:5173" \
  -H "Content-Type: application/json" \
  -d '{"username":"seu-cpf","password":"sua-senha"}' \
  http://peoplehubapi.somee.com/api/Auth/login
```

## ?? Headers de Resposta Esperados

A API agora retorna os seguintes headers de CORS:
```
Access-Control-Allow-Origin: *
Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS
Access-Control-Allow-Headers: *
Access-Control-Expose-Headers: *
```

## ? Principais Mudanças

1. **Política Padrão Permissiva**: `AllowAnyOrigin()` para aceitar qualquer frontend
2. **Ordem dos Middlewares**: CORS antes de HTTPS redirect
3. **HTTPS Condicional**: Só força HTTPS em produção
4. **Configuração de Desenvolvimento**: Settings específicos para dev
5. **Múltiplas Políticas**: Default permissiva + específica para desenvolvimento

## ?? Resultado

Agora a API funciona com:
- ? Frontends locais (localhost:xxxx)
- ? Frontends em produção (qualquer domínio)
- ? Ferramentas de teste (Postman, curl, etc.)
- ? Diferentes frameworks (React, Vue, Angular, etc.)

A API é verdadeiramente **pública** e aceita requisições de **qualquer origem**!