using Abp.Runtime.Security;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YunDa.ISAS.Application;
using YunDa.ISAS.Core.Helper;
using YunDa.ISAS.DataTransferObject.Session;

namespace YunDa.ISAS.Web.Core.Authentication
{
    public class TokenAuthConfiguration
    {
        public SymmetricSecurityKey SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public TimeSpan Expiration { get; set; }

        public string CreateAccessToken(LoginUserOutput userOutput, TimeSpan? expiration = null)
        {
            if (userOutput == null) return null;
            IEnumerable<Claim> claims = TokenAuthConfiguration.CreateClaims(userOutput);
            var now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: this.Issuer,
                audience: this.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? this.Expiration),
                signingCredentials: this.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public static IEnumerable<Claim> CreateClaims(LoginUserOutput userOutput)
        {
            if (userOutput == null)
                userOutput = new LoginUserOutput();
            var claims = new List<Claim>();
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, StringHelper.GuidToLongID(userOutput.Id).ToString()),
                //new Claim(JwtRegisteredClaimNames.Jti, userOutput.Id.ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                new Claim(ClaimTypes.Sid, userOutput.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, StringHelper.GuidToLongID(userOutput.Id).ToString()),
                new Claim(ClaimTypes.Name, userOutput.UserName),
                new Claim(ClaimTypes.Role, ""),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userOutput))
            });

            return claims;
        }

        public string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
    }
}