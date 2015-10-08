namespace Hangfire.Services.Api
{
    public interface IFileSystemStorage
    {
        string[] GetAllFiles();
        void ApplyWatermark(string picture);
    }
}