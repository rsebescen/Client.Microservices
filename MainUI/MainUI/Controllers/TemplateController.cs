using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainUI.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
