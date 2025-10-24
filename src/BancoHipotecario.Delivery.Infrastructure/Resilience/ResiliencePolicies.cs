using Microsoft.Data.SqlClient;
using Polly;
using Polly.Registry;

namespace BancoHipotecario.Delivery.Api
{
   

    public static class ResiliencePolicies
    {
        public static IPolicyRegistry<string> CreateRegistry()
        {
            var registry = new PolicyRegistry();

            registry.Add("SqlRetryPolicy", Policy
                .Handle<SqlException>()          // errores de SQL
                .Or<TimeoutException>()          // timeouts
                .WaitAndRetryAsync(
                    3,
                    intento => TimeSpan.FromSeconds(Math.Pow(2, intento))
                ));

            return registry;
        }
    }
}
