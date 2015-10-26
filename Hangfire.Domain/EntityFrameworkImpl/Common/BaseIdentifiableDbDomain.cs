using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hangfire.Domain.Api.Common;

namespace Hangfire.Domain.EntityFrameworkImpl.Common
{
    public class BaseIdentifiableDbDomain: IIdentifiable, IDbDomain
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("entity_status")]
        public EntityStatus EntityStatus { get; set; }

        [Timestamp]
        [Column("row_version")]
        public byte[] RowVersion { get; set; }

        protected BaseIdentifiableDbDomain()
        {
            EntityStatus = EntityStatus.Active;
        }
    }
}