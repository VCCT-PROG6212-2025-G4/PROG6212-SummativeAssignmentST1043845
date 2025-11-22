using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using DinkToPdf;
using DinkToPdf.Contracts; // if using DinkToPdf
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CMCS_Web_App.Services
{
    public class ReportService
    {
        private readonly AppDbContext _db;
        private readonly IConverter? _pdfConverter; // nullable: only if DinkToPdf registered
        public ReportService(AppDbContext db, IConverter? pdfConverter = null)
        {
            _db = db;
            _pdfConverter = pdfConverter;
        }

        // Fetch approved claims in date range
        public async Task<List<Claim>> GetApprovedClaimsAsync(DateTime from, DateTime to)
        {
            return await _db.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved && c.DateSubmitted >= from && c.DateSubmitted <= to)
                .OrderBy(c => c.Lecturer.LastName).ThenBy(c => c.DateSubmitted)
                .ToListAsync();
        }

        // Build a simple HTML summary report (tailwind classes can be used in layout)
        public string BuildApprovedClaimsHtml(IEnumerable<Claim> claims, DateTime from, DateTime to)
        {
            var sb = new StringBuilder();
            sb.Append($@"<html><head><meta charset='utf-8'><title>Approved Claims Report</title>
                        <style>body{{font-family: Arial, Helvetica, sans-serif;}} table{{width:100%;border-collapse:collapse}} th,td{{border:1px solid #ddd;padding:8px}} th{{background:#111827;color:white}}</style>
                        </head><body>");
            sb.Append($"<h1>Approved Claims Report</h1>");
            sb.Append($"<p>From {from:yyyy-MM-dd} to {to:yyyy-MM-dd}</p>");
            sb.Append("<table><thead><tr><th>Claim ID</th><th>Lecturer</th><th>Email</th><th>Hours</th><th>Rate (R)</th><th>Total (R)</th><th>Date</th></tr></thead><tbody>");
            foreach (var c in claims)
            {
                var rate = c.HourlyRate > 0 ? c.HourlyRate : (c.Lecturer?.RatePerHour ?? 0m);
                var total = Math.Round(c.HoursWorked * rate, 2);
                sb.Append($"<tr><td>{c.ClaimId}</td><td>{c.Lecturer?.FirstName} {c.Lecturer?.LastName}</td><td>{c.Lecturer?.Email}</td><td>{c.HoursWorked}</td><td>{rate:C}</td><td>{total:C}</td><td>{c.DateSubmitted:yyyy-MM-dd}</td></tr>");
            }
            sb.Append("</tbody></table></body></html>");
            return sb.ToString();
        }

        // Build a single invoice HTML for a lecturer
        public string BuildInvoiceHtml(string lecturerName, string lecturerEmail, IEnumerable<Claim> claims, DateTime from, DateTime to)
        {
            var sb = new StringBuilder();
            sb.Append("<html><head><meta charset='utf-8'><title>Invoice</title>");
            sb.Append("<style>body{font-family: Arial;} table{width:100%;border-collapse:collapse} th,td{border:1px solid #ddd;padding:8px} th{background:#111827;color:white}</style>");
            sb.Append("</head><body>");
            sb.Append($"<h2>Invoice for {lecturerName}</h2>");
            sb.Append($"<p>Email: {lecturerEmail}</p>");
            sb.Append($"<p>Period: {from:yyyy-MM-dd} to {to:yyyy-MM-dd}</p>");
            sb.Append("<table><thead><tr><th>Claim ID</th><th>Date</th><th>Hours</th><th>Rate</th><th>Total (R)</th></tr></thead><tbody>");
            decimal grandTotal = 0m;
            foreach (var c in claims)
            {
                var rate = c.HourlyRate > 0 ? c.HourlyRate : (c.Lecturer?.RatePerHour ?? 0m);
                var total = Math.Round(c.HoursWorked * rate, 2);
                grandTotal += total;
                sb.Append($"<tr><td>{c.ClaimId}</td><td>{c.DateSubmitted:yyyy-MM-dd}</td><td>{c.HoursWorked}</td><td>{rate:C}</td><td>{total:C}</td></tr>");
            }
            sb.Append($"</tbody><tfoot><tr><th colspan='4' style='text-align:right'>Grand Total</th><th>{grandTotal:C}</th></tr></tfoot></table>");
            sb.Append("</body></html>");
            return sb.ToString();
        }


        // Convert HTML to PDF using DinkToPdf (optional)
        public byte[]? ConvertHtmlToPdf(string html)
        {
            if (_pdfConverter == null) return null;

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };
            return _pdfConverter.Convert(doc);
        }
    }
}

