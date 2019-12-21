using System.ComponentModel.DataAnnotations;

namespace UDPGameBackend.Model
{
    public class AuthModel
    {
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}