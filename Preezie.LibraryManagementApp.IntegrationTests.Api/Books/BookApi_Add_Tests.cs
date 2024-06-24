using Preezie.LibraryManagementApp.Models;
using System.Net;
using System.Net.Http.Json;

namespace Preezie.LibraryManagementApp.IntegrationTests.Api.Books;


[Collection("BookApiTests")]
public class BookApi_Add_Tests
{
    private readonly BookApiTestFixture _fixture;

    public BookApi_Add_Tests(BookApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddBook_BookIsEmpty_Return400BadRequest()
    {
        //ARRANGE
        var httpClient = _fixture.HttpClientAuthorized;
        var emptyBook = new Book();

        //ACT
        var url = "api/Books";
        var response = await httpClient.PostAsJsonAsync(url, emptyBook);

        //ASSERT
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddBook_ShouldReturnValidDataSuccessfully()
    {
        //ARRANGE
        var httpClient = _fixture.HttpClientAuthorized;
        var dto = new Book()
        {
            Title = "Test",
            Author = "Test",
            ISBN = "6987605",
            IsBorrowed = false
        };

        //ACT
        var url = "api/Books";
        var response = await httpClient.PostAsJsonAsync(url, dto);

        //ASSERT
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}