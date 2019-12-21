using System.Collections.Generic;

namespace UDPGameBackend.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        
        public List<CharacteristicUser> CharacteristicUsers { get; set; }
    }
}