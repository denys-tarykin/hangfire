using System.ComponentModel.DataAnnotations.Schema;
using Hangfire.Domain.Api;
using Hangfire.Domain.EntityFrameworkImpl.Common;

namespace Hangfire.Domain.EntityFrameworkImpl
{
    [Table("assets")]
    public class AssetImpl : BaseIdentifiableDbDomain, IAsset
    {
        [Column("name")]
        public string Name { get; set; }
        
        [Column("path")]
        public string Path { get; set; }
    }
}