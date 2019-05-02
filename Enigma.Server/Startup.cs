using Microsoft.Extensions.DependencyInjection;

namespace Enigma.Server.Startup
{
    public class Startup
    {
        public static void StartUp()
        {
            var serviceCollection = new ServiceCollection
            {
                // In the future put bindings in here!
            }.BuildServiceProvider();
        }
    }
}
