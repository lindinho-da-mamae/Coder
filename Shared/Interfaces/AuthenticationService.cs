using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Shared.Autenticaçao;
using Shared.Requests;
using Shared.Responses;
namespace Shared.Interfaces
{
    public class AuthenticationService : IAuthenticationService
    {
      
        public Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            AuthenticationResponse authenticationResponse = JwtAuthenticationManager.Aunthenticate(request);
          
            if (authenticationResponse == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid user Credentials"));
            }
            return Task.FromResult( authenticationResponse);
        }
    }
}




