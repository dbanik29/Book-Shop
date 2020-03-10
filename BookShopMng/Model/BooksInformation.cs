using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace BookShopMng.Model
{
    [Table("BookInfo")]
    public class BooksInformation
    {
        [Key]
        public int BookId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}
