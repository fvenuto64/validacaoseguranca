using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ValidarSenha.Tests
{
    public class FakeRemoteAddressMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IPAddress fakeIPAddress = IPAddress.Parse("172.190.1.10");

        public FakeRemoteAddressMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext  httpContext)
        {
            httpContext.Connection.RemoteIpAddress = fakeIPAddress;

            await this.next(httpContext);
        }
    }
}
