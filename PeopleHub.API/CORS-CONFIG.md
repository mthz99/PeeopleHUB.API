# Configura��o de CORS - PeopleHub API

## ?? Problema Resolvido

O frontend estava recebendo o erro:
```
Access to XMLHttpRequest at 'http://peoplehubapi.somee.com/api/Auth/login' from origin 'http://localhost:5173' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: Redirect is not allowed for a preflight request.
```

## ?? Solu��es Implementadas

### 1. Configura��o de CORS Permissiva
```csharp
// CORS - Configura��o permissiva para API p�blica
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("*");
    });
    
    // Pol�tica adicional para desenvolvimento local
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

// Condicionar HTTPS redirect apenas para produ��o para evitar problemas de CORS
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
```

### 3. Configura��o de Ambiente de Desenvolvimento
- **appsettings.Development.json**: Desabilita HTTPS for�ado em desenvolvimento
- **Program.cs**: HTTPS redirect s� em produ��o

### 4. Configura��o de Kestrel
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

A API agora aceita requisi��es de:
- ? **Qualquer origem** (`*`) - Pol�tica padr�o
- ? **localhost:5173** (Vite/Vue.js)
- ? **localhost:3000** (React/Next.js)
- ? **localhost:8080** (Vue CLI)
- ? **Qualquer dom�nio em produ��o**

## ?? Como Testar

### 1. Frontend Local (localhost:5173)
```javascript
// Requisi��o do frontend
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

# Testar requisi��o real
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

## ? Principais Mudan�as

1. **Pol�tica Padr�o Permissiva**: `AllowAnyOrigin()` para aceitar qualquer frontend
2. **Ordem dos Middlewares**: CORS antes de HTTPS redirect
3. **HTTPS Condicional**: S� for�a HTTPS em produ��o
4. **Configura��o de Desenvolvimento**: Settings espec�ficos para dev
5. **M�ltiplas Pol�ticas**: Default permissiva + espec�fica para desenvolvimento

## ?? Resultado

Agora a API funciona com:
- ? Frontends locais (localhost:xxxx)
- ? Frontends em produ��o (qualquer dom�nio)
- ? Ferramentas de teste (Postman, curl, etc.)
- ? Diferentes frameworks (React, Vue, Angular, etc.)

A API � verdadeiramente **p�blica** e aceita requisi��es de **qualquer origem**!