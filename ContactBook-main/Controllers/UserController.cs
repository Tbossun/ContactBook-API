using AutoMapper;
using ContactBook.Core;
using DTOs.UserRequestDTOs;
using DTOs.UserResponseDTOs;
using Hateoas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;
using System;
using ImageUploadService.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace ContactBookApi.Controllers
{
    [ApiController]
    [Route("api/User")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IImageService imageService, IMapper mapper)
        {
            _userRepository = userRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new user to the datastore
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("Add-New")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewUser([FromBody] RegRequestDTO newUser)
        {
            try
            {
                Contact user = _mapper.Map<Contact>(newUser);
                var result = await _userRepository.AddUser(user);
                return Ok("User Added Successfully");
            }
            catch (MissingFieldException errors)
            {
                return BadRequest(errors.Message);
            }
        }

        /// <summary>
        /// Returns a paginated list of all users
        /// </summary>
        /// <param name="userActionParams"></param>
        /// <returns></returns>
        [HttpGet("Get-All", Name = "GetAllUsers")]
       //[Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers(int page)
        {
            if (page <= 0)
                page = 1;
            PageList<Contact> listOfUsers = _userRepository.GetAllUsers(page);

            var previousPageLink = listOfUsers.HasPrevious ?
                PageUrl(page, PagedLinkType.PreviousPage) : null;

            var nextPageLink = listOfUsers.HasNext ?
                PageUrl(page, PagedLinkType.NextPage) : null;

            var paginationMetaData = new
            {
                totalCount = listOfUsers.TotalCount,
                pageSize = listOfUsers.PageSize,
                currentPage = listOfUsers.CurrentPage,
                totalPages = listOfUsers.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
            return Ok(_mapper.Map<IEnumerable<GetUserDTO>>(listOfUsers));
        }

        /// <summary>
        /// Fetches Users using a search word or filtering by state name
        /// </summary>
        /// <param name="userActionParams"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        //[Authorize(Roles = "Admin")]
        public IActionResult SearchUsers([FromQuery] UserActionParams userActionParams)
        {
            IEnumerable<Contact> listOfUsers = _userRepository.SearchUsers(userActionParams);
            if (listOfUsers.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<GetUserDTO>>(listOfUsers));
        }

        /// <summary>
        /// Returns a user by using user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get-By-Id", Name = "GetUser")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            Contact user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GetUserDTO>(user));
        }

        /// <summary>teuser method so it taks
        /// Returns a user by using user email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("Get-By-Email")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            Contact user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GetUserDTO>(user));
        }


        /// <summary>
        /// Updates the records of an existing user
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser(UpdateRequestDTO updateUser, string userId)
        {
            try
            {
                bool result = await _userRepository.UpdateUser(updateUser, userId);
                if (result)
                {
                    return Ok("User updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update user");
                }
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Updates the Avatar Url of a user
        /// </summary>
        /// <param name="userId">The ID of the user to update</param>
        /// <param name="imageDto">The image data to upload</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("Photo-Upload")]
        public async Task<IActionResult> UploadProfilePic(string userId, [FromForm] ImageDto imageDto)
        {
            try
            {
                var upload = await _imageService.ImageUploadAsync(imageDto.Image);
                string url = upload.Url.ToString();

                bool result = await _userRepository.UpdateAvatarUrl(url, userId); // Pass the URL and userId to UpdateAvatarUrl method
                if (result)
                    return NoContent();
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        /// <summary>
        /// Removes a user from the data store using the user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            try
            {
                bool result = await _userRepository.DeleteUser(id);
                if (result == true)
                    return NoContent();
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Creates previous and next page urls for the response header
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string PageUrl(int page, PagedLinkType type)
        {
            return type switch
            {
                PagedLinkType.PreviousPage => Url.Link("GetAllUsers",
                new
                {
                    PageNumber = page - 1,
                }),
                PagedLinkType.NextPage => Url.Link("GetAllUsers",
                new
                {
                    PageNumber = page + 1,
                }),
                _ => Url.Link("GetAllUsers",
                new
                {
                    page,
                }),
            };
        }
    }
}
