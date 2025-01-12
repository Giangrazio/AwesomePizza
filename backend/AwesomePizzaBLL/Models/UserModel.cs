using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePizzaBLL.Models
{
    public class UserModel
    {
        public string CodeValue { get; set; }
        public string? Denomination { get; set; }
        public string? RoleName { get; set; }
        public string NickName { get; set; }
    }
}
