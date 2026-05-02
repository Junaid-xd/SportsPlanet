using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SportsPlanet.Helpers
{
    public static class PdfGeneratorHelper
    {
        public static string GenerateBill(
            List<ReceiptLine> lines,
            User user,
            double grandTotal,
            int orderId,
            DateTime date)
        {
            string folder = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "GeneratedBills"));

            Directory.CreateDirectory(folder);

            string fileName = $"SportsPlanet_Bill_{orderId}.pdf";
            string path = Path.Combine(folder, fileName);

            using var document = new PdfDocument();
            document.Info.Title = $"Sports Planet Bill #{orderId}";

            var page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            using var gfx = XGraphics.FromPdfPage(page);

            // SAFE FONTS (NO ARIAL = NO CRASH)
            var titleFont = new XFont("Verdana", 16, XFontStyleEx.Bold);
            var headerFont = new XFont("Verdana", 11, XFontStyleEx.Bold);
            var textFont = new XFont("Verdana", 10, XFontStyleEx.Regular);

            double y = 40;

            // ================= HEADER =================
            gfx.DrawString("SPORTS PLANET", titleFont, XBrushes.Black,
                new XRect(0, y, page.Width, 20),
                XStringFormats.TopCenter);

            y += 30;

            gfx.DrawString($"Invoice #: {orderId}", textFont, XBrushes.Black, 40, y);
            y += 16;

            gfx.DrawString($"Date: {date:dd MMM yyyy hh:mm tt}", textFont, XBrushes.Black, 40, y);
            y += 16;

            gfx.DrawString($"Customer: {user.Name}", textFont, XBrushes.Black, 40, y);
            y += 24;

            // ================= TABLE HEADER =================
            gfx.DrawString("Product", headerFont, XBrushes.Black, 40, y);
            gfx.DrawString("Qty", headerFont, XBrushes.Black, 300, y);
            gfx.DrawString("Price", headerFont, XBrushes.Black, 400, y);

            y += 10;
            gfx.DrawLine(XPens.Black, 40, y, 550, y);
            y += 15;

            // ================= ITEMS =================
            foreach (var line in lines)
            {
                gfx.DrawString(line.ProductName, textFont, XBrushes.Black, 40, y);
                gfx.DrawString(line.Quantity.ToString(), textFont, XBrushes.Black, 300, y);
                gfx.DrawString($"Rs. {line.TotalPrice}", textFont, XBrushes.Black, 400, y);

                y += 16;
            }

            // ================= TOTAL =================
            y += 10;
            gfx.DrawLine(XPens.Black, 40, y, 550, y);

            y += 20;

            gfx.DrawString($"Total: Rs. {grandTotal}", headerFont, XBrushes.Black, 350, y);

            y += 30;

            gfx.DrawString(
                "Thank you for shopping with Sports Planet!",
                textFont,
                XBrushes.Black,
                new XRect(0, y, page.Width, 20),
                XStringFormats.TopCenter
            );

            document.Save(path);

            return path;
        }
    }
}