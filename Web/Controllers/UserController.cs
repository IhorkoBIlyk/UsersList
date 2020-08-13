using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.DTO;
using Web.Models;
using Web.Service.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
           var result = await _userService.Get();
           return new JsonResult(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO newUser)
        {
            var result = await _userService.Create(newUser);
            return new JsonResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserDTO user)
        {
            var result = await _userService.Edit(user.Id, user);

            if (result)
                return Ok("User was created successfully");
            else
                return NotFound("User was not found");
        }
    

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);

            if (result)
                return NotFound("User was not found");
            else
                return Ok("User was deleted successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRange(IEnumerable<UserDTO> users)
        {
            try
            {
                await _userService.DeleteRange(users);
                return Ok("Users was successfuly deleted");
            }
            catch (Exception)
            {
                return NotFound("Users was not found");
            }
        }
    }
}
