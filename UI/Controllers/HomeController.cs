using System.Web.Mvc;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return PartialView();
        }
        
        public ActionResult LoginProcess()
        {

           
            ViewBag.Text = "qwerty";
           return View();
        }
    }
}
