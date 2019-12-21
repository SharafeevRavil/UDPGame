using System.Collections.Generic;

namespace UDPGameBackend.Models
{
    public class Characteristic
    {
        public int CharacteristicId { get; set; }
        public int Name { get; set; }
        
        public List<CharacteristicUser> CharacteristicUsers { get; set; }
    }
}