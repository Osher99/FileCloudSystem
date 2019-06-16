using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using FileProject.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]User newUser)
        {         
                try
                {
                    var User = await _userService.Register(newUser);
                    if (User != null)
                        return new ObjectResult(User);
                    else
                        return NotFound();
                }
                catch (Exception)
                {
                    return BadRequest();
                }         
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]User loginDetails)
        {
            try
            {
                var User = await _userService.Login(loginDetails);
                if (User != null)
                    return new ObjectResult(User);
                else
                    return NotFound();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}