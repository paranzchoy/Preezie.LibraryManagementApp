using Preezie.LibraryManagementApp.Models;
using System.Net;
using System.Net.Http.Json;

namespace Preezie.LibraryManagementApp.IntegrationTests.Api.Books;

[Collection("BookApiTests")]
public class BookApi_GetDetails_Tests
{
    private readonly BookApiTestFixture _fixture;
    private Book _seedBook = default!;
    private Bogus.Faker _faker = new Bogus.Faker();

    public BookApi_GetDetails_Tests(BookApiTestFixture fixture)
    {
        _fixture = fixture;
        SeedData().GetAwaiter().GetResult();
    }

    private async Task SeedData()
    {
        _seedBook = new Book()
        {
            Title = "Test",
            Author = "Test",
            ISBN = "6987605",
            IsBorrowed = false
        };
        _fixture.DbContext.Books.Add(_seedBook);
        await _fixture.DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Get_EntityNotExisting_ShouldReturn404NotFound()
    {
        //ARRANGE
        var notExistingId = 87645445;
        var httpClient = _fixture.HttpClientAuthorized;

        //ACT
        var url = $"api/Books/{notExistingId}";
        var response = await httpClient.GetAsync(url);

        //ASSERT
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnValidDataSuccessfully()
    {
        //ARRANGE
        var existingBook = _seedBook;

        var httpClient = _fixture.HttpClientAuthorized;

        //ACT
        var url = $"api/Books/{existingBook.BookId}";
        var result = await httpClient.GetFromJsonAsync<Book>(url);

        //ASSERT
        Assert.IsType<Book>(result);
        Assert.Equal(existingBook.BookId, result.BookId);
    }
}