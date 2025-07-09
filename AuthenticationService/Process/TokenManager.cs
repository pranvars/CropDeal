using AuthenticationService.EntityModel;
using AuthenticationService.EntityModel;
using AuthenticationService.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Process
{

    public class TokenManager
    {
        private readonly IUserRepository _repository;
        private readonly AppSettings _appSettings;

        public TokenManager(
            IUserRepository repository,
            IOptions<AppSettings> appSettings)
            => (_repository, _appSettings) = (repository, appSettings.Value);

        public  string GetJwtToken( User user, Role role )
        {
            if (user is null)
                throw new ArgumentNullException("request", "Cannot be null.");

            var claims = new List<Claim>
            {
                new Claim("Email", user.Email),
                new Claim("Role", role.RoleName)
            };
            var identity = new ClaimsIdentity(claims);
            var secretKey = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var signingCred = new SigningCredentials(
                key: new SymmetricSecurityKey(secretKey),
                algorithm: SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signingCred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenizer = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(tokenizer);
            return token;
        }

        public string ValidateToken(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            //Generate the security key using the secret key
            SymmetricSecurityKey key = new SymmetricSecurityKey(secretKey);

            //Set the token validation parameters
            TokenValidationParameters tokenParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false, //validate the issuer signing key 
                IssuerSigningKey = key, //the signing key
                ValidateIssuer = false,  //Validate the issuer
                ValidateAudience = false, //validate the audience/clients
                ClockSkew = TimeSpan.Zero //set the clock skew to zero
            };
            try
            {
                //create a Security Token Handler object
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                //and validate the token. It returns a Security Token object
                tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken validatedToken);
                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                //extract the values from the token
                var email = jwtToken.Claims.First(x => x.Type == "Email").Value;

                return email;
            }
            catch (Exception ex) { }

            return string.Empty;
        }
    }
}
