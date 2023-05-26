using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeersApi.NUnit.FeatureTests.Infrastructure.Authentication
{
   public class TestEnvAuthentication
   {
      private readonly string _securityKey;
      private readonly string _validIssuer;
      private readonly string _validAudience;

      public TestEnvAuthentication(IConfiguration configuration)
      {
         _securityKey = configuration["Jwt:SecurityKey"] ??
                        throw new InvalidOperationException("Set the 'Jwt:Security' on appSettings");
         _validAudience = configuration["Jwt:ValidAudience"] ??
                          throw new InvalidOperationException("Set the 'Jwt:ValidAudience' on appSettings");
         _validIssuer = configuration["Jwt:ValidIssuer"] ??
                        throw new InvalidOperationException("Set the 'Jwt:ValidIssuer' on appSettings");
      }

      public string GenerateToken(IEnumerable<Claim> claims) => GetJwtToken(claims);

      private string GetJwtToken(IEnumerable<Claim> claims)
      {
         var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
         var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

         var tokenOptions = new JwtSecurityToken(
            _validIssuer,
            _validAudience,
            claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: signinCredentials
         );

         return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
      }
   }
}
