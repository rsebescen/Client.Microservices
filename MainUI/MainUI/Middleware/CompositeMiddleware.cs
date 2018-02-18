using MainUI.Domain;
using MainUI.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainUI.Middleware
{
    public class CompositeMiddleware
    {
        private readonly RequestDelegate _next;

        public CompositeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var compositeRequest = new CompositeContext(httpContext, Pages);

            if (compositeRequest.RequestType.Equals(CompositeRequestType.NotSupported))
            {
                return;
            }

            var handler = CompositeContentResolver.Resolve(compositeRequest);

            await handler.Handle(_next);
        }

        private static CompositePage[] Pages = new[] {
            new CompositePage
            {
                Name = "Alo",
                MatchString = "/alo",
                BaseUrl = "http://localhost:8080"
            },
            new CompositePage
            {
                Name = "Brb",
                MatchString = "/brb",
                BaseUrl = "http://localhost:3000"
            }
        };
    }

}
