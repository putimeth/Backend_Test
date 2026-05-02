using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Backend_Test.Data;
using Backend_Test.Models;

namespace Backend_Test.Controllers;

[ApiController]
[Authorize]
public class BookController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string BooksApiUrl = "https://api.itbook.store/1.0/search/mysql";
    public BookController(ApiDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// GET /books
    /// Fetch books from IT Book Store API, sorted A-Z by title
    /// </summary>
    [HttpGet("books")]
public async Task<IActionResult> GetBooks()
{
    try
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(10);
        var response = await client.GetAsync(BooksApiUrl);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode,
                new { message = "Failed to fetch books from external API." });

        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<BookApiResponse>(json);

        if (apiResponse == null || apiResponse.Error != "0")
            return BadRequest(new { message = "Invalid response from external API." });

        var sortedBooks = apiResponse.Books
            .OrderBy(b => b.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return Ok(new { total = apiResponse.Total, books = sortedBooks });
    }
    catch (HttpRequestException)
    {
        var mockBooks = new[]
        {
            new { title = "Beginning MySQL Database Design and Optimization", isbn13 = "9781484242216", price = "$35.99" },
            new { title = "Learning MySQL", isbn13 = "9780596008642", price = "$39.99" },
            new { title = "MySQL Cookbook", isbn13 = "9781492093022", price = "$49.99" },
            new { title = "MySQL Crash Course", isbn13 = "9780672327650", price = "$24.99" },
            new { title = "Pro MySQL NDB Cluster", isbn13 = "9781484229811", price = "$44.99" }
        }.OrderBy(b => b.title);

        return Ok(new
        {
            total = "5",
            books = mockBooks,
            note = "External API unavailable. Showing mock data."
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
    }
}

    /// <summary>
    /// POST /user/like
    /// Like a book and store it for the user
    /// </summary>
    [HttpPost("user/like")]
    public async Task<IActionResult> LikeBook([FromBody] LikeRequest request)
    {
        try
        {
            // Validate input
            if (request.UserId <= 0 || string.IsNullOrWhiteSpace(request.BookId))
            {
                return BadRequest(new { message = "UserId and BookId are required." });
            }

            // Check that user exists
            var userExists = await _db.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
            {
                return NotFound(new { message = $"User with ID {request.UserId} not found." });
            }

            // Check for duplicate like
            var alreadyLiked = await _db.UserLikes
                .AnyAsync(ul => ul.UserId == request.UserId && ul.BookId == request.BookId);

            if (alreadyLiked)
            {
                return Conflict(new { message = "User has already liked this book." });
            }

            var like = new UserLike
            {
                UserId = request.UserId,
                BookId = request.BookId
            };

            _db.UserLikes.Add(like);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Book liked successfully.", like });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while liking the book.", detail = ex.Message });
        }
    }
}
