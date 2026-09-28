using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GiftOfTheGivers.Web.Controllers
{
    [Authorize(Roles = "Employee")]
    public class OperationsController : Controller
    {
        // GET: /Operations/VolunteerSignups
        public IActionResult VolunteerSignups()
        {
            // Logic to fetch and display registered volunteers
            return View();
        }

        // POST: /Operations/PostProjectUpdate
        [HttpPost]
        public IActionResult PostProjectUpdate(int projectId, string updateNotes)
        {
            // Logic to save a project update to the database
            return RedirectToAction("Index", "Home");
        }
    }
}
