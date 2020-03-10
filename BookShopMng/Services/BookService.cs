using BookShopMng.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMng.Services
{
    public interface IBookService
    {
        Task<List<BooksInformation>> GetBooksInfo();
        Task<BooksInformation> GetBooksInfoById(int? BookId);
        Task<int> AddBook(BooksInformation book);
        Task<int> DeleteBook(int? BookId);
        Task UpdateBook(BooksInformation book);
    }
    public class BookService : IBookService
    {
        BookShopDbContext _context;
        public BookService(BookShopDbContext bookShopContext)
        {
            _context = bookShopContext;
        }
        public async Task<List<BooksInformation>> GetBooksInfo()
        {
            if (_context != null)
            {
                return await _context.BookInfos.ToListAsync();
            }
            return null;
        }
        public async Task<BooksInformation> GetBooksInfoById(int? BookId)
        {
            if (_context != null)
            {
                return await (from b in _context.BookInfos
                              where b.BookId == BookId
                              select new BooksInformation
                              {
                                  BookId = b.BookId,
                                  Name = b.Name,
                                  Price = b.Price,
                                  Category = b.Category,
                                  Author = b.Author
                              }).FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<int> AddBook(BooksInformation book)
        {
            if(_context != null)
            {
                await _context.BookInfos.AddAsync(book);
                await _context.SaveChangesAsync();
                return book.BookId;
            }
            return 0;
        }
        public async Task<int> DeleteBook(int? BookId)
        {
            int result = 0;
            if (_context != null)
            {
                var book = await _context.BookInfos.FirstOrDefaultAsync(x => x.BookId == BookId);
                if (book != null)
                {
                    _context.Remove(book);
                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }
        public async Task UpdateBook(BooksInformation book)
        {
            if (_context != null)
            {
                _context.BookInfos.Update(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
