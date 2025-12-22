using JWT.Model;
using JWT.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT.Services
{
    public class AuthService : IAuthService
    {
        // private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        //IConfiguration is used to read jwt setting from Appsetting.JSon file
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }




        public async Task<string> Register(User user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                return "Password & Confirmpassword are not matching";
            }
            var userExists = await _context.Users.AnyAsync(u => u.UserName == user.UserName);
            if (userExists)
            {
                return "The user already exist in the database";
            }
            //Adding new user in the Database

            user.ConfirmPassword = user.Password;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return "User registered successfully";
        }


        //It validates user credentials and generates jwt token.
        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName);

            if (user == null || user.Password != loginDTO.Password)
            {
                return null;
            }
            return GenerateJWTToken(user);
        }

        private string GenerateJWTToken(User user)
        {
            //Read JWT configuration from  appsetting.JSON file

            var jwtSettings = _configuration.GetSection("JWT");

            //Here we are creating a secreat key using Appsettingkey , this is key is used to sign & it will validate the token .

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            //Here we defined singing credentials using HMACSHA256 algo
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            //Here we are adding claims also we can store user information inside the Token .
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),

                new Claim("UserID",user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            var Token = new JwtSecurityToken
                (
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpireMinutes"] ?? "30")),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);



        }
    }
}






