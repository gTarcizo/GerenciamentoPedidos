using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ProductsAPI.Endpoints.Security;

public class TokenPOST
{
   public static string Pattern => "/token";
   public static string[] Methods => [HttpMethod.Post.ToString()];
   public static Delegate Handler => Action;

   [AllowAnonymous]
   public static async Task<IResult> Action(IConfiguration configuration)
   {
      try
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
      catch (Exception e)
      {
         return Results.BadRequest(e.ToString());
      }
   }
}
