using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKLEP_INTERNETOWY.Models;
using System;
using System.Linq;

namespace SKLEP_INTERNETOWY.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class OrderController : Controller
    {
        private readonly SklepInternetowyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(SklepInternetowyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                // Jeśli użytkownik jest zalogowany jako admin, pobierz wszystkie zamówienia
                var allOrders = _context.Zamowienia
                    .Include(o => o.Produkt)
                    .ThenInclude(p => p.Kategoria)
                    .ToList();

                return View(allOrders);
            }
            else
            {
                // Jeśli użytkownik nie jest adminem, pobierz zamówienia tylko dla danego użytkownika
                var user = _userManager.GetUserAsync(User).Result;
                var userOrders = _context.Zamowienia
                    .Include(o => o.Produkt)
                    .ThenInclude(p => p.Kategoria)
                    .Where(o => o.IdUzytkownika == user.Id)
                    .ToList();

                return View(userOrders);
            }
        }


        [Authorize(Roles = "user")]
        [HttpGet]
        public IActionResult PlaceOrder()
        {
            // Pobierz dostępne produkty do wyboru
            var availableProducts = _context.Produkty.ToList();

            // Przygotuj listę dla rozwijalnej listy
            ViewBag.AvailableProducts = availableProducts.Select(p => new SelectListItem
            {
                Value = p.IdProduktu.ToString(),
                Text = p.Nazwa
            }).ToList();

            return View();
        }
        [Authorize(Roles = "user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(Zamowienia zamowienie)
        {
            try
            {
                if (zamowienie.IdProduktu.HasValue)
                {
                    // Pobierz wybrany produkt na podstawie ID
                    var selectedProduct = _context.Produkty.Find(zamowienie.IdProduktu.Value);

                    if (selectedProduct != null)
                    {
                        var user = _userManager.GetUserAsync(User).Result;

                        var order = new Zamowienia
                        {
                            IdUzytkownika = user.Id,
                            Kwota = selectedProduct.Cena,
                            Adres = zamowienie.Adres,
                            IdProduktu = selectedProduct.IdProduktu
                        };

                        _context.Zamowienia.Add(order);
                        _context.SaveChanges();

                        Console.WriteLine("Operacja PLACE ORDER zakończona sukcesem!");

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas składania zamówienia: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return View(zamowienie);
        }
        




    }
}
