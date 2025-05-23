using Microsoft.AspNetCore.Mvc;

namespace ShoppingMall.Controllers
{
    public class OrdinaryPatialController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("_TitlePartial");
        }

    }
}
