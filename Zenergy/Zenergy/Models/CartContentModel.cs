using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zenergy.Models
{
    public class CartContentModel
    {
        public int userId { get; set; }
        public int productId { get; set; }
        public int productQuantity { get; set; }
    }
}