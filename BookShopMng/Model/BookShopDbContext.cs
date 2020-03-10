using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMng.Model
{
    public class BookShopDbContext : DbContext
    {
        public BookShopDbContext(DbContextOptions<BookShopDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<BooksInformation> BookInfos { get; set; }
        public virtual DbSet<UsersInformation> UserInfos { get; set; }
        public virtual DbSet<PurchaseInfo> PurchaseInfos { get; set; }
    }
}
