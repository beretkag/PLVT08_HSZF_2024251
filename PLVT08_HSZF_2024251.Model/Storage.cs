using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PLVT08_HSZF_2024251.Model
{
    public class Storage
    {
        [Key]
        public bool Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Capacity { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        public double UsedCapacity { get => Products.Sum(x => x.Quantity); }


        public override string? ToString()
        {
            return $"{Name}{new string(' ', 15 - Name.Length)}" +
                $"{Capacity}{new string(' ', 15 - Capacity.ToString().Length)}";
        }



    }
}
