using Core.DTOs.UserDTOs;

namespace Core.DTOs.CommentDTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public DateTime CreationTime { get; }

        public bool IsChanged { get; set; }

        public int? ParentId { get; set; }
        //public CommentDTO? Parent { get; set; }

        //public ICollection<CommentDTO> SubComments { get; } = new HashSet<CommentDTO>();

        public string OwnerId { get; set; }
        public string OwnerUserName { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        //public ICollection<UserBase> LikedUsers { get; set; } = new HashSet<UserBase>();
        //public ICollection<UserBase> DislikedUsers { get; set; } = new HashSet<UserBase>();
    }
}
