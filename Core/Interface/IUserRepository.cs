using DTOs.UserRequestDTOs;
using Models;
using Hateoas;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core
{
    public interface IUserRepository
    {
        PageList<Contact> GetAllUsers(int pageNo);
        Task<Contact> AddUser(Contact user);
        Task<bool> DeleteUser(string userId);
        Task<Contact> GetUserById(string userId);
        Task<Contact> GetUserByEmail(string email);
        IEnumerable<Contact> SearchUsers(UserActionParams userActionParams);
        Task<bool> UpdateUser(UpdateRequestDTO updateUser, string id);
        Task<bool> UpdateAvatarUrl(string Url, string Id);
    }
}
