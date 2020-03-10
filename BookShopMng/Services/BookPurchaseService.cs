using BookShopMng.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMng.Services
{
    public interface IBookPurchaseService
    {
        Task<List<BooksInformation>> GetBooksInfo();
        Task<int> PurchaseBook(PurchaseInfo entity);
    }
    public class BookPurchaseService : IBookPurchaseService
    {
        BookShopDbContext _context;
        public BookPurchaseService(BookShopDbContext bookShopContext)
        {
            _context = bookShopContext;
        }
        public async Task<int> PurchaseBook(PurchaseInfo entity)
        {
            if(_context != null)
            {
                await _context.PurchaseInfos.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity.PurchaseId;
            }
            return 0;
        }
        public async Task<List<BooksInformation>> GetBooksInfo()
        {
            if (_context != null)
            {
                return await _context.BookInfos.ToListAsync();
            }
            return null;
        }
    }
}
