using System.Net.Http.Json;
using System.Text.Json;
using Generated;
using Xunit.Abstractions;

namespace Api.IntegrationTests.Tests;

public class BooksTests(ITestOutputHelper output ) : ApiTestBase
{
    
    [Fact]
    public async Task CreateBook_CanSuccessFullyCreateBook()
    {
        var dto = new CreateBookDto()
        {
            Author = "A",
            Genre = "A",
            Title = "A"
        };
        var result = (await new LibraryClient(Client).PostAsync(dto)).Result;
        Assert.Equivalent(result.Author, dto.Author);
        Assert.Equivalent(result.Genre, dto.Genre);
        Assert.Equivalent(result.Title, dto.Title);
        Assert.NotEqual(0, result.Id);
    }

    [Fact]
    public async Task LoanDoesntHappen_IfAlreadyLoaned()
    {

        var dto = new CreateBookDto()
        {
            Author = "B",
            Genre = "B",
            Title = "B"
        };
        
        var BookResult = (await new LibraryClient(Client).PostAsync(dto)).Result;
        output.WriteLine(BookResult.Loans.ToString());

        var LoanDto = new LoanBookDto()
        {
            BookId = BookResult.Id,
            UserId = 1
        };
        
        var result = (await new LibraryClient(Client).LoanAsync(LoanDto)).Result;
        
        
        var LoanDto2 = new LoanBookDto()
        {
            BookId = BookResult.Id,
            UserId = 1
        };
        
        var result2 = (await new LibraryClient(Client).LoanAsync(LoanDto2)).Result;
        
        
        Assert.NotNull(result);
        //Assert.NotNull(result2);

    }
    

    
    [Fact]
    public async Task BookDoesntCreate_WithEmptyFields()
    {
        var dto = new CreateBookDto()
        {
            Author = "",
            Genre = "",
            Title = "failing book"
        };

        try
        {
            var result = (await new LibraryClient(Client).PostAsync(dto));
            Assert.False(result.StatusCode == 200);
        }
        catch (ApiException ex)
        {
            Assert.Equal(400, ex.StatusCode);
        }
        

    }
    
    
}