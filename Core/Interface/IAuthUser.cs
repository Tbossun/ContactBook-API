using DTOs.UserRequestDTOs;
using Models;

namespace ContactBook.Core
{
    public interface IAuthUser
    {
        Task<Contact> Login(LoginRequestDTO userRequest);
        Task<Contact> Register(Contact user);
    }
}
