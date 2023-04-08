using Microsoft.AspNetCore.Identity;
using Models;
using DTOs.UserRequestDTOs;
using ContactBook.Core;

namespace Core
{
    public class AuthUser : IAuthUser 
    {
        private readonly UserManager<Contact> _userManager; // private field to store a UserManager object

        public AuthUser(UserManager<Contact> userManager) 
        {
            _userManager = userManager; // sets the private field to the value of the argument passed to the constructor
        }

        /// <summary>
        /// // defines a method called "Login" that takes a LoginRequestDTO object as an argument
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns>Task<User></returns>
        /// <exception cref="AccessViolationException"></exception>
        public async Task<Contact> Login(LoginRequestDTO userRequest) 
        {
            // retrieves the user from the database using the user's email
            Contact user = await _userManager.FindByEmailAsync(userRequest.Email); 
            if (user != null) 
            {
                if (await _userManager.CheckPasswordAsync(user, userRequest.Password) == true) // checks if the password provided matches the user's password
                {
                    return user; // if the password matches, return the user object
                }
                throw new AccessViolationException("Wrong UserName or Password"); 
            }
            throw new AccessViolationException("Wrong UserName or Password"); 
        }



        /// <summary>
        /// defines a method called "Register" that takes a User object as an argument and returns a Task<User>
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="MissingFieldException"></exception>
        /// 


    

        public async Task<Contact> Register(Contact user) 
        {
            // if the user's username is empty or whitespace, set it to their email address
            user.UserName = String.IsNullOrWhiteSpace(user.UserName) ? user.Email : user.UserName; 
            user.CreatedAt = DateTime.Now;
           // user.PasswordHash = new PasswordHasher<User>().HashPassword(user); 

            // creates a new user in the database using the UserManager object
            IdentityResult result = await _userManager.CreateAsync(user, user.Password); 

            // if the user was created successfully
            if (result.Succeeded) 
            {
                return user; 
            }

            string errors = String.Empty; 
            foreach (var error in result.Errors) // for each error in the IdentityResult object
            {
                // add the error message to the errors string
                errors += error.Description + Environment.NewLine; 
            }
            throw new MissingFieldException(errors);
        }
    }

}
