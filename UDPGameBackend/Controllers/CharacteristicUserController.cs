using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDPGameBackend.Models;

namespace UDPGameBackend.Controllers
{
    public class CharacteristicUserController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CharacteristicUserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddCharacteristic(string userName, string characteristicName, float addValue)
        {
            if (_dbContext.Users.All(x => x.Name != userName))
            {
                _dbContext.Users.Add(new User() {Name = userName});
            }

            var user = _dbContext.Users.Include(x => x.CharacteristicUsers)
                .ThenInclude(x => x.Characteristic).First(x => x.Name == userName);
            if (user.CharacteristicUsers.All(x => x.Characteristic.Name != characteristicName))
            {
                if (_dbContext.Characteristics.All(x => x.Name != characteristicName))
                {
                    _dbContext.Add(new Characteristic() {Name = characteristicName});
                }

                user.CharacteristicUsers.Add(new CharacteristicUser()
                {
                    Characteristic = _dbContext.Characteristics.First(x => x.Name == characteristicName),
                    User = user, 
                    Value = 0
                });
            }

            user.CharacteristicUsers.First(x => x.User.Name == userName && x.Characteristic.Name == characteristicName)
                .Value += addValue;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetByUserName(string userName)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Name == userName);
            if (user == null)
            {
                return Ok();
            }

            var characteristicUsers = _dbContext.CharacteristicUsers.Include(x => x.Characteristic).Where(x => x.UserId == user.UserID);
            return Ok(characteristicUsers.Select(x => $"{x.Characteristic.Name} : {x.Value}").Aggregate((x,y) => x + '\n' + y));
        }
    }
}