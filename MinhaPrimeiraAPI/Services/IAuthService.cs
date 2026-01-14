using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(User request);
        Task<IResult> LoginAsync(User request);

    }
}
