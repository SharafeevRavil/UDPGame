using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UDPGameBackend.Entities;
using UDPGameBackend.Model;
using UDPGameBackend.Services;

namespace UDPGameBackend.Controllers
{
    public class UserController :ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(user);
        }

        [HttpPost("register")]
        public IActionResult Registrate([FromBody] AuthModel model)
        {
            var users = _userService.GetAll().Where(x => x.Email == model.Email);
            if (users.Count()==0)
            {
                return BadRequest(new {message = "Email ocupaited"});
            }
            else
            {
                this._userService.AddUser(new User{Id = _userService.GetAll().Count(), Email = model.Email,Password = model.Password});
            }

            return Ok(this._userService.Authenticate(model.Email,model.Password));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

    }
}