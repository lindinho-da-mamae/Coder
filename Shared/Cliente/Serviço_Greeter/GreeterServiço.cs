
using Grpc.Core;
using Shared.Requests;
using Shared.Responses;
using System;
using System.Threading.Tasks;

namespace Shared.Cliente.Serviço_Greeter
{
    public class GreeterServiço
    {
        #region implementaçao do singleton
        private GreeterServiço() { }
        private static GreeterServiço _notificaçao;
        private static readonly object _lock = new object();
        public static GreeterServiço GetInstance()
        {
            if (_notificaçao == null)
            {
                lock (_lock)
                {
                    if (_notificaçao == null)
                    {
                        _notificaçao = new GreeterServiço();
                    }
                }
            }
            return _notificaçao;
        }
        #endregion
        int i = 0;
        public int Tentativaclient { get => tentativaclient; set => tentativaclient = value; }
        int tentativaclient = 0;

        public async Task conversando(Metadata headers)
        {
            if (tentativaclient < 10)
            {
                try
                {i++; 
                    Cliente_pergunta input = new Cliente_pergunta { Name =i.ToString()+ " Lindo" };
                   Servidor_Responde reply = await Canal.GetInstance().Client.SayHelloAsync(input, headers);
                   Notificaçao_Cliente.GetInstance().Mensagem_recebida = "Greeting: " + reply.Message;
                    Tentativaclient = 0;
                    return;
                }
                catch (Exception ex)
                {
                    Tentativaclient++;
                    Notificaçao_Cliente.GetInstance().Mensagem_sistema =
                      "canal nao abriu. tentando abrir o canal " + Tentativaclient.ToString() +
                      " tentativas";
                      await Canal.GetInstance().Definir_Canal();
                    await Canal.GetInstance().chamarserviço();
                    return;
                }
            }
            if (Tentativaclient > 10)
            {
                Notificaçao_Cliente.GetInstance().Mensagem_sistema =
                      "erro ao tentar se comunicar com o servidor na porta = " + Canal.GetInstance().Porta +
                      ". Verifique a porta ou se o servidor está ativo";
            }
        }
    }
}




