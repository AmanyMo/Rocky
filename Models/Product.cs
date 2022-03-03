using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Rocky.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string  Description { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(8,2)")]
        [Range(0,int.MaxValue)]
        public Decimal Price { get; set; }


        public string Image { get; set; }


        [Display(Name ="Category Type")]  
        public int CategoryId { get; set; }

       [ ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}
