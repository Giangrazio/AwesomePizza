using System.ComponentModel.DataAnnotations;

namespace AwesomePizzaBLL.Models
{
    public class TokenRequestModel
    {
        [Required(ErrorMessage = "Il campo NickName è Richiesto")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "Il campo Password è Richiesto")]
        public string Password { get; set; }
    }
}
