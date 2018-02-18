using HtmlAgilityPack;
using System;
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

        internal async Task<string> GetHtml()
        {
            try
            {
                WebRequest request = WebRequest.Create(remotePath);
                var cont = await request.GetResponseAsync();

                var remoteStream = cont.GetResponseStream();

                var remoteDoc = new HtmlDocument();
                remoteDoc.Load(remoteStream);
                var node = remoteDoc.DocumentNode.SelectNodes("//body")[0];
                return node.OuterHtml;
            }
            catch (Exception)
            {
                return "Page cannot be loaded ATM";
            }
        }
    }
}
