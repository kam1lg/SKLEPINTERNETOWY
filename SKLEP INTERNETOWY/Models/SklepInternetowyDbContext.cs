using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SKLEP_INTERNETOWY.Models;

public class SklepInternetowyDbContext : IdentityDbContext<ApplicationUser>
{
    public SklepInternetowyDbContext(DbContextOptions<SklepInternetowyDbContext> options) : base(options)
    {
        
    }

    public DbSet<Produkty> Produkty { get; set; }
    public DbSet<Kategorie> Kategorie { get; set; }
    public DbSet<Zamowienia> Zamowienia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategorie>().HasData(
            new Kategorie { IdKategorii = 1, Nazwa = "RTV" },
            new Kategorie { IdKategorii = 2, Nazwa = "AGD" },
            new Kategorie { IdKategorii = 3, Nazwa = "Budowlane" },
            new Kategorie { IdKategorii = 4, Nazwa = "Sportowe" },
            new Kategorie { IdKategorii = 5, Nazwa = "Inne" }
        );

        modelBuilder.Entity<Produkty>().HasData(

            new Produkty { IdProduktu = 1, Nazwa = "Telewizor", Cena = 1000, IdKategorii = 1 },
            new Produkty { IdProduktu = 2, Nazwa = "Telefon komórkowy", Cena = 2000, IdKategorii = 1 },
            new Produkty { IdProduktu = 3, Nazwa = "Pralka", Cena = 1700, IdKategorii = 2 },
            new Produkty { IdProduktu = 4, Nazwa = "Lodówka", Cena = 2000, IdKategorii = 2 },
            new Produkty { IdProduktu = 5, Nazwa = "Panele", Cena = 500, IdKategorii = 3 },
            new Produkty { IdProduktu = 6, Nazwa = "Płytki", Cena = 750, IdKategorii = 3 },
            new Produkty { IdProduktu = 7, Nazwa = "Rower", Cena = 5000, IdKategorii = 4 },
            new Produkty { IdProduktu = 8, Nazwa = "Deskorolka", Cena = 500, IdKategorii = 4 },
            new Produkty { IdProduktu = 9, Nazwa = "Kawa", Cena = 45, IdKategorii = 5 },
            new Produkty { IdProduktu = 10, Nazwa = "Napój", Cena = 5, IdKategorii = 5 }
            );
    }
}
