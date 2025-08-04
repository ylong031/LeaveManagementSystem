using LeaveManagementSystem.Application.Models;

namespace LeaveManagementSystem.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            /*var data = new TestViewModel
            {
                Name = "Student"
            };*/

            var data = new TestViewModel();
            data.Name = "Student";
            data.DateOfBirth = new DateTime(1954, 12, 01);

            return View(data);
        }
    }
}
