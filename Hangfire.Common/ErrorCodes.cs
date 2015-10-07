namespace Hangfire.Common
{
    public enum ErrorCodes
    {
        InternalServerError = 500,
        None = 0,
        UnexpectedRequestBody = 1,
        LoginIncorrect = 2,
        LoginDisabled = 3,
        UnclassifiedError = 999
    }
}