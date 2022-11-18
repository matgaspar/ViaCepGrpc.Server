using Microsoft.Extensions.DependencyInjection;
using ViaCepGrpc.Services.External;
using ViaCepGrpc.Services.External.Interfaces;

namespace ViaCepGrpc.IoC;
public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddHttpClient<IViaCepService, ViaCepService>(opt =>
        {
            opt.BaseAddress = new Uri("https://viacep.com.br");
        });
    }
}
