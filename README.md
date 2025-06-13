# Para utilizar API de Pedidos

A API é feita em .NET e utiliza do Swagger UI para facilitar os endpoints que são gravados no banco usando EF Core e também utiliza a mensageria do RabbitMQ em um deles.  
Ela está configurada na URL local localhost:5109.
E todos updates de database são feitos no ProductsAPI

## Requisitos
- Você irá precisar de uma versão do Visual Studio code;
- NodeJS na versão 20.17.0;
- .NET na versão 8
- Angular na versão 19.0.0
- Docker Desktop
- Imagem do RabbitMQ instalada
- SQL Server
--
# Preparando 
Instale o docker desktop no [site oficial do Docker](https://www.docker.com/products/docker-desktop/) e assim você conseguira instalar o RabbitMQ nele;

## Instalação e execução do RabbitMQ com Docker

###  Baixar a imagem do RabbitMQ

Abra o terminal e execute:

```bash
docker pull rabbitmq:3-management
```

> Esta imagem oficial já inclui o **Management Plugin**, que fornece uma interface web para facilitar o gerenciamento.

---

###  Iniciar o container RabbitMQ

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

###  Para acessar o painel de gerenciamento

Abra o navegador e vá até: [http://localhost:15672](http://localhost:15672)

Use as credenciais padrão:

```
Usuário: guest
Senha:  guest
```

---
## Instalações do Projeto

Caso precise trocar a versão do NodeJS para a 20.17.0 você pode usar o [NVM](https://github.com/coreybutler/nvm-windows/releases) disponivel no link;
com o NVM você escolhe a versão do node que quer usar digitando por exemplo:

```bash
nvm install 20.17.0
```

no meu caso, angular foi instalado globalmente executando o comando abaixo no cmd:

```bash
npm install -g @angular/cli@19.0.0
```

Instale também o .NET 8 na sua maquina através do link do [.NET](https://dotnet.microsoft.com/pt-br/download) a seguir:
```link
https://dotnet.microsoft.com/pt-br/download
```

Após baixar o repositório, vá até o caminho de onde você baixou e entre na pasta **/angular-pedidos** através do console e digite para instalar tudo que está dentro do :
```bash
npm i
```
Agora você pode rodar o app angular dentro da mesma pasta **/angular-pedidos** através do comando:

```bash
ng serve -o
```

Abra um segundo console e entre na pasta **/ConsumerConsoleExample** e use o comando para restaurar os packages do NuGet:

```bash
dotnet restore
```
após isso, pode iniciar usando

```bash
dotnet watch run
```

Para garantir, faça isso nas outras 2 pastas dos projetos:

Para garantir, bra um segundo console e entre na pasta **/ProductsAPI** e use o comando para restaurar os packages do NuGet:

```bash
dotnet restore
```
**Após atualizar sua connection string no appsettings.Development.json**, você pode atualizar seu banco dentro do ProductsAPI usando o comando:
```bash
dotnet ef database update --context AppDbContext
```

após isso, pode iniciar usando:

```bash
dotnet watch run
```
Para garantir, bra um segundo console e entre na pasta **/Shared** e use o comando para restaurar os packages do NuGet:

```bash
dotnet restore
```
após isso, pode iniciar usando:

```bash
dotnet watch run
```
---

## 🧰 Tecnologias Utilizadas nos Projetos

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
  Atualiza **apenas** o status de um pedido existente pelo Id.

---

# 📦 Sobre o ConsumerConsoleExample

O ConsumerConsoleExample é apenas um exemplo de **Worker monolito** de consumer para **simular** um acesso a fila do RabbitMQ para criar um dado no banco com EF Core. 
Para utiliza-lo você deve garantir que a RabbitMQ esteja rodando.

Apenas substitua a connection string de forma **manual** da Program.cs do projeto de ConsumerConsoleExample para a sua como no exemplo abaixo:

```csharp
         var host = Host.CreateDefaultBuilder(args)
             .ConfigureServices((context, services) =>
             {
                services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer("<Sua Connection String>")); // Aqui ^^
             })
             .Build();
```

---

## ✅ Pronto!

---
