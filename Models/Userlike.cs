using System.Text.Json.Serialization;

namespace Backend_Test.Models;

public class UserLike
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BookId { get; set; } = string.Empty;
}

// แก้ไข: เพิ่ม [JsonPropertyName] ให้รับ snake_case ตามที่โจทย์กำหนด
// { "user_id": xxx, "book_id": 1 }
public class LikeRequest
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("book_id")]
    public string BookId { get; set; } = string.Empty;
}
