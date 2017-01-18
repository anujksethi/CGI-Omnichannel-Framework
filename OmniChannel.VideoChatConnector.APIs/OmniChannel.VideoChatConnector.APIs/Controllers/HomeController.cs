
using OmniChannel.VideoChatConnector.APIs.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OmniChannel.VideoChatConnector.APIs.Controllers
{
    public class HomeController : Controller
    {
        VideoConnectorContext VideoContext = new VideoConnectorContext();
        public ActionResult Index()
        {
            //VideoContext.Database.GetStats();
            //return Json(VideoContext.Database.Server.BuildInfo, JsonRequestBehavior.AllowGet);




            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
