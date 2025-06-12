# 📦 Para utilizar API de Pedidos

A API é feita em .NET e utiliza do Swagger UI para facilitar os endpoints e também utiliza a mensageria do RabbitMQ em um deles.  
Ela está configurada na URL [https://localhost:7113/swagger/index.html](https://localhost:7113/swagger/index.html).
Para que o método `PedidoPOST.cs` da API funcione corretamente, é **obrigatório** que o serviço RabbitMQ esteja instalado e em execução no seu docker desktop [docker desktop](https://www.docker.com/products/docker-desktop/).

---


## 🐇 Instalação e execução do RabbitMQ com Docker

### 📥 1. Baixar a imagem do RabbitMQ

Abra o terminal e execute:

```bash
docker pull rabbitmq:3-management
```

> Esta imagem oficial já inclui o **Management Plugin**, que fornece uma interface web para facilitar o gerenciamento.

---

### ▶️ 2. Iniciar o container RabbitMQ

Execute o comando abaixo para criar e rodar o container:

```bash
docker run -d --hostname rabbitmq-host --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Esse comando:

- Inicia o RabbitMQ em segundo plano (`-d`)
- Mapeia:
  - `5672`: porta usada pela aplicação
  - `15672`: porta do painel de gerenciamento

---

### 🌐 3. Acessar o painel de gerenciamento

Abra o navegador e vá até: [http://localhost:15672](http://localhost:15672)

Use as credenciais padrão:

```
Usuário: guest
Senha:  guest
```

---
### 🔐 Autenticação com JWT

- `POST /token`  
  Gera um token JWT válido para simular um login com autenticação dos demais endpoints. **Este é o único endpoint que não exige autenticação**.

#### Exemplo de implementação

```csharp
public class TokenPOST
{
    public static string Pattern => "/token";
    public static string[] Methods => [HttpMethod.Post.ToString()];
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
        };
        var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

        return Results.Ok(new { token = jwtSecurityTokenHandler.WriteToken(token) });
    }
}
```

---

### 📌 Endpoints protegidos por JWT

Todos os endpoints abaixo **exigem** o envio do token JWT no header `Authorization`:

```
Authorization: Bearer <seu_token>
```

- `POST /pedido`  
  Publica uma mensagem (JSON) na fila `criar-pedido` do RabbitMQ para que um consumer crie utilizando banco de dados.

- `GET /pedidos`  
  Retorna todos os pedidos registrados.

- `PUT /pedido/{id:int}`  
  Atualiza apenas o status de um pedido existente pelo ID.

#### Exemplo da definição com MapMethods

```csharp
app.MapMethods(PedidoPOST.Pattern, PedidoPOST.Methods, PedidoPOST.Handler);
```

---

## 🐇 Integração com RabbitMQ

O endpoint `POST /pedido` envia uma mensagem para a fila `criar-pedido`.  
Certifique-se de que o serviço RabbitMQ está **instalado e rodando** antes de utilizar este endpoint.

### ▶️ Como rodar o RabbitMQ com Docker

```bash
docker pull rabbitmq:3-management

docker run -d --hostname rabbitmq-host --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Acesse o painel de gerenciamento em [http://localhost:15672](http://localhost:15672):  
- **Usuário:** guest  
- **Senha:** guest

---

## 🧰 Tecnologias Utilizadas

| Pacote | Finalidade |
|--------|------------|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Middleware para autenticação via JWT |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Gerenciamento de identidade com EF Core |
| `Microsoft.AspNetCore.OpenApi` | Suporte a anotações OpenAPI (Swagger) |
| `Microsoft.EntityFrameworkCore` | ORM com suporte a LINQ e mapeamento objeto-relacional |
| `Microsoft.EntityFrameworkCore.SqlServer` | Integração do EF Core com SQL Server |
| `Microsoft.EntityFrameworkCore.Design` | Ferramentas de design para EF Core (ex: migrações) |
| `RabbitMQ.Client` | Cliente oficial C# para integração com RabbitMQ |
| `Swashbuckle.AspNetCore` | Geração de documentação interativa com Swagger UI |

---


## ✅ Pronto!

