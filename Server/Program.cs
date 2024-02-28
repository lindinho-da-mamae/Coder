using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shared.Servidor;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using System.Net;
using ProtoBuf.Grpc.Server;
namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Shared.Servidor.builder.Construtor.GetInstance().construir(args);
        
        }

   
    }
}




