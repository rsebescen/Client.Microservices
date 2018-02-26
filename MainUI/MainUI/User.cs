using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainUI.Domain;

namespace MainUI
{
    public class User
    {
        public static MenuItem[] Menu = new[]
        {
            new MenuItem
            {
                Name = "Index",
                Url = "/Home/Index"
            },
            new MenuItem
            {
                Name = "About",
                Url = "/Home/About"
            },
            new MenuItem
            {
                Name = "Contact",
                Url = "/Home/Contact"
            }
        };
    }
}
