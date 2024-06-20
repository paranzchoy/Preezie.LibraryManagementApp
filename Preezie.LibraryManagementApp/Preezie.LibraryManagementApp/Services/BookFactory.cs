using Preezie.LibraryManagementApp.Models;

namespace Preezie.LibraryManagementApp.Services;

// Factory Pattern for Book creation
public static class BookFactory
{
    //private static int _nextId = 1;

    public static Book CreateBook(string title, string author, string isbn)
    {
        return new Book
        {
            //BookId = _nextId++,
            Title = title,
            Author = author,
            ISBN = isbn,
            IsBorrowed = false
        };
    }
}

