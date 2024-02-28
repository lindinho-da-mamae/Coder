
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Autenticaçao;
using Shared.Interfaces;
using System.ComponentModel;
using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

using ProtoBuf.Grpc;
using Shared.Requests;
using Shared.Responses;
using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Server;
namespace Shared.Servidor.builder
{

    public class Construtor
    {
        #region implementaçao do singleton
        private Construtor() { }
        private static Construtor _notificaçao;
        private static readonly object _lock = new object();
        public static Construtor GetInstance()
        {
            if (_notificaçao == null)
            {
                lock (_lock)
                {
                    if (_notificaçao == null)
                    {
                        _notificaçao = new Construtor();
                    }
                }
            }
            return _notificaçao;
        }
        #endregion

        #region notificaçao de propriedades do singleton
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void construir(string[] args)
        {
            var key = Encoding.ASCII.GetBytes(JwtAuthenticationManager.JWT_TOKEN_KEY);

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(x =>
             {
                 x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             }).AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });
            builder.Services.AddAuthorization();

            builder.Services.AddCodeFirstGrpc(x =>
            {
                x.ResponseCompressionLevel = CompressionLevel.Optimal;
            });
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IGreeterService, GreeterService>();



            builder.Services.AddGrpc();


            builder.WebHost.ConfigureKestrel(x =>
            {
                x.Listen(IPAddress.Any, 5004, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    listenOptions.UseHttps();

                });
            });
            builder.Services.AddControllers();

            WebApplication app = builder.Build();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGrpcService<AuthenticationService>();
            app.MapGrpcService<GreeterService>();
            app.Run();

        }




    }
}
