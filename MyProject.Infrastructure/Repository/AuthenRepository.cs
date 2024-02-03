using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyProject.Infrastrcture;
using MyProject.Infrastructure.MyProjectDB;
using MyProject.Models.Dto;
using MyProject.Models.Model;

namespace MyProject.Infrastructure.Repository;

public class AuthenRepository : Repository<User>, IAuthenRepository
{
    private IConfiguration _configuration;
    public AuthenRepository(MyProjectDb context, IConfiguration configuration) : base(context)
    {
        _configuration = configuration;
    }
    
    public Token LoginAsync(UserDto user)
    {
        var token = new Token();
        var userData = Context.Set<User>().FirstOrDefault(u => u.UserName == user.UserName);
        if (userData == null) return token;
        else 
        {
            var role = "";
            if (userData.UserName.Contains("@admin")) role = "Admin";
            else role = "User";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Role", role),
            };

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            

            var userUpdate = new User
            {
                UserName = user.UserName,
                Password = user.Password,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
            };

            Context.Set<User>().Update(userUpdate);
            Context.SaveChanges();

            token.AccessToken = accessToken;
            token.RefreshToken = refreshToken;
            token.Expried = TimeSpan.FromMinutes(30);
            return token;
        }
    }

    public async Task<bool> RegisterAsync(UserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Password = userDto.Password,
        };
        //Kiểm tra xem tài khoản đã tồn tại chưa
        var checkUser = await Context.Set<User>()
            .Where(u => u.UserName == user.UserName)
            .FirstOrDefaultAsync();
        if (checkUser != null)
        {
            return false;
        }
        
        //Kiểm tra xem tai khoản dăng ký la admin hay nhân viên
        if (user.UserName.Contains("@admin.com")) //chua cum ky tu nay thi la admin
        {
            user.Role = 1;
            Context.Set<User>().Add(user);
            await Context.SaveChangesAsync();
        }
        else
        {
            user.Role = 0;
            Context.Set<User>().Add(user);
            await Context.SaveChangesAsync();
        }
        
        return true;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var token = new JwtSecurityToken(
            issuer: "https://localhost:7072/",
            audience: "http://127.0.0.1:5500/",
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        var asscessToken = new  JwtSecurityTokenHandler().WriteToken(token);
        return asscessToken;
    }

    public string GenerateRefreshToken()
    {
        var random = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }
    }
    
    //Lay phan tien to de tao token
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, //here we are saying that we don't care about the token's expiration date
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
    
    //Tao token moi
    public Token Refresh(Token tokenApiModel)
    {
        if (tokenApiModel is null)
            return new Token()
            {
                AccessToken = "Invalid client request",
                RefreshToken = "Invalid client request",
            };
        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;
        var principal = GetPrincipalFromExpiredToken(accessToken);
       
        var username = principal.Identity.Name; //this is mapped to the Name claim by default
        var user = Context.Set<User>().FirstOrDefault(u => u.UserName == username);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return new Token()
            {
                AccessToken = "Invalid client request",
                RefreshToken = "Invalid client request",
            };
        var newAccessToken = GenerateAccessToken(principal.Claims);
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        Context.SaveChanges();
        return (new Token()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Expried = TimeSpan.FromMinutes(30)
        });
    }
    
    public bool Revoke()
    {
        var username = "tung@admin";
        var user = Context.Set<User>().FirstOrDefault(u => u.UserName == username);
        if (user == null) return false;
        user.RefreshToken = null;
        Context.SaveChanges();
        return true;
    }
}