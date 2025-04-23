using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.SharedLibrary.DependencyInjection
{
    public static class JWTAuthenticationSchema
    {
        public static IServiceCollection AddJWTAuthenticationSchena(this IServiceCollection services, IConfiguration config) {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer("Bearer", options =>
              {
                  var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                  string issuer = config.GetSection("Authentication:Issuuer").Value;
                  string audience = config.GetSection("Authentication:Audience").Value!;
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = false,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = issuer,
                      ValidAudience = audience,
                      IssuerSigningKey = new SymmetricSecurityKey(key)
                  };
              });
            return services;
        }
    }
}
