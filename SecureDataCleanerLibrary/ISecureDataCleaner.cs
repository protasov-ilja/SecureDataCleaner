using SecureDataCleanerLibrary.Models;

namespace SecureDataCleanerLibrary
{
    public interface ISecureDataCleaner
    {
        HttpResult CleanHttpResult(HttpResult httpResult);
    }
}
