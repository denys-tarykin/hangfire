namespace Hangfire.Services.Api
{
    public interface IDelayTaskService
    {
        void Test(int a, int b);
        void ApplyWatermark(string picture);
        void StartServer();
    }
}