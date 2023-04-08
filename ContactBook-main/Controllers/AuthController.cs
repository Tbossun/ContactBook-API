using AutoMapper;
using ContactBook.Core;
using Core.Interface;
using DTOs.UserRequestDTOs;
using DTOs.UserResponseDTOs;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUser _authUser;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;

        public AuthController(IAuthUser authUser, ITokenGenerator tokenGenerator, IMapper mapper)
        {
            _authUser = authUser;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
        }

        //Handles users login requests
        [HttpPost]
        [Route("login", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                Contact user = await _authUser.Login(loginRequest);
                var loggedUser = _mapper.Map<LoginResponseDTO>(user);
                loggedUser.Token = await _tokenGenerator.GenerateToken(user);
                return Ok(loggedUser);
            }
            catch (AccessViolationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Handles users registration request
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegRequestDTO regRequest)
        {
            try
            {
                Contact user = _mapper.Map<Contact>(regRequest);
                var result = await _authUser.Register(user);
                return Ok("Registration Successful");
            }
            catch (MissingFieldException errors)
            {
                return BadRequest(errors.Message);
            }
        }
    }
    }

