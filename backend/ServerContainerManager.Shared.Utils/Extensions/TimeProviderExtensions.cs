namespace ServerContainerManager.Shared.Utils.Extensions
{
    public static class TimeProviderExtensions
    {
        public static DateTime GetUtcDateTimeNow(this TimeProvider timeProvider) => timeProvider.GetUtcNow().UtcDateTime;
    }
}
