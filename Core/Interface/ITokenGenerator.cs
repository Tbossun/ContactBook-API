
using Models;

namespace Core.Interface
{
    public interface ITokenGenerator 
    {
        Task<string> GenerateToken(Contact user);
    }
}
