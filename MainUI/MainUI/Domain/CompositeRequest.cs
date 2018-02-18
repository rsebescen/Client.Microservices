using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainUI.Services;
using Microsoft.AspNetCore.Http;

namespace MainUI.Domain
{
    public class CompositeContext
    {
        private CompositePage compositePage;

        public HttpContext HttpContext;
        internal CompositeRequestType RequestType;

        public CompositeContext(HttpContext httpContext, CompositePage[] pages)
        {
            HttpContext = httpContext;
            compositePage = pages.FirstOrDefault(DoesMatch);
            RequestType = DistinguishRequestType();
        }

        private string RequestPath => HttpContext.Request.Path.Value;

        public string RemotePath => $"{compositePage.BaseUrl}{RequestPath.Replace(compositePage.MatchString, "")}";

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

            if (RequestPath.Contains("/assets/") || RequestPath.Contains(".js")
                    || (RequestPath.Contains("/api/") && HttpContext.Request.Method.Equals("GET")))
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
