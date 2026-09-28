using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.Controllers
{
    public class VolunteersController : Controller
    {
        // Simple in-memory list to track prototype signups
        public static readonly List<VolunteerRegistrationModel> VolunteerRoster = new();

        // GET: /Volunteers/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Volunteers/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(VolunteerRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            VolunteerRoster.Add(model);
            TempData["SuccessMessage"] = $"Thank you, {model.FullName}! Your registration interest has been recorded.";
            return RedirectToAction("Confirmation");
        }

        // GET: /Volunteers/Confirmation
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
