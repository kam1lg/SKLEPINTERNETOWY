using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLEP_INTERNETOWY.Models
{
    public class Kategorie
    {
       
        [Key]
        public int IdKategorii { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Nazwa { get; set; }

 
    }
}
