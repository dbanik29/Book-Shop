
using BookShopMng.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BookShopMng.Model;

namespace BookShopMng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Route("GetAllBooks")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetBooksInfo();
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

        [HttpGet]
        [Route("GetBookById")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetBookById(int? BookId)
        {
            if (BookId == null)
            {
                return BadRequest();
            }
            try
            {
                var book = await _bookService.GetBooksInfoById(BookId);
                if(book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add([FromBody]BooksInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _bookService.AddBook(model);
                    if(book > 0)
                    {
                        return Ok(book);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update([FromBody]BooksInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.UpdateBook(model);
                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? BookId)
        {
            int result = 0;
            if (BookId == null)
            {
                return BadRequest();
            }
            try
            {
                result = await _bookService.DeleteBook(BookId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
