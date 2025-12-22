using JWT.Model;

namespace JWT.Services.IServices
{
    public interface IAuthService
    {
        //It handles the user registration logic
        Task<string> Register(User user);

        Task<string> Login(LoginDTO loginDTO);
    }
}
