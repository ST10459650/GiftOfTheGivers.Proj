using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace GiftOfTheGivers.Web.Controllers
{
    [AllowAnonymous] // Allows both guest users and logged-in donors to access public actions
    public class DonationsController : Controller
    {
        // In-memory store for prototype demonstration
        private static readonly List<DonationModel> _donations = new();
        private readonly IHttpClientFactory _httpClientFactory;

        // Inject IHttpClientFactory into constructor
        public DonationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Donations/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new DonationModel();
            return View(model);
        }

        // POST: /Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.DonationId = _donations.Count + 1;
            model.DonationDate = DateTime.Now;

            // Updated logic that resolves donor identity cleanly with a guaranteed non-null string
            string safeDonorName = model.DonorName ?? string.Empty;

            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Donor"))
            {
                if (string.IsNullOrWhiteSpace(safeDonorName))
                {
                    safeDonorName = User.Identity.Name ?? "Valued Donor";
                }
            }
            else if (model.IsAnonymous || string.IsNullOrWhiteSpace(safeDonorName))
            {
                safeDonorName = "Anonymous Contributor";
            }

            model.DonorName = safeDonorName;

            // Call Azure Function to generate Section 18A tax certificate number
            try
            {
                var client = _httpClientFactory.CreateClient();
                string functionUrl = $"https://giftofthegiversfunction26-hbg5gfhhgvdne3d0.southafricanorth-01.azurewebsites.net/api/GenerateTaxCertificate={Uri.EscapeDataString(safeDonorName)}&amount={model.Amount}";

                HttpResponseMessage response = await client.GetAsync(functionUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    using var doc = JsonDocument.Parse(jsonResult);
                    if (doc.RootElement.TryGetProperty("certificateNumber", out var certNum) ||
                        doc.RootElement.TryGetProperty("CertificateNumber", out certNum))
                    {
                        // Fix potential null assignment from GetString()
                        model.CertificateNumber = certNum.GetString() ?? string.Empty;
                    }
                }
            }
            catch
            {
                // Fallback receipt reference if Azure Function service is offline
                model.CertificateNumber = $"GOTG-SEC18A-{DateTime.Now:yyyyMMdd}-{model.DonationId:D4}";
            }

            // Fallback safety check if certificate number is still empty
            if (string.IsNullOrEmpty(model.CertificateNumber))
            {
                model.CertificateNumber = $"GOTG-SEC18A-{DateTime.Now:yyyyMMdd}-{model.DonationId:D4}";
            }

            _donations.Add(model);

            return RedirectToAction("TaxCertificate", new { id = model.DonationId });
        }

        // GET: /Donations/TaxCertificate/1
        [HttpGet]
        public IActionResult TaxCertificate(int id)
        {
            var donation = _donations.FirstOrDefault(d => d.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: /Donations/History
        [Authorize(Roles = "Donor")]
        [HttpGet]
        public IActionResult History()
        {
            var userEmail = User.Identity?.Name;
            var userDonations = _donations
                .Where(d => d.DonorEmail == userEmail || d.DonorName == userEmail)
                .ToList();

            return View(userDonations);
        }
    }
}