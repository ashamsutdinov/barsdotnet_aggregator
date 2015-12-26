using System.Web.Mvc;
using Aggregator.Contracts;
using UI.Models;

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

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            return Json(new {message = "form submitted", model, success=true});
           // return Redirect("/");
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
