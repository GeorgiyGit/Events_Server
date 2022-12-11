
namespace Core.DTOs.CommentDTOs
{
    public class CommentCreateDTO
    {
        public string Text { get; set; } = "";

        public int? ParentId { get; set; }
        public int? EventId { get; set; }
        public int? PlaceId { get; set; }
    }
}
