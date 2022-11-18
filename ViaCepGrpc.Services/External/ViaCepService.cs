
using System.Net.Http.Json;
using ViaCepGrpc.Grpcs;
using ViaCepGrpc.Services.External.Interfaces;

namespace ViaCepGrpc.Services.External;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _client;

    public ViaCepService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ViaCepResponse?> GetAddressByCep(string cep)
    {
        try
        {
            var res = await _client.GetAsync($"ws/{cep}/json");
            if (res.IsSuccessStatusCode)
            {
                var dataResponse = await res.Content.ReadFromJsonAsync<ViaCepResponse>();
                return dataResponse;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex?.InnerException);
        }
    }
}