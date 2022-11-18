using ViaCepGrpc.Grpcs;

namespace ViaCepGrpc.Services.External.Interfaces;

public interface IViaCepService
{
    Task<ViaCepResponse?> GetAddressByCep(string cep);
}