using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLEP_INTERNETOWY.Models
{
    public class Produkty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProduktu { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Nazwa { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public int Cena { get; set; }

        public int? IdKategorii { get; set; }
        [ForeignKey("IdKategorii")]
        public Kategorie? Kategoria { get; set; }
    }
}
