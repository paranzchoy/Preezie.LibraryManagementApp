using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Preezie.LibraryManagementApp.Models;
using Preezie.LibraryManagementApp.Services;

namespace Preezie.LibraryManagementApp.Features;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly Library _library;

    public BooksController()
    {
        _library = Library.Instance;
    }

    [HttpGet]
    public async Task<ActionResult<List<Book>>> ListBooks()
    {
        var data = await _library.ListBooksAsync();

        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book?>> GetBook(int id)
    {
        var book = await _library.GetBookAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book?>> AddBook([FromBody] Book book)
    {
        var newBook = BookFactory.CreateBook(book.Title, book.Author, book.ISBN);
        await _library.AddBookAsync(newBook);
        return CreatedAtAction(nameof(GetBook), new { id = newBook.BookId }, newBook);
    }

    [HttpPatch("{id}/borrow")]
    public async Task<IActionResult> BorrowBook(int id)
    {
        var result = await _library.BorrowBook(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        var errorResponse = new
        {
            result.ErrorMessage,
        };

        return BadRequest(errorResponse);
    }

    [HttpPatch("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var result = await _library.ReturnBookAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        var errorResponse = new
        {
            result.ErrorMessage,
        };

        return BadRequest(errorResponse);
    }
}
