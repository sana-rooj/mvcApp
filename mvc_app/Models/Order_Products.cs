using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models
{
    public class Order_Products
    {
        [Key]
        public int SerialNo { get; set; }
        [ForeignKey("Order_Id")]
        public virtual Order Order_ref { get; set; }

        [ForeignKey("Product_Id")]
        public virtual Product Product_ref { get; set; }
    }
}
