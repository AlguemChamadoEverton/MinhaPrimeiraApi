using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(User request);
        Task<IResult> LoginAsync(User request);

    }
}
