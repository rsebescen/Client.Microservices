using HtmlAgilityPack;
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
            WebRequest request = WebRequest.Create(remotePath);
            var cont = await request.GetResponseAsync();
            var remoteDoc = new HtmlDocument();
            remoteDoc.Load(cont.GetResponseStream());

            return remoteDoc.ParsedText;
        }

        internal async Task<string> GetHtml()
        {
            WebRequest request = WebRequest.Create(remotePath);
            var cont = await request.GetResponseAsync();

            var remoteStream = cont.GetResponseStream();

            var remoteDoc = new HtmlDocument();
            remoteDoc.Load(remoteStream);
            var node = remoteDoc.DocumentNode.SelectNodes("//body")[0];
            return node.OuterHtml;
        }
    }
}
