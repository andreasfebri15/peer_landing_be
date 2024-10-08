﻿using DAL.DTO.Req;
using DAL.DTO.Rest;
using DAL.Repositoris.Service;
using DAL.Repositoris.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BE_PEER.Controllers
{
    [Route("rest/v1/user/[action]")]

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservices;

        public UserController(IUserService userservices)
        {
            _userservices = userservices;
        }


        [HttpPost]
        public async Task<IActionResult> Register(ReqRegisterUserDTO register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();
                    var errorMessage = new StringBuilder("Validation errors occured!");
                    return BadRequest(new RestBaseDTO<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }

                var res = await _userservices.Register(register);

                return Ok(new RestBaseDTO<string>
                {
                    Success = true,
                    Message = res.ToString(),
                    Data = res
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email already used")
                {
                    return BadRequest(new RestBaseDTO<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new RestBaseDTO<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _userservices.GetAllUsers();
                return Ok(new RestBaseDTO<List<ResUserDto>>
                {
                    Success = true,
                    Message = "List of users",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RestBaseDTO<List<ResUserDto>>
                {
                    Success = false,
                    Message = "List of users",
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userservices.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new RestBaseDTO<object>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    });
                }

                return Ok(new RestBaseDTO<ResUserDto>
                {
                    Success = true,
                    Message = "User retrieved successfully",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RestBaseDTO<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user",
                    Data = null
                });
            }
        }

        [HttpPost]

        public async Task<IActionResult> Login (ReqLoginDto loginDto)
        {
            try
            {
                var response = await _userservices.Login(loginDto);
                return Ok(new RestBaseDTO<object>
                {

                    Success = true,
                    Message = "User Login Success",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid Email or Password")
                {
                    return BadRequest(new RestBaseDTO<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new RestBaseDTO<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            }



        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userservices.GetUserById(id);
                await _userservices.DeleteUserById(id);
                return Ok(new RestBaseDTO<object>
                {
                    Success = true,
                    Message = "User deleted successfully",
                    Data = null
                });
            }
           
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RestBaseDTO<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, ReqEditDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();
                    var errorMessage = new StringBuilder("Validation error occured!");
                    return BadRequest(new RestBaseDTO<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                };
                await _userservices.GetUserById(id);
                var user = await _userservices.UpdateUserById(id, dto);
                return Ok(new RestBaseDTO<object>
                {
                    Success = true,
                    Message = "User updated successfully",
                    Data = user
                });
            }
          
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RestBaseDTO<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

    }
}




