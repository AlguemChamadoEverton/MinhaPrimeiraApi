using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(UserModel request);
        Task<IResult> LoginAsync(UserModel request);

    }
}
