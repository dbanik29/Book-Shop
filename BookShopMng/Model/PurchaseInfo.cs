using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMng.Model
{
    [Table("UserPurchaseInfo")]
    public class PurchaseInfo
    {
        [Key]
        public int PurchaseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string BookName { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}
