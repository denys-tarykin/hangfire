using System.ComponentModel.DataAnnotations.Schema;
using Hangfire.Domain.Api;
using Hangfire.Domain.EntityFrameworkImpl.Common;

namespace Hangfire.Domain.EntityFrameworkImpl
{
    [Table("users")]
    public class UserImpl : BaseIdentifiableDbDomain, IUser
    {
        [Column("username")]
        public string Username { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}