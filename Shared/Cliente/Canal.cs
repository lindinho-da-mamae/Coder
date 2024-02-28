using Grpc.Net;
using Grpc.Core;
using Grpc.Net.Client;
using Shared.Cliente.Serviço_Greeter;
using Shared.Interfaces;
using Shared.Requests;
using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;

namespace Shared.Cliente
{
    public class Canal
    {
        #region implementaçao do singleton
        private static Canal _notificaçao;
        private static readonly object _lock = new object();
        public static Canal GetInstance()
        {
            if (_notificaçao == null)
            {
                lock (_lock)
                {

                    if (_notificaçao == null)
                    {
                        _notificaçao = new Canal();
                    }
                }
            }
            return _notificaçao;
        }
        private Canal()
        {
            Definir_Canal();
        }
        #endregion

        #region propriedades
        public int Tentativacanal { get => tentativacanal; set => tentativacanal = value; }
        private int tentativacanal = 0;

        public GrpcChannel Channel { get => channel; set => channel = value; }
        private GrpcChannel channel;

        public bool Canalliberado { get => canalliberado; set => canalliberado = value; }
        private bool canalliberado = false;

        public int Porta { get => porta; set => porta = value; }
        int porta = 5004;

        public IGreeterService Client { get => client; set => client = value; }
        IGreeterService client;

        public CallCredentials Credentials { get => credentials; set => credentials = value; }
        CallCredentials credentials;

        public Metadata Headers { get => headers; set => headers = value; }
        Metadata headers;
        #endregion

        public async Task Definir_Canal()
        {
            if (Channel == null || Channel.State.ToString() == "Shutdown")
            {
                CallCredentials Credentials = CallCredentials.FromInterceptor((c, m) =>
                {
                    m.Add("Authorization",
                        "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNTgxOTYyNzI0LCJleHAiOjE1ODE5NjYzMjQsImlhdCI6MTU4MTk2MjcyNH0.VvYln0PgZQrFwBTx0Ik3TGGI43DxdVVxzHAXma-K5P0");
                    return Task.CompletedTask;
                });

                Channel = GrpcChannel.ForAddress("https://localhost:" + Porta,
                    new GrpcChannelOptions
                    {
                        Credentials = ChannelCredentials.Create(new SslCredentials(), Credentials)
                    });

                IAuthenticationService authenticationClient = Channel.CreateGrpcService<IAuthenticationService>();
                AuthenticationRequest authenticationRequest = new AuthenticationRequest { UserName = "admin", Password = "admin" };
                Responses.AuthenticationResponse authenticationResponse = await authenticationClient.Authenticate(authenticationRequest);
                Headers = new Metadata { { "Authorization", $"Bearer {authenticationResponse.AccessToken}" } };
                
             Notificaçao_Cliente.GetInstance().Mensagem_sistema = "token= " + authenticationResponse.AccessToken.ToString();
                Notificaçao_Cliente.GetInstance().Mensagem_sistema = "canal conectado na porta = "
                    + Porta.ToString();
            }
        }
    
        public async Task chamarserviço()
        {
            if (Tentativacanal < 10)
            {
                try
                {
                    Client = Channel.CreateGrpcService<IGreeterService>();
                    Task task = Task.Run(() => GreeterServiço.GetInstance().conversando(Headers));
                    Tentativacanal = 0;

                }
                catch (Exception ex)
                {
                    Tentativacanal++;
                    Notificaçao_Cliente.GetInstance().Mensagem_sistema =
                        "canal nao abriu. tentando abrir o canal " + Tentativacanal.ToString() +
                        " tentativas";
                    await Definir_Canal();
                    await chamarserviço();
                }
            }
            if (Tentativacanal > 10)
            {
                Notificaçao_Cliente.GetInstance().Mensagem_sistema =
                      "erro ao tentar abrir o canal na porta = " + Porta +
                      ". Verifique a porta ou se o servidor está ativo";
            }
        }
    }


}
//private static async Task ServerStream()
//{
//    var credentials = CallCredentials.FromInterceptor((c, m) => {
//        m.Add("Authorization",
//            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNTgxOTYyNzI0LCJleHAiOjE1ODE5NjYzMjQsImlhdCI6MTU4MTk2MjcyNH0.VvYln0PgZQrFwBTx0Ik3TGGI43DxdVVxzHAXma-K5P0");
//        return Task.CompletedTask;
//    });

//    var channel = GrpcChannel.ForAddress("https://localhost:5001/",
//        new GrpcChannelOptions
//        {
//            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
//        });

//    var client = new GrpcClientCount.ClientCountProvider.ClientCountProviderClient(channel);

//    var token = new CancellationTokenSource(TimeSpan.FromSeconds(5));
//    using var population = client.GetClientCount(
//        new Empty(),
//        cancellationToken: token.Token);
//    try
//    {
//        await foreach (var item in population.ResponseStream.ReadAllAsync(token.Token))
//        { Console.WriteLine(item.Count); }
//    }
//    catch (RpcException exc)
//    {
//        Console.WriteLine(exc.Message);
//    }

//}

//private static async Task ClientStream()
//{
//    var credentials = CallCredentials.FromInterceptor((c, m) => {
//        m.Add("Authorization",
//            "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3QxIiwibmJmIjoxNTgxOTYyNzI0LCJleHAiOjE1ODE5NjYzMjQsImlhdCI6MTU4MTk2MjcyNH0.VvYln0PgZQrFwBTx0Ik3TGGI43DxdVVxzHAXma-K5P0");
//        return Task.CompletedTask;
//    });

//    var channel = GrpcChannel.ForAddress("https://localhost:5001/",
//        new GrpcChannelOptions
//        {
//            Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
//        });

//    var client = new GrpcPopulation.PopulationProvider.PopulationProviderClient(channel);

//    using var populationRequest = client.GetPopulation();

//    foreach (var state in new[] { "NY", "NJ", "MD", "KY" })
//    {
//        await populationRequest.RequestStream.WriteAsync(new GrpcPopulation.PopulationRequest
//        {
//            State = state
//        });
//    }

//    await populationRequest.RequestStream.CompleteAsync();

//    var response = await populationRequest.ResponseAsync;

//    Console.WriteLine(response.Count);

//}





