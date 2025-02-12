using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload.Response.GoogleAuthentication;
using Microsoft.AspNetCore.Http;

namespace Galini.Services.Interface
{
    public interface IGoogleAuthenticationService
    {
        public Task<GoogleAuthResponse> AuthenticateGoogleUser(HttpContext context);

    }
}
