
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Shared.Requests;
using Shared.Responses;
using System.Threading.Tasks;
namespace Shared.Interfaces
{
   [Authorize]
    public class GreeterService : IGreeterService
    {
        
        public Task<Servidor_Responde> SayHelloAsync(Cliente_pergunta request, ServerCallContext context)
        {
           // _logger.LogInformation($"Sending hello to {request.Name}");
            return Task.FromResult(new Servidor_Responde
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
