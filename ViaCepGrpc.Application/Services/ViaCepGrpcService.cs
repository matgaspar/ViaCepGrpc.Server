using System.Text;
using Grpc.Core;
using ViaCepGrpc.Grpcs;
using ViaCepGrpc.Services.External.Interfaces;

namespace ViaCepGrpc.Application.Services;

public class ViaCepGrpcService : Grpcs.ViaCep.ViaCepBase
{
    private readonly ILogger<ViaCepGrpcService> _logger;
    private readonly IViaCepService _viaCepService;

    public ViaCepGrpcService(ILogger<ViaCepGrpcService> logger, IViaCepService viaCepService)
    {
        _logger = logger;
        _viaCepService = viaCepService;
    }

    public override async Task<ViaCepResponse> GetAddressByCep(ViaCepRequest viaCepRequest, ServerCallContext context)
    {
        var res = await _viaCepService.GetAddressByCep(viaCepRequest.Cep);
        var bodyStr = "";

        var req = context.GetHttpContext().Request;

        // Allows using several time the stream in ASP.Net Core
        req.EnableBuffering();

        // Arguments: Stream, Encoding, detect encoding, buffer size 
        // AND, the most important: keep stream opened
        using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
        {
            bodyStr = await reader.ReadToEndAsync();
        }

        // Rewind, so the core is not lost when it looks at the body for the request
        req.Body.Position = 0;

        if (res == null)
            return new();

        return res;
    }
}