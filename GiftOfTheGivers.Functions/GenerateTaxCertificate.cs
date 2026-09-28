using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Functions
{
    public class GenerateTaxCertificate
    {
        private readonly ILogger<GenerateTaxCertificate> _logger;

        public GenerateTaxCertificate(ILogger<GenerateTaxCertificate> logger)
        {
            _logger = logger;
        }

        [Function("GenerateTaxCertificate")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Generating dummy tax certificate...");

            // Read query parameters from the HTTP request
            string donorName = req.Query["donorName"].ToString() ?? "Valued Donor";
            string amountStr = req.Query["amount"].ToString() ?? "0";

            // Build dummy tax certificate payload
            var certificate = new
            {
                CertificateNumber = $"TAX-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                IssueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Organization = "Gift of the Givers Foundation",
                Section18ACompliant = true,
                DonorName = donorName,
                AmountDonated = amountStr,
                Status = "Issued",
                Message = "Thank you for your generous contribution. This certificate serves as tax-deductible proof."
            };

            return new OkObjectResult(certificate);
        }
    }
}