using Microsoft.IdentityModel.Tokens;
using Shared.Requests;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Claims;
namespace Shared.Autenticaçao
{
    public static class JwtAuthenticationManager
    {
        public static readonly int JWT_TOKEN_VALIDITY = 30;

        public static readonly string JWT_TOKEN_KEY = "0ff455a2708394633e4bb2f88002e3cd80cbd76f";

        public static byte[] TokenKey => Encoding.ASCII.GetBytes(JWT_TOKEN_KEY);

        public static DateTime TokenExpiryDateTime => DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);

        public static SymmetricSecurityKey SymmetricSecurityKey => new(TokenKey);

        public static string Issuer => "nhanvuong.vn";

        public static string Audience => "nhanvuong.vn";

        private static SigningCredentials SigningCredentials => new(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);




        public static AuthenticationResponse Aunthenticate(AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest.UserName != "admin" || authenticationRequest.Password != "admin")
            {
                return null;
            }

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            List<Claim> claims = new List<Claim>
    {
      new(ClaimTypes.Name, authenticationRequest.UserName),
  //    new(ClaimTypes.Role, userRole)

    };

            JwtSecurityToken jwtSecurityToken = new(
              claims: claims,
              issuer: Issuer,
              audience: Audience,
              expires: TokenExpiryDateTime,
              signingCredentials: SigningCredentials
            );

            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new()
            {
                AccessToken = token,
                ExpiresIn = (int)TokenExpiryDateTime.Subtract(DateTime.Now).TotalSeconds
            };
        }
    }
}
