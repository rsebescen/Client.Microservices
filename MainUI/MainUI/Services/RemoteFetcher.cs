using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MainUI.Services
{
    public class RemoteFetcher
    {
        private string remotePath;

        public RemoteFetcher(string remotePath)
        {
            this.remotePath = remotePath;
        }

        internal async Task<string> GetContent()
        {
            try
            {
                WebRequest request = WebRequest.Create(remotePath);
                var cont = await request.GetResponseAsync();
                var remoteDoc = new HtmlDocument();
                remoteDoc.Load(cont.GetResponseStream());

                return remoteDoc.ParsedText;
            }
            catch (Exception)
            {
                return "Page cannot be loaded ATM";
            }
        }

        internal async Task<RemoteHtmlData> GetHtml(string serviceUrlPrefix)
        {
            try
            {
                WebRequest request = WebRequest.Create(remotePath);
                var cont = await request.GetResponseAsync();

                var remoteStream = cont.GetResponseStream();
				StreamReader reader = new StreamReader(remoteStream);
				string responseText = reader.ReadToEnd();
				responseText = responseText.Replace("src=\"/", $"src=\"{serviceUrlPrefix}/")
									.Replace("href=\"/", $"href=\"{serviceUrlPrefix}/");

                var remoteDoc = new HtmlDocument();
                remoteDoc.LoadHtml(responseText);
                var body = remoteDoc.DocumentNode.SelectNodes("//body")[0];
                var head = remoteDoc.DocumentNode.SelectNodes("//head")[0];

                return new RemoteHtmlData
                {
                    Head = head,
                    Body = body
                };
            }
            catch (Exception _)
            {
                return new RemoteHtmlData
                {
                    Body = HtmlNode.CreateNode("Page cannot be loaded ATM"),
					Head = HtmlNode.CreateNode("")
                };
            }
        }
    }
}
