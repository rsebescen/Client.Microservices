using MainUI.Domain;
using MainUI.Services;
using MainUI.ValueObjects;
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
        private readonly CompositePages _pages;

        public CompositeMiddleware(RequestDelegate next, CompositePages pages)
        {
            _next = next;
            _pages = pages;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var compositeRequest = new CompositeContext(httpContext, _pages);

            if (compositeRequest.RequestType.Equals(CompositeRequestType.NotSupported))
            {
                return;
            }

            var handler = CompositeContentResolver.Resolve(compositeRequest);

            await handler.Handle(_next);
        }
    }

}
