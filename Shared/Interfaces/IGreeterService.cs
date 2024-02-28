using Grpc.Core;
using Shared.Requests;
using Shared.Responses;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    [ServiceContract]
    public interface IGreeterService
    {
        [OperationContract]
       Task<Servidor_Responde> SayHelloAsync(Cliente_pergunta request,ServerCallContext context);
        // Task<Servidor_Responde> SayHelloAsync(Cliente_pergunta request, calcon context);

    }
}
