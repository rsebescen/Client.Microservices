using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainUI.Domain
{
    public class CompositePage
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string MatchString { get; set; }
        public MenuItem[] MenuItems { get; set; }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Url { get; set; }  
    }
}
