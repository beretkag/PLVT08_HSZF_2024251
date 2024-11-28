
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PLVT08_HSZF_2024251.Model
{
    public class FavoriteProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string PersonId { get; set; }
        [Required]
        public string ProductId { get; set; }
    }
}
