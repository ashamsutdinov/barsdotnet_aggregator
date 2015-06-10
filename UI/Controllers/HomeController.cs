using System.Web.Mvc;
using Aggregator.Contracts;

namespace UI.Controllers
{
    public class HomeController :
        Controller
    {
        public ActionResult Index()
        {
            var userManager = Services.Factory.Get<IUserManager>();
            userManager.Register("login", "password");
            IUser user = userManager.CheckAndGet("login", "password");
            
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Text = "textt";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult LoginProcess()
        {
            ViewBag.Text = "qwerty";
            return View();
        }
    }
}
