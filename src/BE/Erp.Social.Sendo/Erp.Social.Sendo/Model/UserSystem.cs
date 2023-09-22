using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Erp.Social.Sendo.Model
{
    public class UserSystem
    {
        public int id { get; set; }
        public int DId { get; set; }
        [Required(ErrorMessage = "UserName is not null")]
        [RegularExpression("^[a-zA-Z0-9]{6,}$", ErrorMessage = "UserName must be longer than 6 characters and must not contain spaces")]

        public string UserName { get; set; }
        [Required(ErrorMessage = "PassWord is not null")]
        [RegularExpression("^[a-zA-Z0-9]{6,}$", ErrorMessage = "PassWord must be longer than 6 characters and must not contain spaces")]

        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int CreateBy { get; set; }
        public DateTime LastModifyAt { get; set; } = DateTime.Now;
        public int LastModifyBy { get; set; }
        [DefaultValue("1")]
        public string Flag { get; set; }
    }
}
