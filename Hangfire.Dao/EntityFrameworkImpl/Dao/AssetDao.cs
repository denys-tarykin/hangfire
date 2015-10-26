using Hangfire.Dao.Api;
using Hangfire.Dao.EntityFrameworkImpl.Common;
using Hangfire.Domain.Api;
using Hangfire.Domain.EntityFrameworkImpl;
using Microsoft.Practices.Unity;

namespace Hangfire.Dao.EntityFrameworkImpl.Dao
{
    public class AssetDao : GenericEntityFrameworkDaoIdentifiableSupport<IAsset, AssetImpl>, IAssetDao
    {
        public AssetDao(IUnityContainer unityContainer) : base(unityContainer)
        {

        }
    }
}