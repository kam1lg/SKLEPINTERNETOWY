using Microsoft.AspNetCore.Mvc;
using SKLEP_INTERNETOWY.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.EntityFrameworkCore;

namespace SKLEP_INTERNETOWY.Controllers
{
    public class SklepInternetowyController : Controller
    {
        private readonly SklepInternetowyDbContext _context;

        public SklepInternetowyController(SklepInternetowyDbContext context)
        {
            _context = context;
        }

        // GET: SklepInternetowy
        public IActionResult Index()
        {
            var productsWithCategories = _context.Produkty
                .Include(p => p.Kategoria)
                .Select(p => new Produkty
                {
                    IdProduktu = p.IdProduktu,
                    Nazwa = p.Nazwa,
                    Cena = p.Cena,
                    Kategoria = p.Kategoria
                })
                .ToList();

            return View(productsWithCategories);
        }



        // GET: SklepInternetowy/Details/5
       

        public IActionResult Details(int id)
            {
                var product = _context.Produkty
                    .Include(p => p.Kategoria)  // Include the related category
                    .FirstOrDefault(p => p.IdProduktu == id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }


    // GET: SklepInternetowy/Create
    [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Kategorie, "IdKategorii", "Nazwa");
            return View();
        }

        // POST: SklepInternetowy/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(Produkty produkt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Wczytaj ID kategorii z formularza i przypisz je do modelu Produktu
                    produkt.IdKategorii = Convert.ToInt32(HttpContext.Request.Form["IdKategorii"]);

                    // Dodaj nowy produkt do bazy danych
                    _context.Produkty.Add(produkt);
                    _context.SaveChanges();

                    Console.WriteLine("Operacja CREATE zakończona sukcesem!");

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania produktu: {ex.Message}");
                Console.WriteLine(ex.StackTrace); // Dodaj stack trace do logów
            }

            // Jeśli model nie jest poprawny, wróć do widoku create z błędami
            ViewBag.Categories = new SelectList(_context.Kategorie, "IdKategorii", "Nazwa");
            return View(produkt);
        }

        // GET: SklepInternetowy/Edit/5
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var product = _context.Produkty.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Kategorie.ToList(), "IdKategorii", "Nazwa", product.IdKategorii);
            return View(product);
        }

        // POST: SklepInternetowy/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Produkty produkt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    produkt.IdKategorii = Convert.ToInt32(HttpContext.Request.Form["IdKategorii"]);
                    _context.Produkty.Update(produkt);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas edytowania produktu: {ex.Message}");
            }

            ViewBag.Categories = new SelectList(_context.Kategorie.ToList(), "IdKategorii", "Nazwa", produkt.IdKategorii);
            return View(produkt);
        }




        // GET: SklepInternetowy/Delete/5
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var product = _context.Produkty.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: SklepInternetowy/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _context.Produkty.Find(id);
                if (product != null)
                {
                    _context.Produkty.Remove(product);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania produktu: {ex.Message}");
            }

            return View();
        }
    }
}
