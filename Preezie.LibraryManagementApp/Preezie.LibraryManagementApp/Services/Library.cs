using Microsoft.EntityFrameworkCore;
using Preezie.LibraryManagementApp.Data;
using Preezie.LibraryManagementApp.Models;

namespace Preezie.LibraryManagementApp.Services;

public class Library
{
    private static Library _instance;
    //private List<Book> books;

    private readonly ApplicationDbContext _db;

    private Library()
    {
        //Manually initialized DbContext in this singleton
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        _db = new ApplicationDbContext(optionsBuilder.Options);

        //books = new List<Book>();
    }

    public static Library Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Library();
            }
            return _instance;
        }
    }

    public async Task<List<Book>> ListBooksAsync()
    {
        return await _db.Books.ToListAsync();
    }

    public async Task<Book?> GetBookAsync(int bookId)
    {
        return await _db.Books.FindAsync(bookId);
    }

    public async Task<Result<Book>> AddBookAsync(Book book)
    {
        try
        {
            _db.Set<Book>().Add(book);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result<Book>.Failure($"Failed to add book: {ex.Message}");
        }

        return Result<Book>.Success(book);
    }

    public async Task<Result<Book>> BorrowBook(int bookId)
    {
        var book = _db.Set<Book>().Find(bookId);
        if (book == null)
        {
            return Result<Book>.Failure($"Book with Id {bookId} was not found.");
        }

        if (book.IsBorrowed)
        {
            return Result<Book>.Failure($"Book with Id {bookId} is already borrowed and not yet returned");
        }

        try
        {
            book.IsBorrowed = true;
            _db.Entry(book).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result<Book>.Failure($"Failed to borrow book: {ex.Message}");
        }

        return Result<Book>.Success(book);
    }

    public bool ReturnBook(int bookId)
    {
        var book = _db.Set<Book>().Find(bookId);
        if (book != null && book.IsBorrowed)
        {
            book.IsBorrowed = false;
            return true;
        }
        return false;
    }
}

