using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Category
    {
        //[Key]
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        [Required, Display(Name ="Display order") , Range(1,int.MaxValue,ErrorMessage ="Displau order for category must be ...") ,]
        public int DisplayOrder { get; set; }
    }
}
