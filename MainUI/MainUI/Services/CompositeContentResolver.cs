﻿using HtmlAgilityPack;
using MainUI.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MainUI.Services
{
    public class CompositeContentResolver
    {
        public static ARemoteContentHandler Resolve(CompositeContext compositeRequest)
        {
            ARemoteContentHandler handler;
            switch (compositeRequest.RequestType)
            {
                case CompositeRequestType.Asset:
                    handler = new AssetHandler(compositeRequest);
                    break;
                case CompositeRequestType.Rest:
                    handler = new RestHandler(compositeRequest);
                    break;
                case CompositeRequestType.Html:
                    handler = new HtmlHandler(compositeRequest);
                    break;
                case CompositeRequestType.NotSupported:
                default:
                    throw new InvalidOperationException("request cannot be handled");
            }

            return handler;
        }
    }

    public abstract class ARemoteContentHandler
    {
        protected CompositeContext CompositeContext;
        protected RemoteFetcher RemoteFetcher;

        protected ARemoteContentHandler(CompositeContext compositeRequest)
        {
            CompositeContext = compositeRequest;
            RemoteFetcher = new RemoteFetcher(compositeRequest.RemotePath);
        }

        public abstract Task Handle(RequestDelegate next);
    }

    public class AssetHandler : ARemoteContentHandler
    {
        public AssetHandler(CompositeContext compositeRequest)
            : base(compositeRequest)
        {

        }

        public override async Task Handle(RequestDelegate next)
        {
            var content = await RemoteFetcher.GetContent();

            await CompositeContext.HttpContext.Response.WriteAsync(content);
        }
    }

    public class RestHandler : ARemoteContentHandler
    {
        public RestHandler(CompositeContext compositeRequest) : base(compositeRequest)
        {
        }

        public override async Task Handle(RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }

    public class HtmlHandler : ARemoteContentHandler
    {
        public HtmlHandler(CompositeContext compositeContext) : base(compositeContext)
        {
        }

        public override async Task Handle(RequestDelegate next)
        {
            var response = string.Empty;

            var remoteHtml = await RemoteFetcher.GetHtml();
            remoteHtml = remoteHtml.Replace("src=\"", $"src=\"{CompositeContext.MatchString}");

            var existingBody = CompositeContext.HttpContext.Response.Body;
            using (var newBody = new MemoryStream())
            {
                // We set the response body to our stream so we can read after the chain of middlewares have been called.
                CompositeContext.HttpContext.Response.Body = newBody;

                await next(CompositeContext.HttpContext);

                CompositeContext.HttpContext.Response.Body = existingBody;

                newBody.Seek(0, SeekOrigin.Begin);

                // newContent will be `Hello`.
                response = new StreamReader(newBody).ReadToEnd();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(response);

                var remoteNodes = new HtmlNodeCollection(null);
                remoteNodes.Add(HtmlNode.CreateNode(remoteHtml));

                doc.DocumentNode.SelectNodes("//body/div")[0].PrependChildren(remoteNodes);
                response = doc.DocumentNode.InnerHtml;

                // Send our modified content to the response body.
                await CompositeContext.HttpContext.Response.WriteAsync(response);
            }
        }
    }

    internal enum CompositeRequestType
    {
        Asset,
        Rest,
        Html,
        NotSupported
    }
}