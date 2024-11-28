using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLVT08_HSZF_2024251.Model
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id {  get; set; }
        [Create]
        [DisplayName("Név")]
        [Required]
        public string Name { get; set; }

        [Create]
        [DisplayName("Bevásárlásért felelős")]
        [Required]
        public bool ResponsibleForPurchase { get; set; }

        public virtual ICollection<Product> FavoriteProducts { get; set; }
        public virtual ICollection<Mail> Mails { get; set; }

        public Person()
        {
            FavoriteProducts = new HashSet<Product>();
            Mails = new HashSet<Mail>();
        }
    }
}