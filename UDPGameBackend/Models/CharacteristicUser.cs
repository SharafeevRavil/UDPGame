namespace UDPGameBackend.Models
{
    public class CharacteristicUser
    {
        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public float Value { get; set; }
    }
}