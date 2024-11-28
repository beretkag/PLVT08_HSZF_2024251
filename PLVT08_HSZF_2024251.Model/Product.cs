using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PLVT08_HSZF_2024251.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [DisplayName("Név")]
        [Create]
        public string Name { get; set; }
        [Required]
        [DisplayName("Mennyiség")]
        public double Quantity { get; set; } = 0;
        [Required]
        [DisplayName("Kritikus szint")]
        [Create]
        public double CriticalLevel { get; set; }
        [Required]
        [DisplayName("Lejárat")]
        [Create]
        public DateTime BestBefore { get; set; }
        [Required]
        [DisplayName("Hűtős termék")]
        [Create]
        public bool StoreInFridge { get; set; }



        public string ToStringByUser(Person user)
        {
            return $"{Name}{new string(' ', 15 - Name.Length)}" +
                $"{Quantity}{new string(' ', 15 - Quantity.ToString().Length)}"+
                $"{BestBefore.ToShortDateString()}{new string(' ', 15 - BestBefore.ToShortDateString().Length)}" +
                $"{(user.FavoriteProducts.Count(x => x == this) > 0 ? $"[ X ]" : "[   ]")}";
        }

        public string ToString()
        {
            return $"{Name}{new string(' ', 15 - Name.Length)}" +
                $"{Quantity}{new string(' ', 15 - Quantity.ToString().Length)}" +
                $"{BestBefore.ToShortDateString()}{new string(' ', 15 - BestBefore.ToShortDateString().Length)}";
        }

        public static string HeadLineByUser()
        {
            return "Név            Mennyiség      Lejárat        Kedvenc";
        }
        public static string HeadLine()
        {
            return "Név            Mennyiség      Lejárat";
        }


    }
}
