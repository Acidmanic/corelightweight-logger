using System;

namespace Microsoft.Extensions.Logging.LightWeight.Tdd
{
    class Program
    {
        static void Main(string[] args)
        {
           var logger = new ConsoleLogger().EnableAll();
            
            logger.LogInformation("LogInformation");
            logger.LogError("LogError");
            logger.LogDebug("LogDebug");
            logger.LogCritical("LogCritical");
            logger.LogTrace("LogTrace");
            logger.LogWarning("LogWarning");
            
            logger = new ConsoleLogger().Shorten().EnableAll();
            
            logger.LogInformation("LogInformation");
            logger.LogError("LogError");
            logger.LogDebug("LogDebug");
            logger.LogCritical("LogCritical");
            logger.LogTrace("LogTrace");
            logger.LogWarning("LogWarning");
            
            
        }
    }
}
