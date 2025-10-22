using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMongo.Application.VOs
{
    public class LoginVO
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
