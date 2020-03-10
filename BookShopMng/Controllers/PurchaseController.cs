using BookShopMng.Model;
using BookShopMng.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopMng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        readonly IBookPurchaseService _bookPurchaseService;
        readonly IUserService _userService;
        readonly IBookService _bookService;
        IHttpContextAccessor _httpcontext;
        public PurchaseController(IBookPurchaseService bookPurchaseService, IUserService userService, IBookService bookService, IHttpContextAccessor httpcontext)
        {
            _bookPurchaseService = bookPurchaseService;
            _userService = userService;
            _bookService = bookService;
            _httpcontext = httpcontext;
        }

        [HttpGet]
        [Route("GetAllBooksToUser")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllBooksToUser()
        {
            try
            {
                var books = await _bookPurchaseService.GetBooksInfo();
                if (books == null)
                {
                    return NotFound();
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddPurchasedBook")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddPurchasedBook(int bookid)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var username = _httpcontext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    var usermodel = _userService.GetUsersInfo().Result.FirstOrDefault(x => x.UserName == username);
                    var bookmodel = _bookService.GetBooksInfo().Result.FirstOrDefault(x => x.BookId == bookid);
                    var model = new PurchaseInfo()
                    {
                        FirstName = usermodel.FirstName,
                        LastName = usermodel.LastName,
                        UserName = usermodel.UserName,
                        BookName = bookmodel.Name,
                        Category = bookmodel.Category,
                        Price = bookmodel.Price,
                        PurchaseDate = DateTime.Now
                    };
                    var modeladd = await _bookPurchaseService.PurchaseBook(model);
                    if (modeladd > 0)
                    {
                        return Ok(new { message = "Book Purchasing Successfully Done" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Book Purchasing failed" });
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}