using BookShopMng.Services;
using BookShopMng.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookShopMng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        public UserController(IUserService userService)

        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UsersInformation admin)
        {
            var useradmin = _userService.Authentication(admin.UserName, admin.Password);
            if (useradmin == null)
            { 
                return BadRequest(new { message = "Username or Password is incorrect" });
            }
            return Ok(useradmin);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("UserRegistration")]
        public async Task<IActionResult> UserRegistration([FromBody]UsersInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var modelid = await _userService.AddUser(model);
                    if (modelid > 0)
                    {
                        return Ok(modelid);
                    }
                    else
                    {
                        return NotFound();
                        //return BadRequest(new { message = "User registration Failed" });
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetUsersInfo();
                if (users == null)
                {
                    return NotFound();
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody]UsersInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.AddUser(model);
                    if (user > 0)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        return NotFound();
                        //return BadRequest(new { message = "User creation failed" });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? UserId)
        {
            int result = 0;
            if (UserId == null)
            {
                return BadRequest();
            }
            try
            {
                result = await _userService.DeleteUser(UserId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody]UsersInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUser(model);
                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
}
}