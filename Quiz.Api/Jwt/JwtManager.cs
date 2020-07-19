using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Quiz.Api.Jwt
{
    public class JwtManager
    {
        private readonly byte[] PRIVATE_KEY = new byte[256];
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtManager()
        {
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(PRIVATE_KEY);

            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateJwtToken(int roomId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(QuizJwtClaimTypes.RoomId, roomId.ToString()));

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(PRIVATE_KEY), SecurityAlgorithms.HmacSha256Signature);

            var token = _tokenHandler.CreateJwtSecurityToken(subject: claims,
                signingCredentials: signingCredentials);

            return _tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(PRIVATE_KEY)
            };

            try
            {
                _tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            } catch
            {
                return false;
            }
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            return _tokenHandler.ReadJwtToken(token).Claims;
        }
    }
}
