namespace ProjectAPI.DTO
{
    public class GetPost
    {
        public string PostId { get; set; }
        public string? TypeOfBody { get; set; }
        public string? header { get; set; }
        public string? Text { get; set; }
        public List<string>? Images { get; set; }

        public int? likes { get; set; }
        public DateTime? postDate { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserImage { get; set; }

    }
}
