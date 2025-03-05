using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportTA.Data;
using ReportTA.Models;


namespace ReportTA.Controllers
{
    public class TaxController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaxController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult TaxIndex()
        {
            return View();
        }

        
        public IActionResult Calculate()
        {
            return View();
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateTax([FromBody] TaxRequest request)
        {
            if (request.Income <= 0)
            {
                return BadRequest("Invalid income amount.");
            }

            double tax = 0;
            if (request.Regime.ToLower() == "old")
            {
                tax = CalculateTaxOldRegime(request.Income);
            }
            else if (request.Regime.ToLower() == "new")
            {
                tax = CalculateTaxNewRegime(request.Income);
            }
            else
            {
                return BadRequest("Invalid tax regime. Choose 'old' or 'new'.");
            }

            double cess = tax * 0.04;
            double totalTax = tax + cess;

            
            var taxEntry = new TaxRequest
            {
                Income = request.Income,
                Regime = request.Regime,
                TaxAmount = tax,
                Cess = cess,
                TotalTaxPayable = totalTax,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaxRequests.Add(taxEntry);
            await _context.SaveChangesAsync();

            return Ok(taxEntry);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetTaxHistory()
        {
            var history = await _context.TaxRequests.OrderByDescending(t => t.CreatedAt).ToListAsync();
            return Ok(history);
        }

        private double CalculateTaxOldRegime(double income)
        {
            double taxableIncome = income - 50000;

            if (taxableIncome > 150000)
                taxableIncome -= 150000;
            else
                taxableIncome = 0;

            if (taxableIncome <= 250000) return 0;
            else if (taxableIncome <= 500000) return (taxableIncome - 250000) * 0.05;
            else if (taxableIncome <= 1000000) return (250000 * 0.05) + ((taxableIncome - 500000) * 0.20);
            else return (250000 * 0.05) + (500000 * 0.20) + ((taxableIncome - 1000000) * 0.30);
        }

        private double CalculateTaxNewRegime(double income)
        {
            if (income <= 300000) return 0;
            else if (income <= 600000) return (income - 300000) * 0.05;
            else if (income <= 900000) return (300000 * 0.05) + ((income - 600000) * 0.10);
            else if (income <= 1200000) return (300000 * 0.05) + (300000 * 0.10) + ((income - 900000) * 0.15);
            else if (income <= 1500000) return (300000 * 0.05) + (300000 * 0.10) + (300000 * 0.15) + ((income - 1200000) * 0.20);
            else return (300000 * 0.05) + (300000 * 0.10) + (300000 * 0.15) + (300000 * 0.20) + ((income - 1500000) * 0.30);
        }

    }
}
