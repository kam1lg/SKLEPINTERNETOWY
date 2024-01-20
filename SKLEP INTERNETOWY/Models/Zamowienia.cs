using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLEP_INTERNETOWY.Models
{
    public class Zamowienia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdZamowienia { get; set; }

        public string? IdUzytkownika { get; set; }
        [ForeignKey("IdUzytkownika")]
        public ApplicationUser? Uzytkownik { get; set; }

        [Column(TypeName = "int")]
        public int Kwota { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Adres { get; set; }


        public int? IdProduktu { get; set; }
        [ForeignKey("IdProduktu")]
        public Produkty? Produkt { get; set; }
    }
}
