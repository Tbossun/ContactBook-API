
using ContactBook.Core;
using Core.Interface;
using DTOs.UserRequestDTOs;
using Hateoas;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserRepository : IUserRepository
    {
        
        private readonly UserManager<Contact> _userManager;

        public UserRepository(UserManager<Contact> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Contact> GetUserById(string userId) => await _userManager.FindByIdAsync(userId);

        public async Task<Contact> GetUserByEmail(string userEmail) => await _userManager.FindByEmailAsync(userEmail);

        public async Task<Contact> AddUser(Contact user)
        {
            user.UserName = String.IsNullOrWhiteSpace(user.UserName) ? user.Email : user.UserName;
            user.CreatedAt = DateTime.Now;

            IdentityResult result = await _userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                return user;
            }

            string errors = String.Empty;
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            throw new MissingFieldException(errors);
        }


        public IEnumerable<Contact> SearchUsers(UserActionParams userActionParams)
        {
            var collection = _userManager.Users;

            // Gets users from a specific state (Filtering)
            if (!string.IsNullOrWhiteSpace(userActionParams.StateName))
            {
                collection = collection.Where(user => user.State == userActionParams.StateName);
            }

            // Gets users whose First Name, Last Name or City matches a specific search word
            if (!string.IsNullOrWhiteSpace(userActionParams.SearchWord))
            {
                string searchWord = userActionParams.SearchWord;
                collection = collection.Where(user => user.City.Contains(searchWord)
                                            || user.FirstName.Contains(searchWord)
                                            || user.LastName.Contains(searchWord));
            }
            return collection.Skip(10 * (userActionParams.Page - 1))
                             .Take(10).ToList();
        }


        public PageList<Contact> GetAllUsers(int pageNo)
        {
            var collection = _userManager.Users;
            return PageList<Contact>.Create(collection, pageNo);
        }


        public async Task<bool> DeleteUser(string userId)
        {
            Contact user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return true;
                return false;
            }
            throw new ArgumentException("User does not exist");
        }


        public async Task<bool> UpdateUser(UpdateRequestDTO updateUser, string userId)
        {
            Contact user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.FirstName = String.IsNullOrWhiteSpace(updateUser.FirstName) ? user.FirstName : updateUser.FirstName;
                user.LastName = String.IsNullOrWhiteSpace(updateUser.LastName) ? user.LastName : updateUser.LastName;
                user.PhoneNumber = String.IsNullOrWhiteSpace(updateUser.PhoneNumber) ? user.PhoneNumber : updateUser.PhoneNumber;
                user.StreetAddress = String.IsNullOrWhiteSpace(updateUser.StreetAddress) ? user.StreetAddress : updateUser.StreetAddress;
                user.City = String.IsNullOrWhiteSpace(updateUser.City) ? user.City : updateUser.City;
                user.State = String.IsNullOrWhiteSpace(updateUser.State) ? user.State : updateUser.State;
                user.Email = String.IsNullOrWhiteSpace(updateUser.Email) ? user.Email : updateUser.Email;
                user.Password = String.IsNullOrWhiteSpace(updateUser.Password) ? user.Password : updateUser.Password;
                user.UserName = String.IsNullOrWhiteSpace(updateUser.UserName) ? user.UserName : updateUser.UserName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return true;
                return false;
            }
            throw new ArgumentException("User does not exist");
        }


        public async Task<bool> UpdateAvatarUrl(string Url, string Id)
        {
            Contact user = await _userManager.FindByIdAsync(Id);
            user.AvatarUrl = Url;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return true;
            return false;
        }
    }
}
