using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainUI.Services
{
    public class RemoteHtmlData
    {
        public HtmlNode Head { get; set; }
        public HtmlNode Body { get; set; }
    }
}
