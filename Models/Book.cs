using System.Text.Json.Serialization;

namespace Backend_Test.Models;

public class Book
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; } = string.Empty;

    [JsonPropertyName("isbn13")]
    public string Isbn13 { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class BookApiResponse
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    [JsonPropertyName("total")]
    public string Total { get; set; } = string.Empty;

    [JsonPropertyName("books")]
    public List<Book> Books { get; set; } = new();
}
