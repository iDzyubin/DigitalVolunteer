using Microsoft.AspNetCore.Mvc;

namespace DigitalVolunteer.Web.Controllers
{
    public class ExecutorController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();
    }
}