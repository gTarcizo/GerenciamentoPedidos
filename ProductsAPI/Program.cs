using ProductsAPI.Endpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsAPI.Endpoints.Pedidos;
using ProductsAPI.Infra.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
   option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
      In = ParameterLocation.Header,
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      BearerFormat = "JWT",
      Scheme = "Bearer"
   });
   option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSqlServer<AppDbContext>(builder.Configuration["ConnectionStrings:PedidosDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequireDigit = false;
   options.Password.RequireUppercase = false;
   options.Password.RequireLowercase = false;
   options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
{
   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenvalidationParameters()
   {
      ValidateActor = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
      ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"])),
      ClockSkew = TimeSpan.Zero
   };
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

#region Pedido
app.MapMethods(PedidosGETAll.Pattern, PedidosGETAll.Methods, PedidosGETAll.Handler);
#endregion

#region Login
app.MapMethods(TokenPOST.Pattern, TokenPOST.Methods, TokenPOST.Handler);
#endregion

app.Run();
