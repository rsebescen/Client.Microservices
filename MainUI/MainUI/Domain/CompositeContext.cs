using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainUI.Services;
using MainUI.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace MainUI.Domain
{
    public class CompositeContext
    {
        private CompositePage compositePage;

        public HttpContext HttpContext;
        internal CompositeRequestType RequestType;

        public CompositeContext(HttpContext httpContext, CompositePages pages)
        {
            HttpContext = httpContext;
            compositePage = pages.ThatMatch(DoesMatch);
            RequestType = DistinguishRequestType();
        }

        private string RequestPath => HttpContext.Request.Path.Value;

        public string RemotePath => $"{compositePage.BaseUrl.Substring(0, compositePage.BaseUrl.Length-1)}{RequestPath.Replace(MatchString, "")}";

        public string MatchString => compositePage.MatchString;

        private bool DoesMatch(CompositePage compositePage)
        {
            return RequestPath.StartsWith(compositePage.MatchString);
        }

        private CompositeRequestType DistinguishRequestType()
        {
            if (compositePage == null)
            {
                return CompositeRequestType.NotSupported;
            }

            if (RequestPath.Contains("/assets/") || RequestPath.Contains(".js") || RequestPath.Contains(".css"))
            {
                return CompositeRequestType.Asset;
            }

            if (RequestPath.Contains("/api/"))
            {
                return CompositeRequestType.Rest;
            }

            return CompositeRequestType.Html;
        }
    }
}
