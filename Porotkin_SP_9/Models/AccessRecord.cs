using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Porotkin_SP_9.Models
{
    public class AccessRecord
    {
        [Key] public Guid Id { get; set; }
        [DisplayName("Login")] public string Login { get; set; }
        [DisplayName("Password Hash")] public string Passhash { get; set; }
        [DisplayName("E-mail")] public string Email { get; set; }
    }
}